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
        //2D���� Y���� 3D���� X������ ��� = Y�� ��ȯ (2D ������ Z��)
        var lookRote = GameService.playerController.transform.position - transform.position;
        var tan = math.atan2(lookRote.x, lookRote.z);
        var deg = math.degrees(tan);
        var newRote = Quaternion.Euler(0, deg, 0);

        this.transform.rotation = newRote;
    }

    public abstract void Interaction();

    public virtual void End() => GameService.gameManager.Remove(this);
}
