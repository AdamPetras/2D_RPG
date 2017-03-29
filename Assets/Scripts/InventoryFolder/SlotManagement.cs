using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.PlayerFolder;
using UnityEngine;

namespace Assets.Scripts.InventoryFolder
{
    public class SlotManagement
    {
        private enum EDragAndDrop
        {
            Pick,
            Stack,
            Drag,
            Drop,
            None
        }

        private enum EWhereDrop
        {
            Inventory,
            Stack,
        }

        private EDragAndDrop _eDragAndDrop;
        private Vector2 _grabPosition;
        private Item _grabedItem;
        private SlotItem _previousSlot;
        private int _click;
        /* private bool _savePos;
         private Vector2 _posit;
         private string _result;*/

        public SlotManagement()
        {
            _click = 0;
            _grabPosition = Vector2.zero;
            _eDragAndDrop = EDragAndDrop.Pick;
        }

        public void MoveBetweenTwoLists(params List<SlotItem>[] listOfLists)
        {
            if(listOfLists.Where(s => s != null).All(s => s.Count > 0))
            {
                if(Input.GetMouseButtonDown(0) && (_eDragAndDrop == EDragAndDrop.Pick || _eDragAndDrop == EDragAndDrop.Stack)) //uchpen předmět
                {
                    foreach(List<SlotItem> slotList in listOfLists.Where(s => s != null).Where(s => s.Count > 0))
                    {

                        if(Input.GetKey(KeyCode.LeftShift) && slotList.Any(s => s.Rect.Contains(MyInput.CurrentMousePosition()) && s.Item != null && !s.Item.Grabed))
                        {
                            _eDragAndDrop = EDragAndDrop.Stack;
                            _click++;    //přičítání počtu kliků
                            SlotItem actSlotItem = slotList.Find(s => s != null && s.Rect.Contains(MyInput.CurrentMousePosition()) && s.Item != null);  //najdení slotu itemů
                            Item newItem = new Item(actSlotItem.Item);   //vytvoření nového itemu
                            _grabPosition = DeltaPosition(actSlotItem.Position, MyInput.CurrentMousePosition());
                            if(actSlotItem.Item.ActualStack > 0)
                            {
                                newItem.ActualStack = _click;        //zápis stacků do itemu
                                actSlotItem.Item.ActualStack--;     //odečtení staků                      
                            }
                            if(actSlotItem.Item.ActualStack == 0)
                                RemoveItem(actSlotItem);    //odstraň item
                            _grabedItem = newItem;  //nastav jako uchycený item
                            SetPickStats(newItem, MyInput.CurrentMousePosition() - _grabPosition);
                        }
                        //sebrání itemu na který bylo kliknuto
                        else if(slotList.Any(s => s.Rect.Contains(MyInput.CurrentMousePosition()) && s.Item != null && !s.Item.Grabed) && _grabedItem == null)
                            SetItemGrabed(slotList.Find(s => s != null && s.Rect.Contains(MyInput.CurrentMousePosition()) && s.Item != null));
                    }

                }
                else if(Input.GetKeyUp(KeyCode.LeftShift) && _eDragAndDrop == EDragAndDrop.Stack)
                {
                    //pokud pustím shift a stackuju
                    _eDragAndDrop = EDragAndDrop.Drop;
                    _click = 0;
                }

                if(_eDragAndDrop == EDragAndDrop.Drop)
                {
                    //neustálé nastavování hodnot
                    SetPickStats(_grabedItem, MyInput.CurrentMousePosition() - _grabPosition);
                }
                if(Input.GetMouseButtonUp(0) && _eDragAndDrop == EDragAndDrop.Drag)
                {
                    //pokud neni zmáčknuto tlačítko myši a je drag
                    _eDragAndDrop = EDragAndDrop.Drop;
                    AddRemoveStats(_previousSlot, false);
                }
                else if(Input.GetMouseButtonDown(0) && _eDragAndDrop == EDragAndDrop.Drop)
                {
                    bool saved = false;
                    foreach(List<SlotItem> slotList in listOfLists.Where(s => s != null))
                    {
                        //výběr ze všech listů které nejsou null a obsahují kurzor(kolidují s rectem)
                        if(_grabedItem != null)
                        {
                            if(
                                slotList.Any(s => s.Rect.Contains(MyInput.CurrentMousePosition()) || s.Rect.Overlaps(_grabedItem.Rect)))
                            {
                                saved = true;
                                DropService(slotList);
                            }
                        }
                    }
                    if(!saved && _eDragAndDrop != EDragAndDrop.Drag)
                    {   //ošetření aby nebylo drag(tzn. pokud budu dávat na stack a ten stack bude neprázdný a zbydou mi itemy tak je to nezahodí)
                        _eDragAndDrop = EDragAndDrop.None;
                        RemoveItem(_previousSlot);
                    }

                }
                else if(_eDragAndDrop == EDragAndDrop.None)
                {
                    //pokud je state none automaticky nastaví na pick...
                    _eDragAndDrop = EDragAndDrop.Pick;
                }
            }
        }

