public class Dead : Talk
{
    public override void Starting()
    {
        GameService.eventManager.CallUi(UiCode.OnFilm);
        GameService.fade.PlayFade(false, FadeFunction, 0.1f);
        GameService.SetComponent(this);
    }

    public void FadeFunction() => Interaction();

    public override void Interaction()
    {
        GameService.eventManager.CallUi(UiCode.StopVolume);
        GameService.soundManager.OnBackGround(null);
        GameService.eventManager.CallEvent(EventCode.OffBreath);
        GameService.eventManager.CallEvent(EventCode.OnGameOver);
        GameService.eventManager.CallUi(UiCode.OffCamera); 

        var talk = GameService.talkManager;
        var isKorean = GameService.progress.isKorean;

        switch (talk.talkID)
        {
            case 0:
                GameService.fade.PlayFade(true, null, 5f);
                talk.SetTalker(this);

                talk.ShowTalk(isKorean ? "나의 기억은 여기가 끝이다." : "My memory ends here.");

                talk.ShowYesButton("...", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "나의 무덤은 이곳이 된 것이다." : "My grave has become this place.");

                talk.ShowYesButton("...", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                talk.EndTalk(false);
                GameService.ChangeScene("Start", true);
                break;
        }
    }
}
