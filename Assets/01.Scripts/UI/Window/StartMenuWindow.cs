using GameMathods;
using UnityEngine;

public class StartMenuWindow : MonoBehaviour,
IStarting
{
    public void Starting()
    {
        GameService.eventManager.AddUi(UiCode.OnStartWindow, On);
        GameService.eventManager.AddUi(UiCode.OffStartWindow, Off);
    }

    public void On() => this.gameObject.SetActive(true);
    public void Off() => this.gameObject.SetActive(false);
}
