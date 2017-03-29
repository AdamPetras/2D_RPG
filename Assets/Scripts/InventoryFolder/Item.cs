using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Extension;
using Assets.Scripts.InventoryFolder.CraftFolder;
using UnityEngine;

namespace Assets.Scripts.InventoryFolder
{

    public enum EType
    {
        None,
        Usable,
        Consumable,
        Craftable,
        Armour,
        Weapon,
        Ammo
    }

    public enum ESubtype
    {
        None = 0,
        Head,
        Chest,
        Hands,
        Legs,
        Boots,
        Neck,
        Ring,
        Bracers,
        Waist,
        Weapon,
        Axe,
        Bow,
        Sword,
        Spear,
        Knife
    }

    public enum EProfesion
    {
        None,
        Cooking,
        Fishing,
        Tailoring,
        Smithing,
        Crafting
    }

    public enum EStats
    {
        None,
        Health,
        Damage,
        Consumable
    }

    public struct ItemStats
    {
        public EStats EStats { get; private set; }
        public int Value { get; private set; }
        public bool Wearing { get; set; }
        public ItemStats(EStats eStats, int value)
        {
            EStats = eStats;
            Value = value;
            Wearing = false;
        }
    }

    public class Item : GeneralItem
    {
        public int ID { get; private set; }
        public Sprite Icon { get; private set; }
        public string Name { get; private set; }
        public EType Type { get; private set; }
        public ESubtype Subtype { get; private set; }
        public EProfesion EProfesion { get; private set; }
        public List<ItemStats> ItemStats { get; private set; }
        public int Chance { get; private set; }
        public List<CraftItem> CraftItems { get; set; }
        public int Loot { get; private set; }
        public int Quantity { get; private set; }
        public int Stack { get; private set; }
        public int ActualStack { get; set; }
        public bool Grabed { get; set; }

        public GameObject Obj;
        public bool Instantiate;
        public static readonly string PATH = "Inventory";
        public Item(Dictionary<string, string> itemDictionary)
        {
            ID = int.Parse(itemDictionary["ID"]);
            Name = itemDictionary["ItemName"];
            Icon = Resources.Load<Sprite>(PATH+"/" + Name);
            CraftItems = new List<CraftItem>();
            ItemStats = new List<ItemStats>();
            if(itemDictionary["Type"] != "")
                Type = (EType)Enum.Parse(typeof(EType), itemDictionary["Type"], true);
            else
                Type = EType.None;
            if(itemDictionary["Subtype"] != "")
                Subtype = (ESubtype)Enum.Parse(typeof(ESubtype), itemDictionary["Subtype"], true);
            else
                Subtype = ESubtype.None;
            if(itemDictionary["Profesion"] != "")
                EProfesion = (EProfesion)Enum.Parse(typeof(EProfesion), itemDictionary["Profesion"], true);
            else
                EProfesion = EProfesion.None;
            for(int i = 0; i < itemDictionary["Stats"].Split(',').Count(); i++)
            {

                if(itemDictionary["Stats"].Split(',')[i] != "" && itemDictionary["StatValues"].Split(',')[i] != "")
                    ItemStats.Add(new ItemStats((EStats)Enum.Parse(typeof(EStats), itemDictionary["Stats"].Split(',')[i], true), int.Parse(itemDictionary["StatValues"].Split(',')[i])));
            }
            if(itemDictionary["Chance"] != "")
                Chance = int.Parse(itemDictionary["Chance"]);
            /*if(itemDictionary["CraftItem"] != "" && itemDictionary["NumberOfItems"] != ""
                && itemDictionary["CraftItem"].Count() == itemDictionary["NumberOfItems"].Count())*/
            for(int i = 0; i < itemDictionary["CraftItem"].Split(',').Count(); i++)
            {
                if(itemDictionary["CraftItem"].Split(',')[i] != "" && itemDictionary["NumberOfItems"].Split(',')[i] != "")
                    CraftItems.Add(new CraftItem(int.Parse(itemDictionary["CraftItem"].Split(',')[i]), int.Parse(itemDictionary["NumberOfItems"].Split(',')[i])));
            }
            if(itemDictionary["Quantity"] != "")
                Quantity = int.Parse(itemDictionary["Quantity"]);
            if(itemDictionary["Loot"] != "")
                Loot = int.Parse(itemDictionary["Loot"]);
            if(itemDictionary["Stack"] != "")
                Stack = int.Parse(itemDictionary["Stack"]);
            ActualStack = 1;
            Grabed = false;
            Instantiate = false;
        }

        public Item(Item item)
        {
            ID = item.ID;
            Icon = item.Icon;
            Name = item.Name;
            Type = item.Type;
            Subtype = item.Subtype;
            EProfesion = item.EProfesion;
            ItemStats = item.ItemStats;
            Chance = item.Chance;
            CraftItems = item.CraftItems;
            Quantity = item.Quantity;
            Stack = item.Stack;
            ActualStack = item.ActualStack;
            Grabed = false;
            Instantiate = false;
            Loot = item.Loot;
            Rect = item.Rect;
            Texture = item.Texture;
            Position = item.Position;

        }

        public void UpdateRectangePosition()
        {
            Rect = new Rect(Position.x, Position.y, ITEMSIZE, ITEMSIZE);
        }

        public void DrawItem()
        {
            if(Icon != null)
                GUI.DrawTexture(Rect, Icon.texture);
        }

        public void DrawStatus()
        {
            if(!Grabed)
                if(Rect.Contains(MyInput.CurrentMousePosition()))
                    GUI.Box(DrawTreatment.ScreenRectThreatment(new Rect(Rect.xMax, Rect.yMax, 100, 100)), Name + "\n" + Type + "\n" + Subtype);
        }

        public void DeductStack(int stack)
        {
            if(ActualStack >= stack)
            {
                ActualStack -= stack;
            }
        }

        public static Item IdToItem(int id)
        {
            return ItemDatabase.Database.Find(s => s.ID == id);
        }
    }
}
