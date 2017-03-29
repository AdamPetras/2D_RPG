using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.InventoryFolder
{
    public class InventorySettings : InventoryDraw
    {
        public static List<SlotItem> InventoryItemList;
        private const int row = 6;
        private const int column = 5;
        public InventorySettings():base(InventoryItemList = new List<SlotItem>())
        {           
            for (int i = 0; i < row; i++)
                for (int j = 0; j < column; j++)
                {
                    InventoryItemList.Add(new SlotItem(i, j, new Vector2(Screen.width - 20, Screen.height - 20), Resources.Load<Texture>(Item.PATH+"/inventoryItem")));
                }
        }

        /*public void DrawBackGround()
        {
            Vector2 size = new Vector2((column*53)+35,(row*53)+35);
            GUI.DrawTexture(new Rect(Screen.width-size.x,Screen.height-size.y,size.x,size.y),Resources.Load<Texture>("Graphics/2D/InventoryBackground"));
        }*/
    }
}
