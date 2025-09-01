using GameMathods;
using UnityEngine;

public class LoadComponent : MonoBehaviour
{
    //��� �����Ͱ� �ε尡 �� �� ȣ��Ǵ� ��ũ��Ʈ

    private void Start()
    {
        GameService.dataManager.Load();
        FindComponent(this.transform);
    }

    private void FindComponent(Transform _parent)
    {
        var component = _parent.GetComponent<ILoad>();
        if (component != null) component.Load();
        FindLoad(_parent);
    }

    private void FindLoad(Transform _parent)
    {
        for (int i = 0; i < _parent.childCount; i++)
        {
            var child = _parent.GetChild(i);
            FindComponent(child);
        }
    }
}
