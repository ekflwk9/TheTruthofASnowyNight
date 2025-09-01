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
                talk.ShowTalk(isKorean ? "날씨가 점점 안 좋아지고 있어요." : "The weather is getting worse.");

                talk.ShowYesButton("...", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.ShowTalk(isKorean ? "이대로라면 계속 등산하기 힘듭니다." : "We can't keep climbing at this rate.");

                talk.ShowYesButton("...", 2);
                talk.ShowNoButton(null, 0);
                break;

            case 2:
                talk.ShowTalk(isKorean ? "게다가 여기서 유명한 그 귀신이 또 나타났다고 하네요." : "Besides, they say that the famous ghost has appeared again.");

                talk.ShowYesButton(isKorean ? "귀신이요?" : "A ghost?", 3);
                talk.ShowNoButton(isKorean ? "귀신은 존재하지 않아요." : "There are no ghosts.", 4);
                break;

            case 3:
                talk.ShowTalk(isKorean ? "네. 여기서 꽤 유명한 귀신이에요. 그 귀신이 나왔다는 소리가 들려오면 항상 사망자가 발생하거든요." : "Yes. It's a very famous ghost around here, \nand there are always deaths when it's said to have come out.");

                talk.ShowYesButton("...", 5);
                talk.ShowNoButton(null, 0);
                break;

            case 4:
                talk.ShowTalk(isKorean ? "안 믿으셔도 어쩔 수 없습니다. 그 귀신이 나온 이상 저희 가이드들은 절대 등산을 하지 않거든요." : "If you don't believe me, I can't help it, \nbecause our guides never climb the mountain after that ghost.");

                talk.ShowYesButton("...", 5);
                talk.ShowNoButton(null, 0);
                break;

            case 5:
                talk.ShowTalk(isKorean ? "일단 저는 무당들과 함께 먼저 올라갔다 오겠습니다." : "I'll go up first with the shamans and then come back down.");

                talk.ShowYesButton("...", 6);
                talk.ShowNoButton(null, 0);
                break;

            case 6:
                talk.ShowTalk(isKorean ? "위에서 상황이 해결된다면 내려와서 어떻게 되었는지 알려드릴게요." : "If things are clear up there, \nI'll come back down and let you know what happened.");

                talk.ShowYesButton("...", 7);
                talk.ShowNoButton(null, 0);
                break;

            case 7:
                talk.ShowTalk(isKorean ? "제가 올 때까지 여기서 기다려주세요." : "Please wait here until I get back.");

                talk.ShowYesButton("...", 8);
                talk.ShowNoButton(null, 0);
                break;

            case 8:
                talk.ShowTalk(isKorean ? "제가 오기 전까지 깊은 잠에 빠지거나, 텐트 밖으로 나오면 안 돼요." : "You must not fall into a deep sleep or leave your tent until I arrive.");

                talk.ShowYesButton(isKorean ? "왜 밖에 나오면 안 되나요?" : "Why can't I go outside?", 9);
                talk.ShowNoButton(isKorean ? "왜 깊은 잠에 들면 안 되나요?" : "Why can't I fall into a deep sleep?", 11);
                break;

            case 9:
                talk.ShowTalk(isKorean ? "지금 상황에서 밖에 있으면 무슨 일이 벌어질지 모르거든요." : "Because you don't know what could happen if you go outside in this situation.");

                talk.ShowYesButton("...", 10);
                talk.ShowNoButton(null, 0);
                break;

            case 10:
                talk.ShowTalk(isKorean ? "동료가 텐트 밖으로 나가버려서 어쩔 수 없이 데려와야 하지 않는 이상 \n나가지 않는게 좋을거에요." : "Unless your coworker wanders out of the tent \nand you have no choice but to bring them back, you shouldn't leave.");

                talk.ShowYesButton("...", 14);
                talk.ShowNoButton(null, 0);
                break;

            case 11:
                talk.ShowTalk(isKorean ? "깊은 잠에 빠지면 동료에게 무슨 일이 생겨도 구할 수 없기 때문에 위험해요." : "If you fall into a deep sleep, it's dangerous because \nif something happens to your teammates, you won't be able to save them.");

                talk.ShowYesButton("...", 12);
                talk.ShowNoButton(null, 0);
                break;

            case 12:
                talk.ShowTalk(isKorean ? "만약 너무 졸리다면 조금씩 자면서 최대한 조절해 주세요." : "If you're too sleepy, try to control it by sleeping in small increments.");

                talk.ShowYesButton("...", 14);
                talk.ShowNoButton(null, 0);
                break;

            case 14:
                talk.ShowTalk(isKorean ? "그러면 올라갔다 오겠습니다. 꽤 오래 걸릴 수도 있으니 알아두시면 좋을 거예요." : "Then I'll have to go upstairs and come back down, \nwhich could take quite a while, so it's good to know.");

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
