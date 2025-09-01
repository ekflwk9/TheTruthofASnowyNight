using GameMathods;
using UnityEngine;

public class StartOptionButton : Button,
IEnd
{
    protected override void Click()
    {
        this.touchImage.SetActive(false);
        GameService.eventManager.CallUi(UiCode.OnMenu);
        GameService.eventManager.CallUi(UiCode.OffStartWindow);
    }

    public void End() => GameService.progress.Remove(this);
}
