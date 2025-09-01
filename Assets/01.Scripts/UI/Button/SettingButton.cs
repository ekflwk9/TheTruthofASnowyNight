using UnityEngine;
using GameMathods;

public class SettingButton : Button
{
    protected override void Click()
    {
        GameService.eventManager.CallUi(UiCode.OffOption);
        GameService.eventManager.CallUi(UiCode.OnSetting);
    }
}
