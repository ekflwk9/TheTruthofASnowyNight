using UnityEngine;
using GameMathods;
using TMPro;

public class LenguageButton : Button,
ISave, ILoad
{
    private TMP_Text buttonText;

    public override void Starting()
    {
        base.Starting();
        buttonText = Find.Child(transform, "ButtonText").gameObject.GetComponent<TMP_Text>();
        buttonText.text = "English";
    }

    public void Save()
    {
        var isKorean = GameService.progress.isKorean;
        GameService.dataManager.AddData(new Data(this.name, 0, isKorean));
    }

    public void Load()
    {
        var isKorean = GameService.dataManager.FindData(this.name).boolean;

        buttonText.text = isKorean ? "한국어" : "English";
        GameService.progress.ChangeLenguage(isKorean);
    }

    protected override void Click()
    {
        var isKorean = GameService.progress.isKorean;

        buttonText.text = !isKorean ? "한국어" : "English";
        GameService.progress.ChangeLenguage(!isKorean);
    } 
}
