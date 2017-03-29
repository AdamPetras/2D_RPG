using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.CombatSystemFolder;
using UnityEngine;

namespace Assets.Scripts.HUD
{
    public abstract class StatusBars
    {
        private float currHealth;
        private float currEnergy;
        private float maxHealth;
        private float maxEnergy;
        private const int ySize = 10;
        private const int maxBarLenght = 150;
        private GetStatusComponents _getComponents;
        protected StatusBars(Canvas canvas)
        {
            _getComponents = new GetStatusComponents(canvas);
        }

        public virtual void Init(string name)
        {
            AddListeners(name);
        }

        protected void AddListeners(string name)
        {
            Messenger<float>.AddListener(name + "Health", SetHealth);
            Messenger<float>.AddListener(name + "Energy", SetEnergy);
            Messenger<float>.AddListener(name + "MaxHealth", SetMaxHealth);
            Messenger<float>.AddListener(name + "MaxEnergy", SetMaxEnergy);
        }

        protected void RemoveListeners(string name)
        {
            Messenger<float>.RemoveListener(name + "Health", SetHealth);
            Messenger<float>.RemoveListener(name + "Energy", SetEnergy);
            Messenger<float>.RemoveListener(name + "MaxHealth", SetMaxHealth);
            Messenger<float>.RemoveListener(name + "MaxEnergy", SetMaxEnergy);
        }

        private void SetHealth(float constant)
        {
            currHealth = constant;
        }
        private void SetEnergy(float constant)
        {
            currEnergy = constant;
        }
        private void SetMaxHealth(float constant)
        {
            maxHealth = constant;
        }
        private void SetMaxEnergy(float constant)
        {
            maxEnergy = constant;
        }

        private float CurrentBarLenght(float currSize, float maxSize, float maxLenght)
        {
            return (int)((currSize / maxSize) * maxLenght);
        }

        public void SettingSizes()
        {
            _getComponents.GetHealhBar().rectTransform.sizeDelta = new Vector2(CurrentBarLenght(currHealth, maxHealth, maxBarLenght), ySize);
            _getComponents.GetEnergyBar().rectTransform.sizeDelta = new Vector2(CurrentBarLenght(currEnergy, maxEnergy, maxBarLenght), ySize);
        }

        public void SettingText()
        {
            _getComponents.GetHealthText().text = (int)currHealth + "/" + (int)maxHealth + " Health";
            _getComponents.GetEnergyText().text = (int)currEnergy + "/" + (int)maxEnergy + " Energy";
        }
    }
}
