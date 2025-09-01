using GameMathods;
using UnityEngine;

public class OutLineRenderer : MonoBehaviour
{
    public MeshRenderer meshRenderer { get; private set; }
    public void FindRenderer() => meshRenderer = gameObject.GetComponent<MeshRenderer>();
}
