using UnityEngine;
using GameMathods;

public class StartComponent : MonoBehaviour
{
    //최상위 부모 오브젝트에 필요한 시작 스크립트, 해당 오브젝트의 모든 자식들을 검사함

    private void Awake() => CallStarting(this.transform);

    private void CallStarting(Transform _parent)
    {
        var component = _parent.GetComponent<IStarting>();
        if (component != null) component.Starting();
        FindComponent(_parent);
    }

    private void FindComponent(Transform _parent)
    {
        //자식 오브젝트가 있을 경우 재귀 검사
        for (int i = 0; i < _parent.childCount; i++)
        {
            var child = _parent.GetChild(i);
            CallStarting(child);
        }
    }
}
