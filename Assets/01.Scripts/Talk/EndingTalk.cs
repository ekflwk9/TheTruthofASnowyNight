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
                talk.ShowTalk(isKorean ? "�׳� ���� ���̵�� �Բ� �ϻ��ϱ�� �����ߴ�." : "That day I decided to descend with the guide.");

                talk.ShowYesButton("...", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.ShowTalk(isKorean ? "�ᱹ ��������Ʈ�� ������ ������ ���ߴ�." : "I didn't end up photographing the summit of Everest.");

                talk.ShowYesButton("...", 2);
                talk.ShowNoButton(null, 0);
                break;

            case 2:
                talk.ShowTalk(isKorean ? "�׳� �츮�� ���� ���� �ƹ����Ե� ������� �ʾҴ�." : "I didn't tell anyone about what we went through that day");

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
                talk.ShowTalk(isKorean ? "�츮�� �׳� ��������Ʈ�� ������ �����." : "We summited Everest that day.");

                talk.ShowYesButton("...", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.ShowTalk(isKorean ? "���� ��翡 �Ǹ��� �Ǿ���, ��������Ʈ ��ݿ� ������ ������ ����������." : "We were featured in all kinds of articles \nand became famous as the first team to climb Everest.");

                talk.ShowYesButton("...", 2);
                talk.ShowNoButton(null, 0);
                break;

            case 2:
                talk.ShowTalk(isKorean ? "�׳� �츮�� �Բ� �־��� �� ����� ����������?" : "Who was that person with us that day?");

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
                talk.ShowTalk(isKorean ? "�츮�� �׳� ��������Ʈ�� ������ �����." : "We summited Everest that day.");

                talk.ShowYesButton("...", 1);
                talk.ShowNoButton(null, 0);
                break;

            case 1:
                talk.ShowTalk(isKorean ? "���� ��翡 �Ǹ��� �Ǿ���, ��������Ʈ ��ݿ� ������ ������ ����������." : "We were in all kinds of articles, \nand we became famous as the first team to climb Everest.");

                talk.ShowYesButton("...", 2);
                talk.ShowNoButton(null, 0);
                break;

            case 2:
                talk.ShowTalk(isKorean ? "�׳� �츮�� ���� ���� �ƹ����Ե� ������� �ʾҴ�." : "We never told anyone about what happened that day.");

                talk.ShowYesButton("...", 3);
                talk.ShowNoButton(null, 0);
                break;

            case 3:
                talk.ShowTalk(isKorean ? "�׶� �츮�� ����ߴ� �ϵ��� ���� ��𼭺��� ����̾�����?" : "How much of what we witnessed was true?");

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
