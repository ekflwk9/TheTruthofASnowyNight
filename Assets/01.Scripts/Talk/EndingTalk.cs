using GameMathods;
using UnityEngine;

public class EndingTalk : Talk,
ILoad
{
    public override void Starting()
    {
        GameService.SetComponent(this);
        GameService.eventManager.AddEvent(EventCode.OffEndingBackGround, OffBackGround);
    }

    public void Load() => Interaction();

    private void OffBackGround() => this.gameObject.SetActive(false);

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
                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "그날 나는 가이드와 함께 하산하기로 결정했다." : "That day I decided to descend with the guide.");

                talk.ShowYesButton("...", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.ShowTalk(isKorean ? "결국 에베레스트의 정상을 찍지는 못했다." : "I didn't end up photographing the summit of Everest.");

                talk.ShowYesButton("...", 2);
                talk.ShowNoButton(null, 0);
                break;

            case 2:
                talk.ShowTalk(isKorean ? "그날 우리가 겪은 일은 아무에게도 얘기하지 않았다." : "I didn't tell anyone about what we went through that day");

                talk.ShowYesButton("...", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                talk.EndTalk(false);
                GameService.eventManager.CallEvent(EventCode.OnEnding);
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
                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "우리는 그날 에베레스트의 정상을 찍었다." : "We summited Everest that day.");

                talk.ShowYesButton("...", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.ShowTalk(isKorean ? "각종 기사에 실리게 되었고, 에베레스트 등반에 성공한 팀으로 유명해졌다." : "We were featured in all kinds of articles \nand became famous as the first team to climb Everest.");

                talk.ShowYesButton("...", 2);
                talk.ShowNoButton(null, 0);
                break;

            case 2:
                talk.ShowTalk(isKorean ? "그날 우리와 함께 있었던 그 사람은 누구였을까?" : "Who was that person with us that day?");

                talk.ShowYesButton("...", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                talk.EndTalk(false);
                GameService.eventManager.CallEvent(EventCode.OnEnding);
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
                talk.SetTalker(this);
                talk.ShowTalk(isKorean ? "우리는 그날 에베레스트의 정상을 찍었다." : "We summited Everest that day.");

                talk.ShowYesButton("...", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.ShowTalk(isKorean ? "각종 기사에 실리게 되었고, 에베레스트 등반에 성공한 팀으로 유명해졌다." : "We were in all kinds of articles, \nand we became famous as the first team to climb Everest.");

                talk.ShowYesButton("...", 2);
                talk.ShowNoButton(null, 0);
                break;

            case 2:
                talk.ShowTalk(isKorean ? "그날 우리가 겪은 일은 아무에게도 얘기하지 않았다." : "We never told anyone about what happened that day.");

                talk.ShowYesButton("...", 3);
                talk.ShowNoButton(null, 0);
                break;

            case 3:
                talk.ShowTalk(isKorean ? "그때 우리가 목격했던 일들은 과연 어디서부터 사실이었을까?" : "How much of what we witnessed was true?");

                talk.ShowYesButton("...", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                talk.EndTalk(false);
                GameService.eventManager.CallEvent(EventCode.OnEnding);
                break;
        }
    }
}
