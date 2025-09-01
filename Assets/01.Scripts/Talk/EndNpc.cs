using UnityEngine;

public class EndNpc : Talk
{
    public override void Starting()
    {
        LookPlayer();
        GameService.SetComponent(this);
    }

    private void EndFunction()
    {
        GameService.eventManager.CallEvent(EventCode.OnInSound);
        GameService.eventManager.CallUi(UiCode.OffCamera);

        GameService.ChangeScene("Ending", false);

        GameService.playerController.transform.position = Vector3.zero;
        GameService.fade.PlayFade(true);
    }

    private void Ending()
    {
        GameService.camController.LookNpc(Vector3.zero);

        GameService.progress.StopController(true);
        GameService.fade.PlayFade(false, EndFunction, 1f);
    }

    public override void Interaction()
    {
        //GoodEnding
        if (!GameService.progress.lostNpc[0] && !GameService.progress.lostNpc[1]) GoodEnding();

        //SecretEnding
        else if (!GameService.progress.lostNpc[0] && GameService.progress.lostNpc[1]) SecretEnding();

        //BadEnding
        else BadEnding();
    }

    private void BadEnding()
    {
        var talk = GameService.talkManager;
        var isKorean = GameService.progress.isKorean;

        switch (talk.talkID)
        {
            case 0:
                LookPlayer();
                GameService.camController.LookNpc(this.transform.position + Vector3.up * 1.6f);

                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "������� �������̴ٴ� ������ �����̳׿�." : "I'm so sorry to hear about the loss of your coworker.");

                talk.ShowYesButton("...", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.ShowTalk(isKorean ? "�׷��� ���� �� �̻� �� �ͽ��� ������ �ʴ� �� ���ƿ�." : "But I think he's not haunting you anymore.");

                talk.ShowYesButton("...", 2);
                talk.ShowNoButton(null, 0);
                break;

            case 2:
                talk.ShowTalk(isKorean ? "������ ������ �����." : "And it's a beautiful day.");

                talk.ShowYesButton("...", 3);
                talk.ShowNoButton(null, 0);
                break;

            case 3:
                talk.ShowTalk(isKorean ? "���� ��� ���� ���ϴ� �� ���� �� ���ƿ�." : "I think it's time to decide what to do.");

                talk.ShowYesButton("...", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                talk.EndTalk(false);
                Ending();
                break;
        }
    }

    private void SecretEnding()
    {
        var talk = GameService.talkManager;
        var isKorean = GameService.progress.isKorean;

        switch (talk.talkID)
        {
            case 0:
                LookPlayer();
                GameService.camController.LookNpc(this.transform.position + Vector3.up * 1.6f);

                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "��? �ٸ� ���� �������̴ٰ��?" : "What? There's no one else?");

                talk.ShowYesButton("...", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.ShowTalk(isKorean ? "���� �� ���̼� �ö�����ݾƿ�." : "There were two of you originally.");

                talk.ShowYesButton("...", 2);
                talk.ShowNoButton(null, 0);
                break;

            case 2:
                talk.ShowTalk(isKorean ? "���� �ǰ��ϼ̳� ���׿�. ����" : "You must have been tired. Haha.");

                talk.ShowYesButton("...", 3);
                talk.ShowNoButton(null, 0);
                break;

            case 3:
                talk.ShowTalk(isKorean ? "�׷��� ���� �� �̻� �� �ͽ��� ������ �ʴ� �� ���ƿ�." : "But I don't think that ghost is here anymore.");

                talk.ShowYesButton("...", 4);
                talk.ShowNoButton(null, 0);
                break;

            case 4:
                talk.ShowTalk(isKorean ? "������ ������ �����." : "And it's a beautiful day.");

                talk.ShowYesButton("...", 5);
                talk.ShowNoButton(null, 0);
                break;

            case 5:
                talk.ShowTalk(isKorean ? "�ٽ� ����� �����ϼŵ� �� �� �����ϴ�." : "I think you can start climbing again.");

                talk.ShowYesButton("...", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                talk.EndTalk(false);
                Ending();
                break;
        }
    }

    private void GoodEnding()
    {
        var talk = GameService.talkManager;
        var isKorean = GameService.progress.isKorean;

        switch (talk.talkID)
        {
            case 0:
                LookPlayer();
                GameService.camController.LookNpc(this.transform.position + Vector3.up * 1.6f);

                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "���� �� �̻� �� �ͽ��� ������ �ʴ� �� ���ƿ�." : "I don't think I'm haunted by it anymore.");

                talk.ShowYesButton("...", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.ShowTalk(isKorean ? "������ ������ �����." : "And the weather is great.");

                talk.ShowYesButton("...", 2);
                talk.ShowNoButton(null, 0);
                break;

            case 2:
                talk.ShowTalk(isKorean ? "�ٽ� ����� �����ϼŵ� �� �� �����ϴ�." : "I think you can start climbing again.");

                talk.ShowYesButton("...", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                talk.EndTalk(false);
                Ending();
                break;
        }
    }
}
