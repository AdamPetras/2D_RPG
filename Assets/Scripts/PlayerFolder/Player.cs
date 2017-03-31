using UnityEngine;

namespace Assets.Scripts.PlayerFolder
{
    /// <summary>
    /// Enum set state of player or enemy.
    /// </summary>
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
    /// <summary>
    /// Class that manage any problem about player and control his events.
    /// </summary>
    public class Player : BaseCharacter
    {
        /// <summary>
        /// Delegate for event to add health from inventory items.
        /// </summary>
        /// <param name="health">Constat to get health</param>
        private delegate void HealthDelegate(int health);
        /// <summary>
        /// Event to add maximal health of player.
        /// </summary>
        private static event HealthDelegate AddMaxHealthEvent;
        /// <summary>
        /// Event to add current health of player.
        /// </summary>
        private static event HealthDelegate AddCurrentHealthEvent;
        /// <summary>It manage regeneration of energy if i have will exhaust energy then it will be set to <value>True</value> and if the energy cross the border than it sets to <value>False</value>.</summary>
        private bool _regen;
        /// <summary>Shows the position before its used in <see cref="Away"/> to manage that player is AFK.</summary>
        private Vector3 _positionBefore;
        /// <summary>Timer for <see cref="Away"/> to set if player is AFK.</summary>
        private float _afkTimer;
        /// <summary>Constant that shows time that is border to be AFK.</summary>
        private const float AFKTIME = 120;  //2minuty
        /// <summary>
        /// Constructor of <see cref="Player"/>
        /// </summary>
        /// <param name="name">Player name</param>
        /// <param name="health">Player health</param>
        /// <param name="energy">Player energy</param>
        public Player(string name, float health, float energy) : base(name, health, energy)
        {
            Damage = 35;
            AttackSpeed = 2;
            AddMaxHealthEvent += AddMaxHealth;
            AddCurrentHealthEvent += AddCurrentHealth;
        }
        /// <summary>
        /// Manage move and rotate of player while any keys are held.
        /// </summary>
        /// <param name="transform">Position and rotate of player</param>
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
        /// <summary>
        /// Control run while left shift key is held.
        /// </summary>
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
        /// <summary>
        /// Set the player state depend on his reactions.
        /// </summary>
        /// <returns>If he is afk or not.</returns>
        private bool Away()
        {
            if (_afkTimer <= AFKTIME)
                _afkTimer += Time.deltaTime;
            if (_positionBefore != Position)
            {
                _positionBefore = Position;
                _afkTimer = 0;
            }
            if (_afkTimer > AFKTIME)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Its called by <see cref="AddMaxHealthEvent"/> to set player max health.
        /// </summary>
        /// <param name="health">Constant health do add</param>
        public void AddMaxHealth(int health)
        {
            if(EPlayerState == EPlayerState.Dead)
                return;
            MaxHealth += health;
        }

        /// <summary>
        /// Its called by <see cref="AddCurrentHealthEvent"/> to set player current health.
        /// </summary>
        /// <param name="health">Constant health to add</param>
        public void AddCurrentHealth(int health)
        {
            if(EPlayerState == EPlayerState.Dead)
                return;
            CurrentHealth += health;
        }
        /// <summary>
        /// Handler that invokes methods from <see cref="AddMaxHealthEvent"/>
        /// </summary>
        /// <param name="health">Constant health</param>
        public static void OnAddMaxHealth(int health)
        {
            if(AddMaxHealthEvent != null)
                AddMaxHealthEvent.Invoke(health);
        }
        /// <summary>
        /// Handler that invokes methods from <see cref="AddCurrentHealthEvent"/>
        /// </summary>
        /// <param name="health">Constant health</param>
        public static void OnAddCurrentHealth(int health)
        {
            if(AddCurrentHealthEvent != null)
                AddCurrentHealthEvent.Invoke(health);
        }
    }
}
