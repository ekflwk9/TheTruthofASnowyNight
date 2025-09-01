using GameMathods;
using UnityEngine;

public class TestTalker : Talk
{
    public override void Interaction()
    {
        var talk = GameService.talkManager;
        var isKorean = GameService.progress.isKorean;

        switch (talk.talkID)
        {
            case 0:
                isTouch.ShowOutLine(false);
                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "대화가 시작 가능한지 검사합니다." : "Start Talk");

                talk.ShowYesButton(isKorean ? "1번 대화 내용으로 이동합니다." : "Show A Talk", 10);
                talk.ShowNoButton(isKorean ? "2번 대화 내용으로 이동합니다." : "Show B Talk", 11);
                break;

            case 10:
                talk.ShowTalk(isKorean ? "현재 대화 내용은 1번입니다." : "A Talk");

                talk.ShowYesButton(isKorean ? "2번 대화 내용으로도 이동합니다." : "Show B Talk", 11);
                talk.ShowNoButton(isKorean ? "대화를 종료합니다." : "End Talk", -1);
                break;

            case 11:
                talk.ShowTalk(isKorean ? "이제 2번 대화 내용입니다." : "B Talk");

                talk.ShowYesButton(isKorean ? "대화를 종료합니다." : "End Talk", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                isTouch.ShowOutLine(true);
                talk.EndTalk(true);
                break;
        }
    }
}
