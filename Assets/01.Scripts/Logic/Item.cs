using System.IO;
using GameMathods;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Item : MonoBehaviour,
IStarting, IInteraction, IUse, IEnd, ISave, ILoad
{
    protected bool isUse = false;
    public abstract Vector3 showItemPos { get; protected set; } //rotation�� �����տ��� ����
    public abstract EventCode eventCode { get; protected set; } //��ø�Ǵ� �������� ��ġ���� �ʴ� ������ ���

    //[SerializeField] private EventCode eventCode; //���� �������� �������� ��ġ�Ǿ� �ִ� ������ ���

    public virtual void Starting()
    {
        //�̺�Ʈ�� �ʿ��� ��쿡�� �̺�Ʈ �Լ� �߰�
        if (eventCode != EventCode.None)
        {
            GameService.progress.Add(this);
            GameService.eventManager.AddEvent(eventCode, EventFunction);
        }

        //�ʿ� ��ġ�� �����۸� ȣ��Ǳ� ������ ��� ������Ʈ�� �ʿ���� �߰����� ����
        this.gameObject.AddComponent<TouchingComponent>().Starting();
    }

    public virtual void Save()
    {
        GameService.dataManager.AddData(new Data(this.name, 0, isUse));
    }

    public virtual void End()
    {
        //Ǯ�� �����۸� ȣ��
        if (SceneManager.GetActiveScene().name == "Start")
        {
            if (GameService.progress.isLoad) isUse = GameService.dataManager.FindData(this.name).boolean;
        }

        //�ʿ� ��ġ�� �����۸� ȣ��
        else
        {
            //���� ������ �ö� Ȱ��ȭ ���θ� ���� ��뿩�� ������ ����
            GameService.dataManager.AddData(new Data(this.name, 0, isUse));

            //����� �̺�Ʈ�� �ִٸ� ���� �ٲܶ� �̺�Ʈ ����
            if (eventCode != EventCode.None) GameService.eventManager.RemoveEvent(eventCode);
        }
    }

    public virtual void Load()
    {
        if (SceneManager.GetActiveScene().name != "Start")
        {
            //���� Start�� �ƴ϶�� ������ �����͸� �ҷ���
            isUse = GameService.dataManager.FindData(this.name).boolean;

            //�κ��丮�� ���� ��� ������ ��Ȱ��ȭ
            if (GameService.itemController.CheckInventory(this.name)) this.gameObject.SetActive(false);

            //������� ���� �̻� ������ Ȱ��ȭ
            else this.gameObject.SetActive(isUse ? false : true);
        }
    }

    public virtual void Interaction()
    {
        //�������� �Ծ��� ��쿡�� ��Ȱ��ȭ
        if (GameService.itemController.GetItem(this.name))
        {
            //Ǯ�� �������� �̹� �������� ��� Ǯ�� �����۰� ��ȣ�ۿ����� ����
            if (this.gameObject.activeSelf) this.gameObject.SetActive(false);

            //Ǯ�� �������� �������� ��� ��ġ�� �����۰� ��ȣ�ۿ����� ���� �� ������ �̺�Ʈ �Լ� ȣ��
            else GameService.eventManager.CallEvent(eventCode);
        }

        //�� �Ծ��� ��� Pointer Ȱ��ȭ
        else
        {
            GameService.eventManager.CallUi(UiCode.PointEnter);
        }
    }

    public abstract bool Use();

    protected virtual void EventFunction() => this.gameObject.SetActive(false);

    public void RemoveItem() => Destroy(this.gameObject);
}
