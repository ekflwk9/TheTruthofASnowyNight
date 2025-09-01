using UnityEngine;

public class OptionButton : Button
{
    protected override void Click()
    {
        GameService.eventManager.CallUi(UiCode.OffSetting);
        GameService.eventManager.CallUi(UiCode.OnOption);
    }
}
