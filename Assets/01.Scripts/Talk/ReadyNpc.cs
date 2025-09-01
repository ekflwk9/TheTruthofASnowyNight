using UnityEngine;

public class ReadyNpc : Talk
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
                talk.ShowTalk(isKorean ? "������ ���� �� �������� �־��." : "The weather is getting worse.");

                talk.ShowYesButton("...", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.ShowTalk(isKorean ? "�̴�ζ�� ��� ����ϱ� ����ϴ�." : "We can't keep climbing at this rate.");

                talk.ShowYesButton("...", 2);
                talk.ShowNoButton(null, 0);
                break;

            case 2:
                talk.ShowTalk(isKorean ? "�Դٰ� ���⼭ ������ �� �ͽ��� �� ��Ÿ���ٰ� �ϳ׿�." : "Besides, they say that the famous ghost has appeared again.");

                talk.ShowYesButton(isKorean ? "�ͽ��̿�?" : "A ghost?", 3);
                talk.ShowNoButton(isKorean ? "�ͽ��� �������� �ʾƿ�." : "There are no ghosts.", 4);
                break;

            case 3:
                talk.ShowTalk(isKorean ? "��. ���⼭ �� ������ �ͽ��̿���. �� �ͽ��� ���Դٴ� �Ҹ��� ������� �׻� ����ڰ� �߻��ϰŵ��." : "Yes. It's a very famous ghost around here, \nand there are always deaths when it's said to have come out.");

                talk.ShowYesButton("...", 5);
                talk.ShowNoButton(null, 0);
                break;

            case 4:
                talk.ShowTalk(isKorean ? "�� �����ŵ� ��¿ �� �����ϴ�. �� �ͽ��� ���� �̻� ���� ���̵���� ���� ����� ���� �ʰŵ��." : "If you don't believe me, I can't help it, \nbecause our guides never climb the mountain after that ghost.");

                talk.ShowYesButton("...", 5);
                talk.ShowNoButton(null, 0);
                break;

            case 5:
                talk.ShowTalk(isKorean ? "�ϴ� ���� ������ �Բ� ���� �ö󰬴� ���ڽ��ϴ�." : "I'll go up first with the shamans and then come back down.");

                talk.ShowYesButton("...", 6);
                talk.ShowNoButton(null, 0);
                break;

            case 6:
                talk.ShowTalk(isKorean ? "������ ��Ȳ�� �ذ�ȴٸ� �����ͼ� ��� �Ǿ����� �˷��帱�Կ�." : "If things are clear up there, \nI'll come back down and let you know what happened.");

                talk.ShowYesButton("...", 7);
                talk.ShowNoButton(null, 0);
                break;

            case 7:
                talk.ShowTalk(isKorean ? "���� �� ������ ���⼭ ��ٷ��ּ���." : "Please wait here until I get back.");

                talk.ShowYesButton("...", 8);
                talk.ShowNoButton(null, 0);
                break;

            case 8:
                talk.ShowTalk(isKorean ? "���� ���� ������ ���� �ῡ �����ų�, ��Ʈ ������ ������ �� �ſ�." : "You must not fall into a deep sleep or leave your tent until I arrive.");

                talk.ShowYesButton(isKorean ? "�� �ۿ� ������ �� �ǳ���?" : "Why can't I go outside?", 9);
                talk.ShowNoButton(isKorean ? "�� ���� �ῡ ��� �� �ǳ���?" : "Why can't I fall into a deep sleep?", 11);
                break;

            case 9:
                talk.ShowTalk(isKorean ? "���� ��Ȳ���� �ۿ� ������ ���� ���� �������� �𸣰ŵ��." : "Because you don't know what could happen if you go outside in this situation.");

                talk.ShowYesButton("...", 10);
                talk.ShowNoButton(null, 0);
                break;

            case 10:
                talk.ShowTalk(isKorean ? "���ᰡ ��Ʈ ������ ���������� ��¿ �� ���� �����;� ���� �ʴ� �̻� \n������ �ʴ°� �����ſ���." : "Unless your coworker wanders out of the tent \nand you have no choice but to bring them back, you shouldn't leave.");

                talk.ShowYesButton("...", 14);
                talk.ShowNoButton(null, 0);
                break;

            case 11:
                talk.ShowTalk(isKorean ? "���� �ῡ ������ ���ῡ�� ���� ���� ���ܵ� ���� �� ���� ������ �����ؿ�." : "If you fall into a deep sleep, it's dangerous because \nif something happens to your teammates, you won't be able to save them.");

                talk.ShowYesButton("...", 12);
                talk.ShowNoButton(null, 0);
                break;

            case 12:
                talk.ShowTalk(isKorean ? "���� �ʹ� �����ٸ� ���ݾ� �ڸ鼭 �ִ��� ������ �ּ���." : "If you're too sleepy, try to control it by sleeping in small increments.");

                talk.ShowYesButton("...", 14);
                talk.ShowNoButton(null, 0);
                break;

            case 14:
                talk.ShowTalk(isKorean ? "�׷��� �ö󰬴� ���ڽ��ϴ�. �� ���� �ɸ� ���� ������ �˾Ƶνø� ���� �ſ���." : "Then I'll have to go upstairs and come back down, \nwhich could take quite a while, so it's good to know.");

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
