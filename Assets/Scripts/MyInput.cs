using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public static class MyInput
    {
        public static Vector2 CurrentMousePosition()
        {
            return new Vector2(Input.mousePosition.x, Math.Abs(Input.mousePosition.y - Screen.height));
        }
    }
}
