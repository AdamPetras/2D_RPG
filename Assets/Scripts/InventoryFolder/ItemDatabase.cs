using System.Collections.Generic;
using System.Xml;
using Assets.Scripts.Extension;
using UnityEngine;

namespace Assets.Scripts.InventoryFolder
{
    public class ItemDatabase : MonoBehaviour
    {
        public TextAsset ItemInventory;
        public static List<Item> Database = new List<Item>();
        private XmlDatabaseReader _xmlDatabaseReader;

        void Awake()
        {
            _xmlDatabaseReader = new XmlDatabaseReader(ItemInventory);
            _xmlDatabaseReader.ReadItemsFromDatabase("Item","ID","ItemName","Type","Subtype","Stats","StatValues","Chance",
                "CraftItem","NumberOfItems","Quantity","Profesion","Loot","Stack");           
        }
    }
}
