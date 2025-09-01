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

    //게임 전용
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
            //Resources파일 안에 프리팹 로드
            var findResource = Resources.Load<GameObject>(_resourceName);

            if (findResource == null) Debug.Log("없는 리소스 호출");
            return findResource;
        }

        public static T Script<T>(Transform _parent) where T : class
        {
            T findChild = null;

            //해당 클래스 발견시 즉시 반환 (유니티 컴포넌트는 못찾음..)
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
            //부모 스크립트에 붙어있지 않다면 자식 오브젝트 모두 검사
            var outLineComponent = _component.GetComponent<OutLineRenderer>();
            outLineComponent = outLineComponent ?? Find.Script<OutLineRenderer>(_component.transform);

            //MeshRenderer가져오기
            outLineComponent.FindRenderer();
            var renderer = outLineComponent.meshRenderer;

            //아웃라인 프리팹 불러오기
            var outLineMaterial = Find.Resource("OutLineMaterial").GetComponent<ResourceMarerial>().material;
            if (outLineMaterial == null) Debug.Log("MaterialComponent가 없는 오브젝트임");

            //마지막 배열에 아웃라인 마테리얼 추가
            renderer.materials = Service.AddArray(renderer.materials, outLineMaterial);

            //복사된 마테리얼을 반환
            return renderer.materials[renderer.materials.Length - 1];
        }

        public static GameObject SpawnShowItem(MonoBehaviour _item)
        {
            //손에 스폰시킬 아이템 복사 생성
            var showItem = Object.Instantiate(_item).transform;
            showItem.name = _item.name;

            //플레이어와의 접촉 불가능 하게 / 그림자가 보이지 않기 위해 변경
            SetAllTagLayer(showItem, "Untagged", "ShowItem");

            //필요없는 스크립트가 있을 경우 삭제
            var collider = showItem.GetComponent<Collider>();
            if (collider != null) Object.DestroyImmediate(collider);

            var rigid = showItem.GetComponent<Rigidbody>();
            if (rigid != null) Object.DestroyImmediate(rigid);

            var touch = showItem.GetComponent<TouchingComponent>();
            if (touch != null) Object.DestroyImmediate(touch);

            var drop = showItem.GetComponent<DropComponent>();
            if (drop != null) Object.DestroyImmediate(drop);

            //Load 메서드이기에 GameService.playerController할당이 완료가 된상태
            var inventoryPos = Find.Child(GameService.playerController.transform, "Inventory");
            if (inventoryPos == null) Debug.Log("Inventory 오브젝트가 존재하지 않음");

            //Inventory오브젝트의 하위 오브젝트로 이동
            showItem.transform.SetParent(inventoryPos.transform);
            showItem.gameObject.SetActive(false);

            //따로 지정한 위치가 있는지 (rotation은 프리팹에서 설정)
            var itemPos = showItem.GetComponent<Item>().showItemPos;

            if (itemPos != null) showItem.transform.position = inventoryPos.position + itemPos;
            else Debug.Log("SpawnPosComponent가 추가되지 않음");

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

