using GameMathods;
using UnityEngine;

public class CameraWindow : MonoBehaviour,
IStarting
{
    public void Starting()
    {
        GameService.eventManager.AddUi(UiCode.OnCamera, On);
        GameService.eventManager.AddUi(UiCode.OffCamera, Off);
    }

    public void On() => this.gameObject.SetActive(true);
    public void Off() => this.gameObject.SetActive(false);
}
