using UnityEngine;

namespace Assets.Scripts.InventoryFolder
{

    public class SlotItem : GeneralItem
    {
        public bool Occupied { get; set; }
        public int Row { get; private set; }
        public int Column { get; private set; }
        public bool OnHover { get; set; }
        public Item Item { get; set; }
        public ESubtype Subtype { get; private set; }
        public EType Type { get; private set; }
        private const int SPACE = 0;
        private Vector2 _defaultPostition;

        public SlotItem(int column, int row, Vector2 defaultPosition, Texture texture)
        {
            Row = row;
            Column = column;
            Texture = texture;
            _defaultPostition = defaultPosition;
            CalculatePosition();
            Subtype = ESubtype.None;
            Type = EType.None;
        }

        public SlotItem(Vector2 position, Texture texture, EType eType, ESubtype eSubtype)
        {
            Position = position;
            Texture = texture;
            Subtype = eSubtype;
            Type = eType;
            Occupied = false;
            UpdateRectangePosition();
        }

        private void CalculatePosition()
        {
            _defaultPostition = new Vector2(_defaultPostition.x - ITEMSIZE, _defaultPostition.y - ITEMSIZE);
            Position = new Vector2(Row * (ITEMSIZE + SPACE), Column * (ITEMSIZE + SPACE));
            Position = _defaultPostition - Position;
            UpdateRectangePosition();
        }

        private void UpdateRectangePosition()
        {
            Rect = new Rect(Position.x, Position.y, ITEMSIZE, ITEMSIZE);
        }

        public void DrawEmpty()
        {
            GUI.DrawTexture(new Rect(Rect.x, Rect.y, ITEMSIZE, ITEMSIZE), Texture);
        }
        public void DrawFull()
        {
            if(Item != null)
            {
                if(Item.Icon != null)
                    GUI.DrawTexture(new Rect(Position.x, Position.y, ITEMSIZE, ITEMSIZE), Item.Icon.texture);
                if(Item.Stack > 1 && Item.ActualStack > 1)
                {
                    GUI.skin.label.fontSize = 16;
                    GUI.Label(new Rect(Rect.center.x, Rect.center.y, 24, 24), Item.ActualStack.ToString());
                }
            }
        }
        public void DrawOnHower()
        {
            if(!Occupied)
                GUI.DrawTexture(Rect, Resources.Load<Texture>(Item.PATH+"/OnHover"));
        }

        public void PickUpItem()
        {
            Occupied = false;
        }
    }
}