        public void DrawStacking()
        {
            //vykreslení stacků
            if(_eDragAndDrop == EDragAndDrop.Stack)
            {
                GUI.Box(new Rect(MyInput.CurrentMousePosition().x - 20, MyInput.CurrentMousePosition().y - 20, 20, 20), _click.ToString());
            }
        }

        private void DropService(List<SlotItem> slotList)
        {
            if(slotList.Any(s => s.Rect.Overlaps(_grabedItem.Rect)))
            {
                if(slotList.Any(s => s.Rect.Contains(MyInput.CurrentMousePosition()) && !s.Occupied && CanIPutToSlot(s, _grabedItem)))
                {   //přidání do inventáře
                    DropItem(slotList.Find(s => s.Rect.Contains(MyInput.CurrentMousePosition()) && !s.Occupied && CanIPutToSlot(s, _grabedItem)), EWhereDrop.Inventory);
                }
                else if(slotList.Any(s => s.Rect.Contains(MyInput.CurrentMousePosition()) && s.Occupied && CanWeStack(s.Item, _grabedItem)))
                {   //stackování
                    DropItem(slotList.Find(s => s.Rect.Contains(MyInput.CurrentMousePosition()) && s.Occupied && CanWeStack(s.Item, _grabedItem)), EWhereDrop.Stack);
                }
                else
                {   //jinak hození na předchozí pozici
                    _previousSlot.Item = _grabedItem;   //pozice je předchozí
                    _eDragAndDrop = EDragAndDrop.None;  //nastavení na none
                    SetDropStats(_previousSlot, _grabedItem);   //nastavení příslušných atributů
                    _grabedItem = null;
                    AddRemoveStats(_previousSlot);
                }
            }
        }

        private void DropItem(SlotItem slotItem, EWhereDrop eWhereDrop)
        {
            if(eWhereDrop == EWhereDrop.Inventory)  //pokud vloží někam do inventáře(dropu) někde na volné místo
            {
                SetDropStats(slotItem, _grabedItem);    //nastavení všech potřebných atributů
                if(_previousSlot != null && !_previousSlot.Occupied && slotItem != _previousSlot)
                {
                    //zde byl bug kvuli tomu že se nenulovat item na předchozím místě item se tvářil že tam není (graficky) logicky tam byl
                    _previousSlot.Item = null;
                }
                _grabedItem = null; //neni nic uchpeno
                _eDragAndDrop = EDragAndDrop.None;  //state je none
            }
            else if(eWhereDrop == EWhereDrop.Stack) //pokud vloží někam kde se bude stackovat
            {
                if(!StackOverflow(slotItem.Item, _grabedItem))
                {
                    slotItem.Item.ActualStack += _grabedItem.ActualStack;   //přičtení k aktuálnímu stacku
                    _eDragAndDrop = EDragAndDrop.None;  //state nastaven na none
                    _grabedItem = null;     //není uchopen
                }
                else
                {   //pokud přeteče stackování
                    int differ = slotItem.Item.Stack - slotItem.Item.ActualStack;   //rozdíl staků
                    slotItem.Item.ActualStack = slotItem.Item.Stack;    //nastavení staků na max
                    _grabedItem.ActualStack -= differ;  //odečtení staků z drženého itemu
                    _eDragAndDrop = EDragAndDrop.Drag;      //state je pořád drag
                }
            }
            AddRemoveStats(slotItem);
        }



