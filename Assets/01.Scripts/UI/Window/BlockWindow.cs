using GameMathods;
using UnityEngine;

public class BlockWindow : MonoBehaviour,
IStarting
{
    public void Starting()
    {
        GameService.eventManager.AddUi(UiCode.OnBlock, On);
        GameService.eventManager.AddUi(UiCode.OffBlock, Off); 
    }

    public void On() => this.gameObject.SetActive(true);
    public void Off() => this.gameObject.SetActive(false);
}
