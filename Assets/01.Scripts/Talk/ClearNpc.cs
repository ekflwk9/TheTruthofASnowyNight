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
                talk.ShowTalk(isKorean ? "조치는 취했지만 아직도 계속 그 귀신이 나온다는 소리가 들려오는 모양이에요." : "I've taken action, but I'm still hearing the ghosts.");

                talk.ShowYesButton("...", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.ShowTalk(isKorean ? "더 이상의 등산은 힘들 것 같아요." : "I don't think I can hike anymore.");

                talk.ShowYesButton(isKorean ? "그렇다면 하루만 더 있어보면 안 될까요?" : "So can we stay one more day?", 2);
                talk.ShowNoButton(isKorean ? "저희 여길 오기 위해서 정말 많은 돈을 투자했어요. 이대로 내려갈 수 없습니다." : "We've invested a lot of money to get here, and we can't go back down.", 2);
                break;

            case 2:
                talk.ShowTalk(isKorean ? "그렇다면 마지막으로 무당들과 한 번 더 확인해 보고 올게요." : "In that case, I'll check with the shamans one last time.");

                talk.ShowYesButton("...", 3);
                talk.ShowNoButton(null, 0);
                break;

            case 3:
                talk.ShowTalk(isKorean ? "제가 돌아왔을 때 모두 살아계시다면 그때 한 번 다시 생각해 보자고요." : "If you're all still alive when I get back, then we'll reconsider.");

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
