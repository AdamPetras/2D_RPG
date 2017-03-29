
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
            //enemy(vyvolá list) (zahodi list)-> (vyvolá list) character (zahodí list)
            //Target = Enemy    Target = Enemy -> Target = Character    Target = Character
            if((_name == "" && TargetCharacter.TargetName != null))
            {
                _canvas.enabled = true;
                base.Init(name);
                _name = name;
                //Debug.Log("bb");
            }
            else if((_name != TargetCharacter.TargetName && _name != "")|| (_name != "" && TargetCharacter.TargetName == null))
            {
                //Debug.Log("cc");
                _canvas.enabled = false;
                RemoveListeners(_name);
                _name = "";
            }
            //Debug.Log(_name +" Update "+ TargetCharacter.TargetName +" "+ TargetCharacter.ETarget);
        }
    }
}
