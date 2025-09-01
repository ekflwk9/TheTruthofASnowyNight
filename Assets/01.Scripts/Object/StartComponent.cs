using UnityEngine;
using GameMathods;

public class StartComponent : MonoBehaviour
{
    //�ֻ��� �θ� ������Ʈ�� �ʿ��� ���� ��ũ��Ʈ, �ش� ������Ʈ�� ��� �ڽĵ��� �˻���

    private void Awake() => CallStarting(this.transform);

    private void CallStarting(Transform _parent)
    {
        var component = _parent.GetComponent<IStarting>();
        if (component != null) component.Starting();
        FindComponent(_parent);
    }

    private void FindComponent(Transform _parent)
    {
        //�ڽ� ������Ʈ�� ���� ��� ��� �˻�
        for (int i = 0; i < _parent.childCount; i++)
        {
            var child = _parent.GetChild(i);
            CallStarting(child);
        }
    }
}
