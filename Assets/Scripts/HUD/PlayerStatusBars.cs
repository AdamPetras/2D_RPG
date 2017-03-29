using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.HUD
{
    public sealed class PlayerStatusBars:StatusBars
    {
        public PlayerStatusBars(Canvas canvas) : base(canvas)
        {
            Init("Player");
        }
    }
}
