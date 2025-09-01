using GameMathods;
using UnityEngine;

public class TouchingComponent : MonoBehaviour,
IStarting
{
    public Material outLine { get; private set; }

    public void Starting() => outLine = Game.SpawnOutLine(this);

    public void ShowOutLine(bool _isShow) => outLine.SetFloat("_Scale", _isShow ? 1.02f : 0f);

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            outLine.SetFloat("_Scale", 1.02f);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            outLine.SetFloat("_Scale", 0f);
        }
    }
}
