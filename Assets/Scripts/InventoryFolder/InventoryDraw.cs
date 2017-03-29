using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.InventoryFolder
{
    public abstract class InventoryDraw
    {
        private readonly List<SlotItem> _itemList; 
        protected InventoryDraw(List<SlotItem> itemList)
        {
            _itemList = itemList;
        }

        public virtual void DrawEmptyItems()
        {
            if (_itemList == null) return;
            foreach (SlotItem inventoryItem in _itemList.Where(s => !s.Occupied))
            {
                inventoryItem.DrawEmpty();
            }
        }

        public virtual void DrawFullItems()
        {
            if (_itemList == null) return;
            foreach (SlotItem inventoryItem in _itemList.Where(s => s.Occupied))
            {
                inventoryItem.DrawFull();
            }
        }

        public virtual void DrawItemInfo()
        {
            if (_itemList == null) return;
            foreach (SlotItem inventoryItem in _itemList.Where(s => s.Item != null))
            {
                inventoryItem.Item.DrawStatus();
            }
        }

        public virtual void DrawOnHover()
        {
            if (_itemList == null) return;
            foreach (SlotItem inventoryItem in _itemList.Where(inventoryItem => inventoryItem.Rect.Contains(MyInput.CurrentMousePosition())))
            {
                inventoryItem.DrawOnHower();
            }
        }
    }
}
