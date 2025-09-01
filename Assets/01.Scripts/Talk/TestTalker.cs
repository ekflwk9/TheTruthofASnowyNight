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
                talk.ShowTalk(isKorean ? "��ȭ�� ���� �������� �˻��մϴ�." : "Start Talk");

                talk.ShowYesButton(isKorean ? "1�� ��ȭ �������� �̵��մϴ�." : "Show A Talk", 10);
                talk.ShowNoButton(isKorean ? "2�� ��ȭ �������� �̵��մϴ�." : "Show B Talk", 11);
                break;

            case 10:
                talk.ShowTalk(isKorean ? "���� ��ȭ ������ 1���Դϴ�." : "A Talk");

                talk.ShowYesButton(isKorean ? "2�� ��ȭ �������ε� �̵��մϴ�." : "Show B Talk", 11);
                talk.ShowNoButton(isKorean ? "��ȭ�� �����մϴ�." : "End Talk", -1);
                break;

            case 11:
                talk.ShowTalk(isKorean ? "���� 2�� ��ȭ �����Դϴ�." : "B Talk");

                talk.ShowYesButton(isKorean ? "��ȭ�� �����մϴ�." : "End Talk", -1);
                talk.ShowNoButton(null, 0);
                break;

            case -1:
                isTouch.ShowOutLine(true);
                talk.EndTalk(true);
                break;
        }
    }
}
