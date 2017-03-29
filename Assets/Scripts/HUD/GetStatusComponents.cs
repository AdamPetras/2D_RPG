using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.HUD
{
    public class GetStatusComponents
    {
        private Canvas _canvas;
        public GetStatusComponents (Canvas canvas)
        {
            _canvas = canvas;
        }

        public Transform GetMainPanel()
        {
            return _canvas.transform.Find("Panel");
        }

        public Image GetHealhBar()
        {
            return GetMainPanel().transform.Find("Health").GetComponent<Image>();
        }
        public Image GetEnergyBar()
        {
            return GetMainPanel().transform.Find("Energy").GetComponent<Image>();
        }
        public Text GetHealthText()
        {
            return GetMainPanel().transform.Find("HealthText").GetComponent<Text>();
        }
        public Text GetEnergyText()
        {
            return GetMainPanel().transform.Find("EnergyText").GetComponent<Text>();
        }

    }
}
