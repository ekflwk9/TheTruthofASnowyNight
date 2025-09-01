using GameMathods;
using UnityEngine;

public class DropComponent : MonoBehaviour,
IStarting
{
    public Rigidbody rigid { get; private set; }

    public void Starting()
    {
        GameService.SetComponent(this);
        rigid = GetComponent<Rigidbody>();
    }

    public void Drop(float _power)
    {
        var camPos = GameService.camController.transform;

        this.transform.position = camPos.position + camPos.forward * 0.7f;
        this.transform.rotation = Quaternion.identity;

        if (!this.gameObject.activeSelf) this.gameObject.SetActive(true);
        rigid.linearVelocity = camPos.forward * _power; //ЦђБе 2.5f
    }
}
