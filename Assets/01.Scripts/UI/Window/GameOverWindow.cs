using GameMathods;
using UnityEngine;

public class GameOverWindow : MonoBehaviour,
IStarting
{
    public void Starting()
    {
        GameService.eventManager.AddEvent(EventCode.OnGameOver, EventFunction);
        this.gameObject.SetActive(false);
    }

    public void EventFunction() => this.gameObject.SetActive(true);
}