        private bool CanIPutToSlot(SlotItem slot, Item item)
        {
            if(slot.Subtype == ESubtype.None || slot.Type == EType.None)
                return true;
            if(slot.Type == EType.Armour && item.Subtype == slot.Subtype)
                return true;
            if(slot.Type == EType.Weapon && item.Type == EType.Weapon)
                return true;
            return false;
        }

        private void SetItemGrabed(SlotItem slotItem)
        {
            _grabedItem = slotItem.Item;    //nastavení grab itemu na slotitem
            _previousSlot = slotItem;       //nastavení předchozího slotu
            SetDropStats(slotItem, _grabedItem, false, true);
            slotItem.Occupied = false;     //inventarový item již neni obsazen
            _grabPosition = DeltaPosition(slotItem.Item.Position, MyInput.CurrentMousePosition());      //nastavení defaultní pozice kvůli posunování textury na pozici kurzoru            
            _eDragAndDrop = EDragAndDrop.Drag;  //nastavení na statu na drag
            SetPickStats(slotItem.Item, MyInput.CurrentMousePosition() - _grabPosition);
        }

        private void SetPickStats(Item item, Vector2 position)
        {
            if(item != null)
            {
                item.Position = position;   //pozicování
                _grabedItem.Grabed = true;
                item.UpdateRectangePosition(); //updatuje rectangle
            }
        }

        private Vector2 DeltaPosition(Vector2 itemPosition, Vector2 mousePosition)
        {
            return new Vector2(mousePosition.x - itemPosition.x, mousePosition.y - itemPosition.y);   //uloží pozici při úchytu
        }

        public void DrawMovingItem()
        {
            if(_grabedItem == null)
                return;
            _grabedItem.DrawItem();
        }

        private static bool CanWeStack(Item firstItem, Item secondItem)
        {
            return (firstItem.Name == secondItem.Name && firstItem.Stack > 1 && (firstItem.ActualStack < firstItem.Stack || secondItem.ActualStack < secondItem.Stack) && firstItem.ActualStack < firstItem.Stack);

        }



        private void RemoveItem(SlotItem slotItem)
        {
            _grabedItem = null;
            SetDropStats(slotItem, null, false);
        }

        public static bool AddToSlot(List<SlotItem> slotList, Item item)
        {
            if(slotList.Where(s => s.Item != null).Any(s => CanWeStack(s.Item, item)))
            {
                Item stackableItem = slotList.Where(s => s.Item != null).First(s => CanWeStack(s.Item, item)).Item;
                if(StackOverflow(stackableItem, item) && CanWeStack(stackableItem, stackableItem))
                {
                    int differ = stackableItem.Stack - stackableItem.ActualStack;
                    stackableItem.ActualStack = stackableItem.Stack;
                    Debug.Log(differ);
                    item.ActualStack = differ;
                    SlotItem slotItem = slotList.First(s => !s.Occupied);
                    SetDropStats(slotItem, new Item(item));
                    return true;
                }
                else
                {
                    stackableItem.ActualStack += item.ActualStack;  //přičtení stacků k itemu
                    return true;
                }

            }
            else if(slotList.Any(s => !s.Occupied))
            {
                SlotItem slotItem = slotList.First(s => !s.Occupied);
                SetDropStats(slotItem, new Item(item));
                return true;
            }
            else
                Debug.Log("ALL SLOTS ARE FULL");
            return false;
        }

        public static void SetDropStats(SlotItem slotItem, Item item, bool occupied = true, bool grabed = false)
        {
            if(slotItem != null)
            {
                slotItem.Item = item;
                slotItem.Occupied = occupied;
                if(item != null)
                {
                    item.Grabed = grabed;
                    item.Position = slotItem.Position; //pozicování
                    item.UpdateRectangePosition();
                }
            }
        }

