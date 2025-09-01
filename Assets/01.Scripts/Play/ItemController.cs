using UnityEngine;
using GameMathods;
using System.Collections.Generic;

public class ItemController : MonoBehaviour,
IStarting, ISave, IEnd
{
    private GameObject touchItem;
    private bool isClick = false;

    private string slot1;//, slot2;
    private Dictionary<string, GameObject> showItem = new Dictionary<string, GameObject>();

    public void Starting()
    {
        GameService.SetComponent(this);
        GameService.eventManager.AddUi(UiCode.DropWindow, OnDropWindow);
    }

    public void Save()
    {
        //GameService.dataManager.AddData(new Data("Inventory1", 0, false, 0, slot1));
        //GameService.dataManager.AddData(new Data("Inventory2", 0, false, 0, slot2));
    }

    public void End()
    {
        //로드 버튼을 눌렀을 경우에만 세이브 파일 할당
        if (GameService.progress.isLoad)
        {
            var slot1Data = GameService.dataManager.FindData("Inventory1").str;
            //var slot2Data = GameService.dataManager.FindData("Inventory2").str;

            slot1 = slot1Data == "" ? null : slot1Data;
            //slot2 = slot2Data == "" ? null : slot2Data;
        }
    }

    private void OnDropWindow()
    {
        if(CheckPickUp(slot1)) GameService.eventManager.CallUi(UiCode.OnDrop);
        //else if(CheckPickUp(slot2)) GameService.eventManager.CallUi(UiCode.OnDrop);
    }

    public bool CheckPickUp(string _itemName)
    {
        //해당 아이템을 들고있는가?
        if (_itemName == null)
        {
            return false;
        }

        else if (showItem[_itemName].activeSelf)
        {
            return true;
        }

        else return false;
    }

    public bool CheckInventory(string _itemName)
    {
        if (slot1 == _itemName) return true;
        //else if (slot2 == _itemName) return true;
        else return false;
    }

    public void AddShowItem(string _itemName, GameObject _showItem)
    {
        if (!showItem.ContainsKey(_itemName)) showItem.Add(_itemName, _showItem);
    }

    public void RemoveItem(string _itemName)
    {
        if (CheckPickUp(_itemName))
        {
            if (_itemName == slot1) slot1 = null;
            //else if(_itemName == slot2) slot2 = null;

            showItem[_itemName].SetActive(false);
        }
    }

    public void ResetItem()
    {
        foreach (var key in showItem.Keys)
        {
            showItem[key].SetActive(false);
        }

        slot1 = null;
        //slot2 = null;
        GameService.eventManager.CallUi(UiCode.OffDrop);
    }

    public bool GetItem(string _itemName)
    {
        if (!showItem.ContainsKey(_itemName))
        {
            Debug.Log(_itemName + "은 플레이어 손에 스폰되지 않은 아이템임");
            return false;
        }

        else if (slot1 == null)
        {
            slot1 = _itemName;
            SwitchSlotItem(slot1, null);
        }

        //else if (slot2 == null)
        //{
        //    slot2 = _itemName;
        //    SwitchSlotItem(slot2, slot1);
        //}

        else return false;
        return true;
    }

    private void SwitchSlotItem(string _showItem, string _hideItem)
    {
        //아이템을 소지 중일 경우에만
        if (_showItem != null)
        {
            //아이템을 현재 장착 중일 경우에만
            showItem[_showItem].SetActive(true);
            GameService.eventManager.CallUi(UiCode.OnDrop);
        }

        //다음 슬롯이 아이템이 없을 경우 UI 비활성화
        else
        {
            GameService.eventManager.CallUi(UiCode.OffDrop);
        }

        if (_hideItem != null)
        {
            //이미 장착 중인 아이템이 있을 경우 비활성화
            if (showItem[_hideItem].activeSelf) showItem[_hideItem].SetActive(false);
        }
    }

    private string DropItem(string _itemName)
    {
        if (CheckPickUp(_itemName))
        {
            showItem[_itemName].SetActive(false);
            GameService.eventManager.CallUi(UiCode.OffDrop);
            GameService.gameManager.Drop(_itemName, 2.5f);

            return null;
        }

        else return _itemName;
    }

    private string UseItem(string _itemName)
    {
        if (CheckPickUp(_itemName))
        {
            if (GameService.gameManager.Use(_itemName))
            {
                GameService.eventManager.CallUi(UiCode.OffDrop);
                showItem[_itemName].SetActive(false);
                return null;
            }

            else return _itemName;
        }

        else return _itemName;
    }

    public void Update()
    {
        if (!GameService.progress.isStop)
        {
            ClickDown();
            ClickUp();
            //Slot1();
            //Slot2();
            Drop();
        }
    }

    private void Drop()
    {
        if (Input.GetKeyDown(GameKey.key[ConstKey.Drop]))
        {
            slot1 = DropItem(slot1);
            //slot2 = DropItem(slot2);
        }
    }

    //private void Slot1()
    //{
    //    if (Input.GetKeyDown(GameKey.key[ConstKey.Slot1]))
    //    {
    //        SwitchSlotItem(slot1, slot2);
    //    }
    //}
    //
    //private void Slot2()
    //{
    //    if (Input.GetKeyDown(GameKey.key[ConstKey.Slot2]))
    //    {
    //        SwitchSlotItem(slot2, slot1);
    //    }
    //}

    private void ClickDown()
    {
        if (Input.GetKeyDown(GameKey.key[ConstKey.Mouse0]))
        {
            if (touchItem != null)
            {
                if (!touchItem.activeSelf) touchItem = null;
                else GameService.eventManager.CallUi(UiCode.PointClick);
            }

            isClick = true;
        }
    }

    private void ClickUp()
    {
        if (Input.GetKeyUp(GameKey.key[ConstKey.Mouse0]))
        {
            if (isClick)
            {
                //현재 아이템을 먹을 준비 중인가?
                if (touchItem != null)
                {
                    GameService.eventManager.CallUi(UiCode.PointExit);
                    GameService.gameManager.Interaction(touchItem);
                }

                //그게 아니라면 아이템을 들고 있는가?
                else
                {
                    slot1 = UseItem(slot1);
                    //slot2 = UseItem(slot2);
                }

                isClick = false; //대화가 다시 진행 되는 것을 방지
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item") || other.gameObject.CompareTag("NPC"))
        {
            RaycastHit rayHit;
            var origin = this.transform.position;
            var direction = other.transform.position - origin;
            //Debug.DrawRay(origin, direction * 4f, Color.red, 1f);

            //앞에 장애물이 있는지 검사
            if (Physics.Raycast(origin, direction, out rayHit, 4f))
            {
                if (rayHit.collider.CompareTag("Item") || other.gameObject.CompareTag("NPC"))
                {
                    touchItem = other.gameObject;
                    GameService.eventManager.CallUi(UiCode.PointEnter);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //아이템 획득으로 비활성화시 해당 메서드 실행 안됨
        if (other.gameObject.CompareTag("Item") || other.gameObject.CompareTag("NPC"))
        {
            isClick = false;
            touchItem = null;
            GameService.eventManager.CallUi(UiCode.PointExit);
        }
    }
}
