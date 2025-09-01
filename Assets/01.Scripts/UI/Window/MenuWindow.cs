using GameMathods;
using UnityEngine;

public class MenuWindow : MonoBehaviour,
IStarting
{
    public void Starting()
    {
        GameService.eventManager.AddUi(UiCode.OnMenu, On);
        GameService.eventManager.AddUi(UiCode.OffMenu, Off);
    }

    private void On() => this.gameObject.SetActive(true);
    private void Off() => this.gameObject.SetActive(false);
}
