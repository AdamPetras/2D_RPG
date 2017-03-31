using System;
using Assets.Scripts.CombatSystemFolder;
using UnityEngine;

namespace Assets.Scripts.PlayerFolder
{
    /// <summary>
    /// Abstract class that is inherited by other character classes.
    /// </summary>
    public abstract class BaseCharacter
    {
        /// <summary> Name of charater.</summary>
        public string Name { get; protected set; }
        ///<summary>Maximum health of character.</summary>
        public float MaxHealth { get; protected set; }
        ///<summary>Current health of character.</summary>
        public float CurrentHealth { get; protected set; }
        ///<summary>Maximul energy of character.</summary>
        public float MaxEnergy { get; protected set; }
        ///<summary>Current energy of character.</summary>
        public float CurrentEnergy { get; protected set; }
        ///<summary>Actual movement speed of character.</summary>
        public float Speed { get; protected set; }
        ///<summary>Default movement speed of character.</summary>
        public float DefaultSpeed { get; private set; }
        ///<summary>Actual position of character.</summary>
        public Vector3 Position { get; set; }
        ///<summary>Player state of character.</summary>
        public EPlayerState EPlayerState { get; protected set; }
        ///<summary>Attack speed of character.</summary>
        public float AttackSpeed { get; protected set; }
        ///<summary>Attack damage of character.</summary>
        public float Damage { get; protected set; }
        ///<summary>Timer to that count when the energy could regenerate.</summary>
        private float _energyTimer;
        ///<summary>Readonly variable that shows how many points of <see cref="CurrentEnergy"/> will be regenerate.</summary>
        private readonly float _energyRegeneration;

        /// <summary>
        /// The constructor of <see cref="BaseCharacter"/>
        /// </summary>
        /// <param name="name">The name of character</param>
        /// <param name="health">Indicates health of character</param>
        /// <param name="energy">Indicates energy of character</param>
        public BaseCharacter(string name, float health, float energy)
        {
            Name = name;
            MaxHealth = health;
            CurrentHealth = MaxHealth;
            MaxEnergy = energy;
            CurrentEnergy = MaxEnergy;
            Speed = 1;
            DefaultSpeed = Speed;
            Position = Vector3.zero;
            _energyTimer = 0;
            _energyRegeneration = 5;
        }
        /// <summary>
        /// Control run of character and consumption of energy.
        /// </summary>
        public virtual void Run()
        {
            if(CurrentEnergy > 0)
            {
                CurrentEnergy -= 0.2f;
                EPlayerState = EPlayerState.Run;
                _energyTimer = 0;
                if (Speed < 2f)
                    Speed += Mathf.Lerp(0, 2f, Time.deltaTime);
                else Speed = 2f;
            }
        }
        /// <summary>
        /// Control walking and speed of character.
        /// </summary>
        protected virtual void Walk()
        {
            EPlayerState = EPlayerState.Walk;
            if (Speed > DefaultSpeed)
                Speed -= Mathf.Lerp(0, 2f, Time.deltaTime);
            else Speed = DefaultSpeed;
        }

        /// <summary>
        /// Manage regeneration of energy that is based on time.
        /// </summary>
        public virtual void EnergyRegeneration()
        {
            if(_energyTimer < _energyRegeneration + 1)
            {
                _energyTimer += Time.deltaTime;
            }
            if(_energyTimer >= _energyRegeneration && CurrentEnergy < MaxEnergy)
            {
                CurrentEnergy += 0.05f;
                if(CurrentEnergy >= MaxEnergy) //přetečení
                    CurrentEnergy = MaxEnergy; //zarovnání
            }
        }

        /// <summary>
        /// Called in <see cref="CombatSystem"/> to control damage and health of characters.
        /// </summary>
        /// <param name="eState">State that control if isnt already dead.</param>
        /// <param name="damage">Constant damage.</param>
        public virtual void DamageTaken(EPlayerState eState, float damage)
        {
            if (eState == EPlayerState.Dead || EPlayerState == EPlayerState.Dead)
                return;
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                EPlayerState = EPlayerState.Dead;           
                CurrentHealth = 0;             
            }
        }
        /// <summary>
        /// Call methods from the class <see cref="Messenger"/> and broadcast variables.
        /// </summary>
        /// <param name="name">Param to differentiate broadcasts.</param>
        public void AddBroadcasts(string name)
        {
            Messenger<float>.Broadcast(name + "Health", CurrentHealth);
            Messenger<float>.Broadcast(name + "MaxHealth", MaxHealth);
            Messenger<float>.Broadcast(name + "Energy", CurrentEnergy);
            Messenger<float>.Broadcast(name + "MaxEnergy", MaxEnergy);
        }

        
    }
    
}
