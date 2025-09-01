using GameMathods;
using UnityEngine;

public class SettingWindow : MonoBehaviour,
IStarting
{
    public void Starting()
    {
        GameService.eventManager.AddUi(UiCode.OnSetting, On);
        GameService.eventManager.AddUi(UiCode.OffSetting, Off);
    }

    public void On() => this.gameObject.SetActive(true);
    public void Off() => this.gameObject.SetActive(false);
}
