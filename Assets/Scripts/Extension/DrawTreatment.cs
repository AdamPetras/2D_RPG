using UnityEngine;

namespace Assets.Scripts.Extension
{
    public static class DrawTreatment
    {
        public static Rect ScreenRectThreatment(Rect rect)
        {
            if ((Screen.height < rect.yMax) && (Screen.width < rect.xMax))
                return new Rect(rect.x - (rect.xMax - Screen.width), rect.y - (rect.yMax- Screen.height), rect.width, rect.height);
            if (Screen.width < rect.xMax)
                return new Rect(rect.x-(rect.xMax-Screen.width),rect.y,rect.width,rect.height);
            if (Screen.height < rect.yMax)
                return new Rect(rect.x, rect.y - (rect.yMax - Screen.height), rect.width, rect.height);         
            return rect;
        }
    }
}
