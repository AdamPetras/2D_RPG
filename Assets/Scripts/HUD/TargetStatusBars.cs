
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.CombatSystemFolder;
using UnityEngine;

namespace Assets.Scripts.HUD
{
    public class TargetStatusBars : StatusBars
    {
        private Canvas _canvas;
        private string _name;
        public TargetStatusBars(Canvas canvas) : base(canvas)
        {
            _name = "";
            _canvas = canvas;
        }

        public override void Init(string name)
        {
            if(_name == "" && TargetEnemy.TargetName != null)
            {
                _canvas.enabled = true;
                base.Init(name);
                _name = name;
                Debug.Log("bb");
            }
            else if(_name != "" && TargetEnemy.TargetName == null)
            {
                _canvas.enabled = false;
                RemoveListeners(_name);
                _name = "";
            }
        }
    }
}
