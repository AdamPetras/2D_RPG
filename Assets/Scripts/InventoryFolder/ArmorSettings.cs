using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.InventoryFolder
{
    public class ArmorSettings : InventoryDraw
    {
        public static List<SlotItem> ArmorItemList = new List<SlotItem>();
        private Texture texture;
        private const int TEXTURESIZE = 48;
        private const int SPACE = 10;
        private const int COLUMN = 2;
        private const int ROW = 5;
        public ArmorSettings() : base(ArmorItemList)
        {
            int increm = 1;
            for(int j = 1; j <= COLUMN; j++)
                for(int i = 1; i <= ROW; i++)
                {
                    EType type;
                    texture = Resources.Load<Texture>(Item.PATH+"/Frame" + (ESubtype)(increm));
                    if(increm <= 10)
                        type = EType.Armour;
                    else
                        type = EType.Weapon;
                    ArmorItemList.Add(new SlotItem(new Vector2(Screen.width - SPACE - (j * (SPACE + TEXTURESIZE)), Screen.height - 420 - (ROW * (TEXTURESIZE + SPACE)) + (i * (TEXTURESIZE + SPACE))), texture, type, (ESubtype)(increm)));
                    increm++;
                }
        }
    }
}
