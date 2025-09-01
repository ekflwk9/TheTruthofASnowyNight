using System.Collections.Generic;
using System.IO;
using GameMathods;
using UnityEngine;

#region Json����
//staic
//private
//protected �� �������� ����

//�迭�� �����̵�

//struct ����ü�� ���� ��� �ݵ�� [System.Serializable] ����ȭ�� ����� �����
//private / get private set�� ��� �ݵ�� [SerializeField]�� ����ȭ ����� �����
//public���� ����� ����, ����ü�� [SerializeField]�� �� �ʿ� ����
#endregion

[System.Serializable]
public struct Data
{
    public string dataName;
    public int integer;
    public bool boolean;
    public float floating;
    public string str;
    public Vector3 vector;
    public KeyCode keyCode;

    public Data
        (
        string _dataName,
        int _integer = 0,
        bool _boolean = false,
        float _floating = 0f,
        string _str = "",
        Vector3 _vector = new Vector3(),
        KeyCode _keyCode = KeyCode.None
        )
    {
        dataName = _dataName;
        integer = _integer;
        boolean = _boolean;
        floating = _floating;
        str = _str;
        vector = _vector;
        keyCode = _keyCode;
    }
}

public class SaveData
{
    public List<Data> data = new List<Data>();

    public SaveData(List<Data> _data) => data = _data;
}

public class DataManager
{
    private ISave[] isSave = new ISave[0];
    //private ILoad[] isLoad = new ILoad[0];

    private List<Data> data = new List<Data>();

    public void ResetScripts()
    {
        isSave = new ISave[0];
        //isLoad = new ILoad[0];
    }

    public void AddScripts(MonoBehaviour _data)
    {
        if (_data is ISave save)
        {
            if (!Service.ContainArray(isSave, save)) isSave = Service.AddArray(isSave, save);
        }

        //LoadComponent��ũ��Ʈ���� ��ͷ� ã�� ��� �����Ͽ� �ʿ� ����
        //if (_data is ILoad load)
        //{
        //    if (ContainArray(isLoad, load)) isLoad = AddArray<ILoad>(isLoad, load);
        //}
    }

    public void AddData(Data _data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].dataName == _data.dataName)
            {
                data[i] = _data;
                return;
            }
        }

        data.Add(_data);
    }

    public Data FindData(string _dataName)
    {
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].dataName == _dataName) return data[i];
        }

        return default;
    }

    public void Save()
    {
        data.Clear();

        for (int i = 0; i < isSave.Length; i++)
        {
            isSave[i].Save();
        }

        var saveClass = JsonUtility.ToJson(new SaveData(data), true);
        File.WriteAllText(Application.dataPath + "/VEsA", saveClass);
    }

    public void Load()
    {
        //ù ���۽ÿ��� ������ �ε�
        if (data.Count != 0) return;

        var loadFile = Application.dataPath + "/VEsA";

        if (File.Exists(loadFile))
        {
            loadFile = File.ReadAllText(loadFile);
            var loadClass = JsonUtility.FromJson<SaveData>(loadFile);
            data = loadClass.data;
        }
    }
}



