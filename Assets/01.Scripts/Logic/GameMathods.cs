using UnityEngine;

namespace GameMathods
{
    public delegate void Function();
    public interface IStarting { public void Starting(); }
    public interface ILenguage { public void ChangeLenguage(); }
    public interface ISave { public void Save(); }
    public interface ILoad { public void Load(); }
    public interface IEnd { public void End(); }
    public interface IInteraction { public void Interaction(); }
    public interface IUse { public bool Use(); }

    //���� ����
    public interface IDay { public bool EventTime(); }

    public class Find
    {
        public static Transform Child(Transform _parent, string _childName)
        {
            Transform findChild = null;

            for (int i = 0; i < _parent.childCount; i++)
            {
                var child = _parent.GetChild(i);
                findChild = child.name == _childName ? child : Child(child, _childName);

                if (findChild != null) break;
            }

            return findChild;
        }

        public static AudioClip SoundSource(string _resourceName)
        {
            return LoadResource("Sound/" + _resourceName).GetComponent<AudioSource>().clip;
        }

        public static Item ItemSource(string _resourceName)
        {
            return LoadResource("Item/" + _resourceName).GetComponent<Item>();
        }

        public static GameObject Resource(string _resourceName)
        {
            return LoadResource("Object/" + _resourceName);
        }

        private static GameObject LoadResource(string _resourceName)
        {
            //Resources���� �ȿ� ������ �ε�
            var findResource = Resources.Load<GameObject>(_resourceName);

            if (findResource == null) Debug.Log("���� ���ҽ� ȣ��");
            return findResource;
        }

        public static T Script<T>(Transform _parent) where T : class
        {
            T findChild = null;

            //�ش� Ŭ���� �߽߰� ��� ��ȯ (����Ƽ ������Ʈ�� ��ã��..)
            for (int i = 0; i < _parent.childCount; i++)
            {
                var child = _parent.GetChild(i);
                findChild = child.GetComponent<T>() ?? Script<T>(child);
                if (findChild != null) break;
            }

            return findChild;
        }
    }

    public class Game
    {
        public static Material SpawnOutLine(MonoBehaviour _component)
        {
            //�θ� ��ũ��Ʈ�� �پ����� �ʴٸ� �ڽ� ������Ʈ ��� �˻�
            var outLineComponent = _component.GetComponent<OutLineRenderer>();
            outLineComponent = outLineComponent ?? Find.Script<OutLineRenderer>(_component.transform);

            //MeshRenderer��������
            outLineComponent.FindRenderer();
            var renderer = outLineComponent.meshRenderer;

            //�ƿ����� ������ �ҷ�����
            var outLineMaterial = Find.Resource("OutLineMaterial").GetComponent<ResourceMarerial>().material;
            if (outLineMaterial == null) Debug.Log("MaterialComponent�� ���� ������Ʈ��");

            //������ �迭�� �ƿ����� ���׸��� �߰�
            renderer.materials = Service.AddArray(renderer.materials, outLineMaterial);

            //����� ���׸����� ��ȯ
            return renderer.materials[renderer.materials.Length - 1];
        }

        public static GameObject SpawnShowItem(MonoBehaviour _item)
        {
            //�տ� ������ų ������ ���� ����
            var showItem = Object.Instantiate(_item).transform;
            showItem.name = _item.name;

            //�÷��̾���� ���� �Ұ��� �ϰ� / �׸��ڰ� ������ �ʱ� ���� ����
            SetAllTagLayer(showItem, "Untagged", "ShowItem");

            //�ʿ���� ��ũ��Ʈ�� ���� ��� ����
            var collider = showItem.GetComponent<Collider>();
            if (collider != null) Object.DestroyImmediate(collider);

            var rigid = showItem.GetComponent<Rigidbody>();
            if (rigid != null) Object.DestroyImmediate(rigid);

            var touch = showItem.GetComponent<TouchingComponent>();
            if (touch != null) Object.DestroyImmediate(touch);

            var drop = showItem.GetComponent<DropComponent>();
            if (drop != null) Object.DestroyImmediate(drop);

            //Load �޼����̱⿡ GameService.playerController�Ҵ��� �Ϸᰡ �Ȼ���
            var inventoryPos = Find.Child(GameService.playerController.transform, "Inventory");
            if (inventoryPos == null) Debug.Log("Inventory ������Ʈ�� �������� ����");

            //Inventory������Ʈ�� ���� ������Ʈ�� �̵�
            showItem.transform.SetParent(inventoryPos.transform);
            showItem.gameObject.SetActive(false);

            //���� ������ ��ġ�� �ִ��� (rotation�� �����տ��� ����)
            var itemPos = showItem.GetComponent<Item>().showItemPos;

            if (itemPos != null) showItem.transform.position = inventoryPos.position + itemPos;
            else Debug.Log("SpawnPosComponent�� �߰����� ����");

            return showItem.gameObject;
        }

        public static void SetAllTagLayer(Transform _parent, string _tag, string _layer)
        {
            _parent.gameObject.tag = _tag;
            _parent.gameObject.layer = LayerMask.NameToLayer(_layer);
            ChangeChildTagLayer(_parent, _tag, _layer);
        }

        public static void ChangeChildTagLayer(Transform _parent, string _tag, string _layer)
        {
            for (int i = 0; i < _parent.childCount; i++)
            {
                var child = _parent.GetChild(i);
                SetAllTagLayer(child, _tag, _layer);
            }
        }
    }

    public class Service
    {
        public static T[] AddArray<T>(T[] _array, T _data) where T : class
        {
            var temp = new T[_array.Length + 1];

            for (int i = 0; i < _array.Length; i++)
            {
                temp[i] = _array[i];
            }

            temp[_array.Length] = _data;
            return temp;
        }

        public static bool ContainArray<T>(T[] _array, T _data) where T : class
        {
            for (int i = 0; i < _array.Length; i++)
            {
                if (_array[i] == _data) return true;
            }

            return false;
        }

        public static T[] RemoveArray<T>(T[] _array, T _data) where T : class
        {
            var tempArray = new T[_array.Length == 0 ? 0 : _array.Length - 1];
            var isFind = false;

            for (int i = 0; i < _array.Length; i++)
            {
                if (_array[i] == _data) isFind = true;
                else if (isFind) tempArray[i - 1] = _array[i];
                else tempArray[i] = _array[i];
            }

            return tempArray;
        }
    }
}

