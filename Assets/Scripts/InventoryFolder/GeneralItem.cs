using UnityEngine;

namespace Assets.Scripts.InventoryFolder
{
    public abstract class GeneralItem
    {
        public Vector2 Position { get; set; }
        public Texture Texture { get; set; }
        public Rect Rect { get; set; }
        protected const int ITEMSIZE = 48;
    }
}
