using UnityEngine;

namespace Assets.Scripts.PlayerFolder
{
    public class BaseCharacter
    {
        public string Name { get; protected set; }
        public float MaxHealth { get; protected set; }
        public float CurrentHealth { get; protected set; }
        public float MaxEnergy { get; protected set; }
        public float CurrentEnergy { get; protected set; }
        public float Speed { get; protected set; }
        public float DefaultSpeed { get; private set; }
        public Vector3 Position { get; set; }
        public EPlayerState EPlayerState { get; protected set; }
        public float AttackSpeed { get; protected set; }
        public float Damage { get; protected set; }
        private float _energyTimer;
        private readonly float _energyRegeneration;

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

        protected virtual void Walk()
        {
            EPlayerState = EPlayerState.Walk;
            if (Speed > DefaultSpeed)
                Speed -= Mathf.Lerp(0, 2f, Time.deltaTime);
            else Speed = DefaultSpeed;
        }

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

        public void DamageTaken(float damage)
        {
            if (EPlayerState == EPlayerState.Dead)
                return;
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                EPlayerState= EPlayerState.Dead;
            }
        }

        public void AddBroadcasts(string name)
        {
            Messenger<float>.Broadcast(name + "Health", CurrentHealth);
            Messenger<float>.Broadcast(name + "MaxHealth", MaxHealth);
            Messenger<float>.Broadcast(name + "Energy", CurrentEnergy);
            Messenger<float>.Broadcast(name + "MaxEnergy", MaxEnergy);
        }
    }
}
