using UnityEngine;

namespace Assets.Scripts.PlayerFolder
{
    public enum EPlayerState
    {
        Walk,
        Run,
        Jump,
        Swim,
        Dead,
        AFK,
        None
    }

    public class Player : BaseCharacter
    {
        private delegate void HealthDelegate(int health);

        private static event HealthDelegate AddMaxHealthEvent;
        private static event HealthDelegate AddCurrentHealthEvent;

        private bool _regen;
        private Vector3 _positionBefore;
        private float _treshHold;
        private const float AFKTIME = 120;  //2minuty

        public Player(string name, float health, float energy) : base(name, health, energy)
        {
            AddMaxHealthEvent += AddMaxHealth;
            AddCurrentHealthEvent += AddCurrentHealth;
        }

        public void Moving(Transform transform)
        {
            if (EPlayerState == EPlayerState.Dead)
                return;
            if (Input.GetAxis("Vertical") != 0)
            {
                transform.position += transform.up * Input.GetAxis("Vertical") * Speed * 0.02f;        
            }
            else if(!Away())
                EPlayerState = EPlayerState.None;
            else EPlayerState = EPlayerState.AFK;
            if (Input.GetAxis("Horizontal") != 0)
            {
                transform.Rotate(0, 0, (Input.GetAxis("Horizontal") * -3));               
            }
            Position = transform.position;
        }
        public override void Run()
        {
            if(EPlayerState == EPlayerState.Dead)
                return;
            if(Input.GetKey("left shift") && CurrentEnergy > 0 && !_regen && (EPlayerState == EPlayerState.Walk || EPlayerState == EPlayerState.Run))
            {
                base.Run();
            }
            else
            {
                if(CurrentEnergy <= 0 && !_regen) //ošetření pokud pořád drží shift
                {
                    _regen = true;
                    CurrentEnergy = 0;
                    Walk();
                }
                if(_regen && CurrentEnergy >= 15)   //pokud vyčerpal uplně energii tak musí čekat až bude energie na 15 poté může zase běhat
                    _regen = false;
                Walk();
            }
        }
        private bool Away()
        {
            if (_treshHold <= AFKTIME)
                _treshHold += Time.deltaTime;
            if (_positionBefore != Position)
            {
                _positionBefore = Position;
                _treshHold = 0;
            }
            if (_treshHold > AFKTIME)
            {
                return true;
            }
            return false;
        }

        public void AddMaxHealth(int health)
        {
            if(EPlayerState == EPlayerState.Dead)
                return;
            MaxHealth += health;
        }

        public void AddCurrentHealth(int health)
        {
            if(EPlayerState == EPlayerState.Dead)
                return;
            CurrentHealth += health;
        }

        public static void OnAddMaxHealth(int health)
        {
            if(AddMaxHealthEvent != null)
                AddMaxHealthEvent.Invoke(health);
        }

        public static void OnAddCurrentHealth(int health)
        {
            if(AddCurrentHealthEvent != null)
                AddCurrentHealthEvent.Invoke(health);
        }

    }
}
