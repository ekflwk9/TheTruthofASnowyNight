using GameMathods;
using UnityEngine;

public class TalkManager
{
    public int talkID { get; private set; }
    public string talk { get; private set; }
    public string talkButton { get; private set; }
    public IInteraction talker { get; private set; }

    //대화를 시작할 경우 아래 순서대로 메서드 실행함
    public void SetTalker(IInteraction _talker)
    {
        talker = _talker;

        GameService.playerController.MoveAction(0);
        GameService.progress.StopAllController(true);

        //DropWindow가 필요 없는 게임이라면 해당 메서드 제외
        GameService.eventManager.CallUi(UiCode.OffDrop);
    }

    public void ShowTalk(string _talk)
    {
        if (talker == null) Debug.Log("대화자가 지정되지 않았음");

        talk = _talk;
        GameService.eventManager.CallUi(UiCode.OnTalk);
    }

    public void ShowYesButton(string _talk, int _talkID)
    {
        talkID = _talkID;
        talkButton = _talk;
        GameService.eventManager.CallUi(UiCode.OffYesButton);
    }

    public void ShowNoButton(string _talk, int _talkID)
    {
        talkID = _talkID;
        talkButton = _talk;
        GameService.eventManager.CallUi(UiCode.OffNoButton);
    }

    public void ShowButtonMenu()
    {
        GameService.eventManager.CallUi(UiCode.OnYesButton);
        GameService.eventManager.CallUi(UiCode.OnNoButton);
    }

    public void NextTalk(int _talkID)
    {
        talkID = _talkID;
        talker.Interaction();
    }

    public void EndTalk(bool _showPointWindow)
    {
        talkID = 0;
        talker = null;

        if (_showPointWindow)
        {
            GameService.eventManager.CallUi(UiCode.PointEnter);
        }

        else
        {
            GameService.eventManager.CallUi(UiCode.PointExit);
        }

        GameService.progress.StopAllController(false);
        GameService.eventManager.CallUi(UiCode.OffTalk);

        //DropWindow가 필요 없는 게임이라면 해당 메서드 제외
        GameService.eventManager.CallUi(UiCode.DropWindow);
    }
}
