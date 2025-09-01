using GameMathods;
using UnityEngine;

public class Door : MonoBehaviour,
IStarting, IInteraction, IEnd
{
    private GameRules gameRules;
    private AudioClip sound;
    private Transform nextPos;
    private GameObject otherDoor;

    private Vector3 doorPos;
    private bool isInDoor = false;

    public void Starting()
    {
        sound = Find.SoundSource("Tent");
        nextPos = Find.Child(this.transform, "NextPos");

        //안쪽 문일 경우
        if (this.name == "InDoor")
        {
            otherDoor = Find.Child(this.transform.parent, "OutDoor").gameObject;
            isInDoor = true;
        }

        //바깥 문일 경우
        else
        {
            otherDoor = Find.Child(this.transform.parent, "InDoor").gameObject;
        }

        GameService.SetComponent(this);
    }

    public void Interaction()
    {
        GameService.soundManager.OnEffect(sound);
        GameService.progress.StopController(true);
        GameService.playerController.MoveAction(0);

        GameService.fade.PlayFade(false, EventFuntion);
    }

    private void EventFuntion()
    {
        //플레이어 밖으로 이동
        GameService.playerController.transform.position = nextPos.position;

        //문 위치 서로 바꿈
        doorPos = otherDoor.transform.position;
        doorPos.y = this.transform.parent.position.y;
        otherDoor.transform.position = doorPos;

        doorPos = this.transform.position;
        doorPos.y = this.transform.parent.position.y + -50f;
        this.transform.position = doorPos;

        //안쪽 문일 경우
        if (isInDoor)
        {
            GameService.eventManager.CallEvent(EventCode.OnOutSound);
            GameService.eventManager.CallEvent(EventCode.OnBreath);

            //이벤트 시간일 경우에만
            if (gameRules.CheckEvent())
            {
                GameService.eventManager.CallEvent(EventCode.OnSnow);
            }

            //이벤트 시간이 아니라면 게임 종료
            else
            {
                GameService.eventManager.CallEvent(EventCode.OffSlider);
                GameService.eventManager.CallUi(UiCode.OffMenu);

                GameService.itemController.ResetItem();
                GameService.gameManager.SetAllOffItem();

                GameService.ChangeScene("GameOver", false);
            }
        }

        //바깥쪽 문일 경우
        else
        {
            GameService.eventManager.CallEvent(EventCode.OffSnow);
            GameService.eventManager.CallEvent(EventCode.OnInSound);
            GameService.eventManager.CallEvent(EventCode.OffBreath);
        }

        GameService.fade.PlayFade(true);
        GameService.progress.StopController(false);
    }

    public void SetRules(GameRules _ruleComponent) => gameRules = _ruleComponent;

    public void End() => GameService.gameManager.Remove(this);
}
