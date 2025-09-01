using System.IO;
using GameMathods;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Item : MonoBehaviour,
IStarting, IInteraction, IUse, IEnd, ISave, ILoad
{
    protected bool isUse = false;
    public abstract Vector3 showItemPos { get; protected set; } //rotation은 프리팹에서 설정
    public abstract EventCode eventCode { get; protected set; } //중첩되는 아이템이 배치되지 않는 게임일 경우

    //[SerializeField] private EventCode eventCode; //같은 아이템이 여러개가 배치되어 있는 게임일 경우

    public virtual void Starting()
    {
        //이벤트가 필요한 경우에만 이벤트 함수 추가
        if (eventCode != EventCode.None)
        {
            GameService.progress.Add(this);
            GameService.eventManager.AddEvent(eventCode, EventFunction);
        }

        //맵에 배치된 아이템만 호출되기 때문에 드랍 컴포넌트는 필요없어 추가되지 않음
        this.gameObject.AddComponent<TouchingComponent>().Starting();
    }

    public virtual void Save()
    {
        GameService.dataManager.AddData(new Data(this.name, 0, isUse));
    }

    public virtual void End()
    {
        //풀링 아이템만 호출
        if (SceneManager.GetActiveScene().name == "Start")
        {
            if (GameService.progress.isLoad) isUse = GameService.dataManager.FindData(this.name).boolean;
        }

        //맵에 배치된 아이템만 호출
        else
        {
            //씬을 나갔다 올때 활성화 여부를 위해 사용여부 데이터 저장
            GameService.dataManager.AddData(new Data(this.name, 0, isUse));

            //등록한 이벤트가 있다면 씬을 바꿀때 이벤트 삭제
            if (eventCode != EventCode.None) GameService.eventManager.RemoveEvent(eventCode);
        }
    }

    public virtual void Load()
    {
        if (SceneManager.GetActiveScene().name != "Start")
        {
            //씬이 Start가 아니라면 무조건 데이터를 불러옴
            isUse = GameService.dataManager.FindData(this.name).boolean;

            //인벤토리에 있을 경우 무조건 비활성화
            if (GameService.itemController.CheckInventory(this.name)) this.gameObject.SetActive(false);

            //사용하지 않은 이상 무조건 활성화
            else this.gameObject.SetActive(isUse ? false : true);
        }
    }

    public virtual void Interaction()
    {
        //아이템을 먹었을 경우에만 비활성화
        if (GameService.itemController.GetItem(this.name))
        {
            //풀링 아이템이 이미 켜져있을 경우 풀링 아이템과 상호작용으로 간주
            if (this.gameObject.activeSelf) this.gameObject.SetActive(false);

            //풀링 아이템이 꺼져있을 경우 배치된 아이템과 상호작용으로 간주 후 지정된 이벤트 함수 호출
            else GameService.eventManager.CallEvent(eventCode);
        }

        //못 먹었을 경우 Pointer 활성화
        else
        {
            GameService.eventManager.CallUi(UiCode.PointEnter);
        }
    }

    public abstract bool Use();

    protected virtual void EventFunction() => this.gameObject.SetActive(false);

    public void RemoveItem() => Destroy(this.gameObject);
}
