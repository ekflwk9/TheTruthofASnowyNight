using GameMathods;
using UnityEngine;

public class StartButton : Button,
ILoad, IEnd
{
    public override void Starting()
    {
        base.Starting();
        GameService.progress.StopAllController(true);
    }

    public void Load()
    {
        GameService.eventManager.CallUi(UiCode.OffCamera);
        GameService.fade.PlayFade(true, null, 2f);
    }

    protected override void Click()
    {
        //게임 시작시 로드를 할 것인가?
        GameService.progress.CanLoad(false);

        GameService.eventManager.CallUi(UiCode.OnBlock);
        GameService.fade.PlayFade(false, StartFuntion);
    }

    private void StartFuntion()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameService.ChangeScene("StartAction", false);

        //test
        //GameService.ChangeScene("Play", false);
        //GameService.progress.StopAllController(false);
        //GameService.eventManager.CallUi(UiCode.OnCamera);

        GameService.eventManager.CallUi(UiCode.OffBlock);
        GameService.fade.PlayFade(true);
    }

    public void End() => GameService.progress.Remove(this);
}
