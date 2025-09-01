using System.IO;
using GameMathods;
using UnityEngine;

public class StartContinueButton : Button,
IEnd
{
    public override void Starting()
    {
        base.Starting();

        //세이브 파일이 존재하지 않을 경우 이어하기 버튼 비활성화
        if (!File.Exists(Application.dataPath + "/VEsA")) this.gameObject.SetActive(false);
    }

    protected override void Click()
    {
        //게임 시작시 로드를 할 것인가?
        GameService.progress.CanLoad(true);

        GameService.eventManager.CallUi(UiCode.OnBlock);
        GameService.fade.PlayFade(false, StartFuntion);
    }

    private void StartFuntion()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        var saveSceneName = GameService.dataManager.FindData("SceneName").str;
        saveSceneName = saveSceneName == null ? "Play1" : saveSceneName;

        GameService.ChangeScene(saveSceneName, false);
        GameService.progress.StopAllController(false);

        GameService.eventManager.CallUi(UiCode.OnCamera);
        GameService.eventManager.CallUi(UiCode.OffBlock);

        GameService.fade.PlayFade(true);
    }

    public void End() => GameService.progress.Remove(this);
}
