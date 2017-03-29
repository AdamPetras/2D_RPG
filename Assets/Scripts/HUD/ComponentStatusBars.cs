using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.CombatSystemFolder;
using UnityEngine;

namespace Assets.Scripts.HUD
{
    public class ComponentStatusBars : MonoBehaviour
    {
        private TargetStatusBars _statusBars;
        private PlayerStatusBars _playerStatusBars;
        private Canvas _canvas;
        public bool IsPlayer;
        void Start()
        {
            _canvas = GetComponent<Canvas>();
            if(!IsPlayer)
                _statusBars = new TargetStatusBars(_canvas);
            else
                _playerStatusBars = new PlayerStatusBars(_canvas);
        }

        void FixedUpdate()
        {
            if(!IsPlayer)
            {
                _statusBars.Init(TargetEnemy.TargetName);
                _statusBars.SettingSizes();
                _statusBars.SettingText();
            }
            else if(IsPlayer)
            {
                _playerStatusBars.SettingSizes();
                _playerStatusBars.SettingText();
            }
        }
    }
}
