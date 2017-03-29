using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.HUD
{
    public class ComponentStatusBars : MonoBehaviour
    {
        private StatusBars _statusBars;
        private Canvas _canvas;
        void Start()
        {
            _canvas = GetComponent<Canvas>();
            _statusBars = new StatusBars("Player",_canvas);
        }

        void Update()
        {
            _statusBars.SettingSizes();
            _statusBars.SettingText();
        }
    }
}
