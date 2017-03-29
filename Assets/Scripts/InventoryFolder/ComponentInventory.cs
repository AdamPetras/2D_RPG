using System.Linq;
using Assets.Scripts.InventoryFolder.CraftFolder;
using UnityEngine;

namespace Assets.Scripts.InventoryFolder
{
    public class ComponentInventory : MonoBehaviour
    {
        private InventorySettings _inventorySettings;
        private SlotManagement _slotManagement;
        private ArmorSettings _armorSettings;
        private Drop _drop;
        public static bool ShowInv;
        private bool ShowDrop;

        // Use this for initialization
        void Start()
        {
            _slotManagement = new SlotManagement();
            _inventorySettings = new InventorySettings();
            _armorSettings = new ArmorSettings();
            _drop = new Drop();
            _drop.AddItems(new Item(ItemDatabase.Database[1]), new Item(ItemDatabase.Database[4]), new Item(ItemDatabase.Database[7]),  new Item(ItemDatabase.Database[4]), new Item(ItemDatabase.Database[4]),
                new Item(ItemDatabase.Database[3]), new Item(ItemDatabase.Database[1]), new Item(ItemDatabase.Database[7])
                , new Item(ItemDatabase.Database[3]), new Item(ItemDatabase.Database.Find(s=>s.ID == 100)), new Item(ItemDatabase.Database[7])
                , new Item(ItemDatabase.Database[3]), new Item(ItemDatabase.Database[2]), new Item(ItemDatabase.Database[7])
                , new Item(ItemDatabase.Database[3]), new Item(ItemDatabase.Database[2]), new Item(ItemDatabase.Database[6])
                , new Item(ItemDatabase.Database[3]), new Item(ItemDatabase.Database[2]), new Item(ItemDatabase.Database[6])
                , new Item(ItemDatabase.Database[1]), new Item(ItemDatabase.Database[2]), new Item(ItemDatabase.Database[6])
                , new Item(ItemDatabase.Database[1]), new Item(ItemDatabase.Database[2]), new Item(ItemDatabase.Database[6]));
        }

        // Update is called once per frame
        void Update()
        {
            if (ShowInv && !ComponentCraftMenu.Draw)   //ošetření pohybu pokud neni zobrazen inventář
            {
                _slotManagement.MoveBetweenTwoLists(InventorySettings.InventoryItemList, Drop.DropItemList, ArmorSettings.ArmorItemList);
                _slotManagement.InventoryInteract();
            }
            if (ShowDrop && !ComponentCraftMenu.Draw) //ošetření pohybu pokud neni zobrazen drop
            {
               // _drop.PickUp(_slotManagement.AddToList, InventorySettings.InventoryItemList);
                _drop.Update();
            }
            if (Input.GetKeyDown(KeyCode.I) && !ShowInv && !ComponentCraftMenu.Draw)
                ShowInv = true;
            else if (Input.GetKeyDown(KeyCode.I) && ShowInv && !ComponentCraftMenu.Draw)
                ShowInv = false;
            if (Input.GetKeyDown(KeyCode.B) && !ShowDrop && !ComponentCraftMenu.Draw)
                ShowDrop = true;
            else if (Input.GetKeyDown(KeyCode.B) && ShowDrop || ComponentCraftMenu.Draw)
                ShowDrop = false;
        }

        void OnGUI()
        {
            if (ShowInv)   //vykreslení inventáře
            {                              
                //_inventorySettings.DrawBackGround();
                _inventorySettings.DrawEmptyItems();
                _armorSettings.DrawEmptyItems();
                _inventorySettings.DrawFullItems();                   
                _armorSettings.DrawFullItems();
                if (InventorySettings.InventoryItemList.Any(s => s.Rect.Contains(MyInput.CurrentMousePosition())))
                    _inventorySettings.DrawOnHover();
                if (ArmorSettings.ArmorItemList.Any(s => s.Rect.Contains(MyInput.CurrentMousePosition())))
                    _armorSettings.DrawOnHover();               
            }          
            if (ShowDrop)  //vykreslení dropu
            {
                //_drop.DrawEmptyItems();  
                _drop.DrawFullItems();
            }
            if (ShowInv)   //kvůli vykreslovacím vrstvám
            {
                _slotManagement.DrawMovingItem();
                _armorSettings.DrawItemInfo();
                _inventorySettings.DrawItemInfo();
                _slotManagement.DrawStacking();
            }
            if (ShowDrop)  //kvůli vykreslovacím vrstvám                
                _drop.DrawItemInfo();
            

        }
    }
}
