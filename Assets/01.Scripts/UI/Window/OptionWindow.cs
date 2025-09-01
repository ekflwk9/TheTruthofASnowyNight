using GameMathods;
using UnityEngine;

public class OptionWindow : MonoBehaviour,
IStarting
{
    public void Starting()
    {
        GameService.eventManager.AddUi(UiCode.OnOption, On);
        GameService.eventManager.AddUi(UiCode.OffOption, Off);
    }

    private void On() => this.gameObject.SetActive(true);
    private void Off() => this.gameObject.SetActive(false);
}
