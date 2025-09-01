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
            //첫 스폰이라면 해당 아이템 복사 생성
            var cloneItem = Instantiate(allItem[i]).GetComponent<Item>();
            cloneItem.name = allItem[i].name;

            DontDestroyOnLoad(cloneItem);
            cloneItem.gameObject.SetActive(false);

            //복사 생성된 아이템을 인벤토리 목록에 추가
            var showItem = Game.SpawnShowItem(cloneItem);
            GameService.itemController.AddShowItem(cloneItem.name, showItem);

            cloneItem.gameObject.AddComponent<TouchingComponent>().Starting();
            cloneItem.gameObject.AddComponent<DropComponent>().Starting();
            GameService.SetComponent(cloneItem);
        }
    }
}
