using GameMathods;
using UnityEngine;
using Unity.Mathematics;

public abstract class Talk : MonoBehaviour,
IStarting, IInteraction, IEnd
{
    protected TouchingComponent isTouch;

    public virtual void Starting()
    {
        GameService.SetComponent(this);

        isTouch = this.gameObject.AddComponent<TouchingComponent>();
        isTouch.Starting();
    }

    protected void LookPlayer()
    {
        //2D에서 Y축이 3D에선 X축으로 계산 = Y축 변환 (2D 에서는 Z축)
        var lookRote = GameService.playerController.transform.position - transform.position;
        var tan = math.atan2(lookRote.x, lookRote.z);
        var deg = math.degrees(tan);
        var newRote = Quaternion.Euler(0, deg, 0);

        this.transform.rotation = newRote;
    }

    public abstract void Interaction();

    public virtual void End() => GameService.gameManager.Remove(this);
}
