using Assets.Scripts.CombatSystemFolder;
using Assets.Scripts.EnemyFolder;
using UnityEngine;

namespace Assets.Scripts.PlayerFolder
{
    public class CharacterComponent:MonoBehaviour
    {
        public Player Player;
        private IPlayerCombatSystem _combatSystem;
        void Start()
        {
            Player = new Player(name,100,100);
            _combatSystem = new CombatSystem(null);
        }

        void Update()
        {
            Player.Moving(transform);
            Player.Run();
            _combatSystem.Attack();
        }

        void FixedUpdate()
        {
            Player.EnergyRegeneration();
            if(Messenger.IsListenerReady(name + "Health", name + "MaxHealth", name + "Energy", name + "MaxEnergy"))
                Player.AddBroadcasts(name);
        }
        
    }
}
