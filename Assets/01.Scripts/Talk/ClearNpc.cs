using UnityEngine;

public class ClearNpc : Talk
{
    public override void Starting()
    {
        LookPlayer();
        GameService.SetComponent(this);
    }

    private void FadeFunction()
    {
        GameService.ChangeScene("Play", false);
        GameService.eventManager.CallUi(UiCode.OffMenu);
        GameService.playerController.transform.position = Vector3.up;

        GameService.progress.StopController(false);
        GameService.fade.PlayFade(true, null, 0.7f);
    }

    public override void Interaction()
    {
        var talk = GameService.talkManager;
        var isKorean = GameService.progress.isKorean;

        switch (talk.talkID)
        {
            case 0:
                LookPlayer();
                GameService.camController.LookNpc(this.transform.position + Vector3.up * 1.6f);

                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "��ġ�� �������� ������ ��� �� �ͽ��� ���´ٴ� �Ҹ��� ������� ����̿���." : "I've taken action, but I'm still hearing the ghosts.");

                talk.ShowYesButton("...", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.ShowTalk(isKorean ? "�� �̻��� ����� ���� �� ���ƿ�." : "I don't think I can hike anymore.");

                talk.ShowYesButton(isKorean ? "�׷��ٸ� �Ϸ縸 �� �־�� �� �ɱ��?" : "So can we stay one more day?", 2);
                talk.ShowNoButton(isKorean ? "���� ���� ���� ���ؼ� ���� ���� ���� �����߾��. �̴�� ������ �� �����ϴ�." : "We've invested a lot of money to get here, and we can't go back down.", 2);
                break;

            case 2:
                talk.ShowTalk(isKorean ? "�׷��ٸ� ���������� ������ �� �� �� Ȯ���� ���� �ðԿ�." : "In that case, I'll check with the shamans one last time.");

                talk.ShowYesButton("...", 3);
                talk.ShowNoButton(null, 0);
                break;

            case 3:
                talk.ShowTalk(isKorean ? "���� ���ƿ��� �� ��� ��ư�ôٸ� �׶� �� �� �ٽ� ������ ���ڰ��." : "If you're all still alive when I get back, then we'll reconsider.");

                talk.ShowYesButton("...", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                talk.EndTalk(false);
                GameService.camController.LookNpc(Vector3.zero);

                GameService.progress.StopController(true);
                GameService.fade.PlayFade(false, FadeFunction);
                break;
        }
    }
}
