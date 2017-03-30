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
        private IPlayerStatusBar _playerStatusBar;
        private ITargetStatusBar _targetStatusBar;
        public bool IsPlayer;
        void Start()
        {
            if(!IsPlayer)
                _targetStatusBar = new StatusBars(GetComponent<Canvas>());
            else
                _playerStatusBar = new StatusBars(GetComponent<Canvas>());
        }

        void Update()
        {
            if(!IsPlayer)
            {
                _targetStatusBar.Init(TargetCharacter.TargetName);
                _targetStatusBar.SettingSizes();
                _targetStatusBar.SettingText();
            }
            else if(IsPlayer)
            {
                _playerStatusBar.Init("Player");
                _playerStatusBar.SettingSizes();
                _playerStatusBar.SettingText();
            }
        }
    }
}
