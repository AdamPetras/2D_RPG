using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.Scripts.InventoryFolder.CraftFolder
{
    public struct CraftItem
    {
        public int ID;
        public int NumberOfItems;

        public CraftItem(int id, int numberOfItems)
        {
            ID = id;
            NumberOfItems = numberOfItems;
        }
    }

    public class CraftDataOperating
    {
        public Item SelectedItem { get; set; }
        private GetCraftComponent _getCraftComponent;
        private GameObject _craftItemPrefab;
        private List<GameObject> _objectList;
        public CraftDataOperating(GetCraftComponent getCraftComponent)
        {
            _getCraftComponent = getCraftComponent;
            _getCraftComponent.GetCraftButton().onClick.AddListener(AddToInventory);
            _objectList = new List<GameObject>();
            _craftItemPrefab = Resources.Load("Prefab/CraftItem") as GameObject;
        }

        public void ItemClicked()
        {
            //Debug.Log(SelectedItem.CraftItems.Count);
            int index = 0;
            foreach(GameObject obj in _objectList.ToArray())
            {
                RemoveFromList(_objectList, obj);
            }
            foreach(CraftItem craftItem in SelectedItem.CraftItems)
            {
                InstatiateItem(Item.IdToItem(craftItem.ID).Icon, SelectedItem.CraftItems[index].NumberOfItems.ToString(), _getCraftComponent.GetCraftNeedPanel());                
                index++;
            }
            //Debug.Log(SelectedItem.ID);
            InstatiateItem(Item.IdToItem(SelectedItem.ID).Icon, SelectedItem.Quantity.ToString(), _getCraftComponent.GetProductPanel());
        }

        private void InstatiateItem(Sprite icon, string text, Transform parent) 
        {
            GameObject itemObject = Object.Instantiate(_craftItemPrefab);
            _objectList.Add(itemObject);
            if(icon != null)
                itemObject.GetComponent<Image>().sprite = icon;
            itemObject.transform.Find("Text").GetComponent<Text>().text = text;
            itemObject.transform.SetParent(parent, true);
            itemObject.transform.localScale = Vector3.one;
        }

        private void RemoveFromList(List<GameObject> viewCardList, GameObject gameObject)
        {
            if(viewCardList.Contains(gameObject))
            {
                viewCardList.Remove(gameObject);  //odstranění vybraného objektu z listu
                Object.Destroy(gameObject);
            }
        }

        private bool IsInventoryContains()
        {
            int index = 0;
            foreach(CraftItem craftItem in SelectedItem.CraftItems)
            {
                if(!SlotManagement.IfAnySlotListContains(InventorySettings.InventoryItemList, craftItem.ID, craftItem.NumberOfItems))
                {
                    return false;
                }
                index++;
            }
            return true;
        }

        public void AddToInventory()
        {
            if(SelectedItem != null)
                if(IsInventoryContains())
                {
                    SelectedItem.ActualStack = SelectedItem.Quantity;
                    if(SlotManagement.AddToSlot(InventorySettings.InventoryItemList, SelectedItem))
                    {
                        RemoveItemsFromInv();                     
                    }
                }
        }

        private void RemoveItemsFromInv()
        {
            foreach(CraftItem craftItem in SelectedItem.CraftItems)
            {
                SlotManagement.DeleteItemByStacks(InventorySettings.InventoryItemList, craftItem.ID, craftItem.NumberOfItems);
            }
        }
    }
}
