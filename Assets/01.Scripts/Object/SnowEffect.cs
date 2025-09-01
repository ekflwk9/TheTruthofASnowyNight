using GameMathods;
using UnityEngine;

public class SnowEffect : MonoBehaviour,
IStarting, IEnd
{
    private Vector3 resetPos;

    public void Starting()
    {
        GameService.eventManager.AddEvent(EventCode.OnSnow, OnSnow);
        GameService.eventManager.AddEvent(EventCode.OffSnow, OffSnow);

        resetPos = this.transform.position;
        this.transform.position = Vector3.down * 300;
        GameService.SetComponent(this);
    }

    private void OnSnow() => this.transform.position = resetPos;
    private void OffSnow() => this.transform.position = Vector3.down * 300;

    public void End()
    {
        GameService.eventManager.RemoveEvent(EventCode.OnSnow);
        GameService.eventManager.RemoveEvent(EventCode.OffSnow);
    }
}
