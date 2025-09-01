using System.IO;
using GameMathods;
using UnityEngine;

public class StartContinueButton : Button,
IEnd
{
    public override void Starting()
    {
        base.Starting();

        //���̺� ������ �������� ���� ��� �̾��ϱ� ��ư ��Ȱ��ȭ
        if (!File.Exists(Application.dataPath + "/VEsA")) this.gameObject.SetActive(false);
    }

    protected override void Click()
    {
        //���� ���۽� �ε带 �� ���ΰ�?
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
