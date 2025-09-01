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

        //���� ���� ���
        if (this.name == "InDoor")
        {
            otherDoor = Find.Child(this.transform.parent, "OutDoor").gameObject;
            isInDoor = true;
        }

        //�ٱ� ���� ���
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
        //�÷��̾� ������ �̵�
        GameService.playerController.transform.position = nextPos.position;

        //�� ��ġ ���� �ٲ�
        doorPos = otherDoor.transform.position;
        doorPos.y = this.transform.parent.position.y;
        otherDoor.transform.position = doorPos;

        doorPos = this.transform.position;
        doorPos.y = this.transform.parent.position.y + -50f;
        this.transform.position = doorPos;

        //���� ���� ���
        if (isInDoor)
        {
            GameService.eventManager.CallEvent(EventCode.OnOutSound);
            GameService.eventManager.CallEvent(EventCode.OnBreath);

            //�̺�Ʈ �ð��� ��쿡��
            if (gameRules.CheckEvent())
            {
                GameService.eventManager.CallEvent(EventCode.OnSnow);
            }

            //�̺�Ʈ �ð��� �ƴ϶�� ���� ����
            else
            {
                GameService.eventManager.CallEvent(EventCode.OffSlider);
                GameService.eventManager.CallUi(UiCode.OffMenu);

                GameService.itemController.ResetItem();
                GameService.gameManager.SetAllOffItem();

                GameService.ChangeScene("GameOver", false);
            }
        }

        //�ٱ��� ���� ���
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