        public static bool IfAnySlotListContains(List<SlotItem> slotList, int itemId, int numberOfStack)
        {
            /*
            foreach(SlotItem slotItem in slotList.Where(s => s.Item.ID == itemId))
            {
                allStacks += slotItem.Item.ActualStack;
            }
            */
            int allStacks = slotList.Where(s => s.Item != null).Where(s => s.Item.ID == itemId).Sum(slotItem => slotItem.Item.ActualStack);
            return numberOfStack <= allStacks;
        }

        public static void DeleteItemByStacks(List<SlotItem> slotList, int itemId, int numberOfStack)
        {
            if(IfAnySlotListContains(slotList, itemId, numberOfStack))
            {
                foreach(SlotItem slotItem in slotList.Where(s => s.Item != null).Where(s => s.Item.ID == itemId))
                {
                    if(!StackUnderflow(slotItem.Item, numberOfStack))
                        slotItem.Item.ActualStack -= numberOfStack;
                    else
                    {
                        numberOfStack -= slotItem.Item.ActualStack;
                        slotItem.Item = null;
                        slotItem.Occupied = false;
                    }

                }
            }
        }

        private static bool StackOverflow(Item firstItem, Item secondItem)
        {
            return firstItem.ActualStack + secondItem.ActualStack > firstItem.Stack;
        }

        private static bool StackUnderflow(Item firstItem, Item secondItem)
        {
            return firstItem.ActualStack - secondItem.ActualStack < 1;
        }

        private static bool StackUnderflow(Item firstItem, int secondItemStack)
        {
            return firstItem.ActualStack - secondItemStack < 1;
        }

        private static bool StackOverflow(Item firstItem, int secondItemStack)
        {
            return firstItem.ActualStack + secondItemStack > firstItem.Stack;
        }
        public void AddRemoveStats(SlotItem slotItem, bool add = true)
        {
            if(slotItem.Type == EType.Armour)
            {
                foreach(ItemStats itemStats in slotItem.Item.ItemStats.Where(s => s.EStats == EStats.Health))
                {
                    if(add)
                        Player.OnAddMaxHealth(itemStats.Value);
                    else
                        Player.OnAddMaxHealth(-itemStats.Value);
                }
            }
        }

        public void PickUp(List<SlotItem> sourceList, List<SlotItem> destinatioList)
        {
            if(sourceList != null)
                if(sourceList.FindAll(s => s.Item != null).Count > 0)
                {
                    Vector2 mousePosition = new Vector2(Input.mousePosition.x, Math.Abs(Input.mousePosition.y - Screen.height));    //vycentrování do levého dolního rohu
                    if(Input.GetButton("RMouse"))  //uchpen předmět
                    {
                        if(sourceList.Any(s => s.Rect.Contains(mousePosition) && s.Item != null))
                        {
                            SlotItem slotItem = sourceList.Find(s => s.Rect.Contains(mousePosition) && s.Item != null);
                            if(AddToSlot(destinatioList, slotItem.Item))
                            {
                                RemoveItem(slotItem);
                                slotItem.Item.UpdateRectangePosition();
                            }
                            else
                                Debug.Log("FULL inventory");
                            Debug.Log("ADD ITEM TO INVENTORY");
                        }
                    }
                }
        }

        public void InventoryInteract()
        {
            if(Input.GetMouseButtonDown(1))
            {
                SlotItem slotItem = InventorySettings.InventoryItemList.Find(s => s.Rect.Contains(MyInput.CurrentMousePosition()));
                if(slotItem != null)
                if(slotItem.Item != null)
                {
                    if(slotItem.Item.Type == EType.Consumable)
                    {
                        Player.OnAddCurrentHealth(slotItem.Item.ItemStats[0].Value);
                        if(!StackUnderflow(slotItem.Item, 1))
                        {
                            slotItem.Item.ActualStack--;
                        }
                        else
                        {
                            slotItem.Occupied = false;
                            slotItem.Item = null;
                        }

                    }
                }
            }
        }
    }
}
