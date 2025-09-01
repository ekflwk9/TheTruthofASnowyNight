using GameMathods;
using UnityEngine;

public class ItemSpawnComponent : MonoBehaviour,
ILoad
{
    public void Load()
    {
        var allItem = Resources.LoadAll<GameObject>("Item");

        for (int i = 0; i < allItem.Length; i++)
        {
            //ù �����̶�� �ش� ������ ���� ����
            var cloneItem = Instantiate(allItem[i]).GetComponent<Item>();
            cloneItem.name = allItem[i].name;

            DontDestroyOnLoad(cloneItem);
            cloneItem.gameObject.SetActive(false);

            //���� ������ �������� �κ��丮 ��Ͽ� �߰�
            var showItem = Game.SpawnShowItem(cloneItem);
            GameService.itemController.AddShowItem(cloneItem.name, showItem);

            cloneItem.gameObject.AddComponent<TouchingComponent>().Starting();
            cloneItem.gameObject.AddComponent<DropComponent>().Starting();
            GameService.SetComponent(cloneItem);
        }
    }
}
