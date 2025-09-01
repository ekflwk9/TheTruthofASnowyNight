using UnityEngine;
using GameMathods;
using System.Collections.Generic;
using UnityEditor;

public class GameManager
{
    private Dictionary<string, IUse> use = new Dictionary<string, IUse>();
    private Dictionary<string, IInteraction> interactions = new Dictionary<string, IInteraction>();
    private Dictionary<string, DropComponent> drop = new Dictionary<string, DropComponent>();

    public void AddScripts(MonoBehaviour _type)
    {
        var key = _type.name;

        if (key == null)
        {
            Debug.Log("MonoBehaviour�� �������� �ʴ� ������Ʈ��");
            return;
        }

        if (_type is IUse && !use.ContainsKey(key))
        {
            use.Add(key, _type as IUse);
        }

        if (_type is IInteraction && !interactions.ContainsKey(key))
        {
            interactions.Add(key, _type as IInteraction);
        }

        if (_type is DropComponent && !drop.ContainsKey(key))
        {
            drop.Add(key, _type as DropComponent);
        }
    }

    public void ResetScript()
    {
        foreach (var key in drop.Keys)
        {
            var item = drop[key].GetComponent<Item>();

            if (item != null) item.RemoveItem();
            else Debug.Log(drop[key].name + "�� Item ������Ʈ�� ����");
        }

        drop.Clear();
        use.Clear();
        interactions.Clear();
    }

    public void Remove(MonoBehaviour _type)
    {
        var key = _type.name;

        if (key == null)
        {
            Debug.Log("MonoBehaviour�� �������� �ʴ� ������Ʈ��");
            return;
        }

        if (_type is IUse && use.ContainsKey(key)) use.Remove(key);
        if (_type is IInteraction && interactions.ContainsKey(key)) interactions.Remove(key);
        if (_type is DropComponent && drop.ContainsKey(key)) drop.Remove(key);
    }

    public void SetAllOffItem()
    {
        foreach(var key in drop.Keys)
        {
            drop[key].gameObject.SetActive(false);
        }
    }

    public void Interaction(GameObject _object)
    {
        if (interactions.ContainsKey(_object.name)) interactions[_object.name].Interaction();
        else Debug.Log(_object.name + "�� ��ȣ�ۿ뿡 �߰� �Ǿ����� ����");
    }

    public bool InteractionContain(string _objectName)
    {
        if (interactions.ContainsKey(_objectName)) return true;
        else return false;
    }

    public bool Use(string _objectName)
    {
        if (use.ContainsKey(_objectName)) return use[_objectName].Use();
        else Debug.Log(_objectName + "�� ��� ���� �����ۿ� �߰� �Ǿ����� ����");

        return false;
    }

    public bool UseContain(string _objectName)
    {
        if (use.ContainsKey(_objectName)) return true;
        else return false;
    }

    public void Drop(string _objectName, float _dropPower)
    {
        if (drop.ContainsKey(_objectName)) drop[_objectName].Drop(_dropPower);
        else Debug.Log(_objectName + "�� ��� �����ۿ� �߰� �Ǿ����� ����");
    }

    public bool DropContain(string _objectName)
    {
        if (drop.ContainsKey(_objectName)) return true;
        else return false;
    }
}
