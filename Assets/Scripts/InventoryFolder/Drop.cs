using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.InventoryFolder
{
    public class Drop : InventoryDraw
    {
        public static List<SlotItem> DropItemList;
        public Drop():base(DropItemList = new List<SlotItem>())
        {
            
        }

        public void AddItems(params Item[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (DropItemList.Any(s => s.Item.Name == items[i].Name && s.Item.ActualStack < s.Item.Stack))
                {
                    DropItemList.Find(s => s.Item.Name == items[i].Name && s.Item.ActualStack < s.Item.Stack).Item.ActualStack++;
                    //Debug.Log(DropItemList.Find(s => s.Item.Name == items[i].Name).Item.ActualStack +"  "+DropItemList.Find(s => s.Item.Name == items[i].Name).Item.Stack);
                }
                else
                {
                    DropItemList.Add(new SlotItem(DropItemList.Count, 0, new Vector2(Screen.width / 2, Screen.height / 2), Resources.Load<Texture>(Item.PATH+"/inventoryItem")));
                    DropItemList.Last().Item = items[i];
                    DropItemList.Last().Occupied = true;
                    DropItemList.Last().Item.Position = DropItemList.Last().Position;
                    DropItemList.Last().Item.UpdateRectangePosition();
                }
            }
        }

        public void Update()
        {
            DeleteList();
        }

        private void DeleteItem(SlotItem item)
        {
            DropItemList.Remove(item);
        }

        private void DeleteList()
        {
            if (DropItemList != null)
                if (DropItemList.FindAll(s => s.Item != null).Count == 0)
                {
                    DropItemList = null;
                    Debug.Log("deleted");
                }
        }
    }
}
