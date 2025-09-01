using UnityEngine;
using GameMathods;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class ResolutionButton : Button,
ISave, ILoad
{
    private bool isFullScreen = true;
    private TMP_Text buttonText;

    public override void Starting()
    {
        base.Starting();

        buttonText = Find.Child(transform, "ButtonText").gameObject.GetComponent<TMP_Text>();
        buttonText.text = "On";
    }

    public override void ChangeLenguage()
    {
        base.ChangeLenguage();

        if (isFullScreen) buttonText.text = GameService.progress.isKorean ? "ÄÑ±â" : "On";
        else buttonText.text = GameService.progress.isKorean ? "²ô±â" : "Off";
    }

    public void Save() => GameService.dataManager.AddData(new Data(this.name, 0, isFullScreen));

    public void Load()
    {
        if(!File.Exists(Application.dataPath + "/VEsA")) isFullScreen = true;
        else isFullScreen = GameService.dataManager.FindData(this.name).boolean;

        Screen.SetResolution(isFullScreen ? 1920 : 1280, isFullScreen ? 1080 : 720, isFullScreen);
        ChangeLenguage();
    }

    protected override void Click()
    {
        isFullScreen = !isFullScreen;
        Screen.SetResolution(isFullScreen ? 1920 : 1280, isFullScreen ? 1080 : 720, isFullScreen);
        ChangeLenguage();
    }
}
