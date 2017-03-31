using Assets.Scripts.CombatSystemFolder;
using UnityEngine;

namespace Assets.Scripts.HUD
{
    /// <summary>
    /// Manage everything about of status bar of player and target and then it control drawing of it.
    /// </summary>
    public class StatusBars:IPlayerStatusBar,ITargetStatusBar
    {
        /// <summary>Current health of target or player.</summary>
        private float currHealth;
        /// <summary>Current energy of target or player.</summary>
        private float currEnergy;
        /// <summary>Maximal health of target or player.</summary>
        private float maxHealth;
        /// <summary>Maximal energy of target or player.</summary>
        private float maxEnergy;
        /// <summary>Constant indicate maximal y size of bars.</summary>
        private const int ySize = 10;
        /// <summary>Constant indicate maximal x size of bars.</summary>
        private const int maxBarLenght = 150;
        /// <summary>Class to get components from canvas.</summary>
        private GetStatusComponents _getComponents;
        /// <summary>Class canvas to manage drawing.</summary>
        private Canvas _canvas;
        /// <summary>Variable sets the name of target to indicate null target.</summary>
        private string _name;
        /// <summary>
        /// The constructor of <see cref="StatusBars"/>.
        /// </summary>
        /// <param name="canvas">Ist used to disable and enable canvas rendering.</param>
        public StatusBars(Canvas canvas)
        {
            _getComponents = new GetStatusComponents(canvas);
            _canvas = canvas;
            _name = "";
        }
        /// <summary>
        /// <see cref="IPlayerStatusBar.Init"/>
        /// </summary>
        /// <param name="name">Name of player</param>
        void IPlayerStatusBar.Init(string name)
        {
            AddListeners(name);
        }
        /// <summary>
        /// <see cref="IPlayerStatusBar.Init"/>
        /// </summary>
        /// <param name="name">Name of target.</param>
        void ITargetStatusBar.Init(string name)
        {
            if((_name == "" && TargetCharacter.TargetName != null))
            {
                _canvas.enabled = true;
                AddListeners(name);
                _name = name;
            }
            else if((_name != TargetCharacter.TargetName && _name != "") || (_name != "" && TargetCharacter.TargetName == null))
            {
                _canvas.enabled = false;
                RemoveListeners(_name);
                _name = "";
            }
        }

        /// <summary>
        /// Call methods from the class <see cref="Messenger"/> and listen the broadcasts in loop.
        /// </summary>
        /// <param name="name">Param to differentiate broadcasts.</param>
        protected void AddListeners(string name)
        {
            Messenger<float>.AddListener(name + "Health", SetHealth);
            Messenger<float>.AddListener(name + "Energy", SetEnergy);
            Messenger<float>.AddListener(name + "MaxHealth", SetMaxHealth);
            Messenger<float>.AddListener(name + "MaxEnergy", SetMaxEnergy);
        }
        /// <summary>
        /// Call methods from the class <see cref="Messenger"/> and remove listeners.
        /// </summary>
        /// <param name="name">Param to differentiate broadcasts.</param>
        protected void RemoveListeners(string name)
        {
            Messenger<float>.RemoveListener(name + "Health", SetHealth);
            Messenger<float>.RemoveListener(name + "Energy", SetEnergy);
            Messenger<float>.RemoveListener(name + "MaxHealth", SetMaxHealth);
            Messenger<float>.RemoveListener(name + "MaxEnergy", SetMaxEnergy);
        }
        /// <summary>
        /// Sets <see cref="currHealth"/> which is given by the certain broadcasting.
        /// </summary>
        /// <param name="constant">Value from listener.</param>
        private void SetHealth(float constant)
        {
            currHealth = constant;
        }
        /// <summary>
        /// Sets <see cref="currEnergy"/> which is given by the certain broadcasting.
        /// </summary>
        /// <param name="constant">Value from listener.</param>
        private void SetEnergy(float constant)
        {
            currEnergy = constant;
        }
        /// <summary>
        /// Sets <see cref="maxHealth"/> which is given by the certain broadcasting.
        /// </summary>
        /// <param name="constant">Value from listener.</param>
        private void SetMaxHealth(float constant)
        {
            maxHealth = constant;
        }
        /// <summary>
        /// Sets <see cref="maxEnergy"/> which is given by the certain broadcasting.
        /// </summary>
        /// <param name="constant">Value from listener.</param>
        private void SetMaxEnergy(float constant)
        {
            maxEnergy = constant;
        }
        /// <summary>
        /// Calculates the actual lenght of bar.
        /// </summary>
        /// <param name="currSize">Current size of bar which is given by <see cref="currHealth"/> or <see cref="currEnergy"/>.</param>
        /// <param name="maxSize">Maximum size of bar which is given by <see cref="maxHealth"/> or <see cref="maxEnergy"/>.</param>
        /// <param name="maxLenght">Maximum size of whole bar which is given by <see cref="maxBarLenght"/>.</param>
        /// <returns></returns>
        private float CurrentBarLenght(float currSize, float maxSize, float maxLenght)
        {
            return (int)((currSize / maxSize) * maxLenght);
        }
        /// <summary>
        /// Called in loop and it calls the <see cref="CurrentBarLenght"/> to calc sizes.
        /// </summary>
        public void SettingSizes()
        {
            _getComponents.GetHealhBar().rectTransform.sizeDelta = new Vector2(CurrentBarLenght(currHealth, maxHealth, maxBarLenght), ySize);
            _getComponents.GetEnergyBar().rectTransform.sizeDelta = new Vector2(CurrentBarLenght(currEnergy, maxEnergy, maxBarLenght), ySize);
        }
        /// <summary>
        /// <see cref="ITargetStatusBar.SettingText"/>
        /// </summary>
        public void SettingText()
        {
            _getComponents.GetHealthText().text = (int)currHealth + "/" + (int)maxHealth + " Health";
            _getComponents.GetEnergyText().text = (int)currEnergy + "/" + (int)maxEnergy + " Energy";
        }

    }

    public interface IPlayerStatusBar
    {
        /// <summary>
        /// Initialize status bar and manage it.
        /// </summary>
        /// <param name="name">Name of player</param>
        void Init(string name);
        /// <summary>
        /// Called in loop and it set sizes of bars;
        /// </summary>
        void SettingSizes();
        /// <summary>
        /// Called in loop and it set text in bars.
        /// </summary>
        void SettingText();
    }

    public interface ITargetStatusBar
    {
        /// <summary>
        /// Initialize status bar and manage it.
        /// </summary>
        /// <param name="name">Name of player</param>
        void Init(string name);
        /// <summary>
        /// Called in loop and it set sizes of bars;
        /// </summary>
        void SettingSizes();
        /// <summary>
        /// Called in loop and it set text in bars.
        /// </summary>
        void SettingText();
    }
}
