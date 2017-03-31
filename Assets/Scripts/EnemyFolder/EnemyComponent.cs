using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.CombatSystemFolder;
using Assets.Scripts.PlayerFolder;
using UnityEngine;

namespace Assets.Scripts.EnemyFolder
{
    public class EnemyComponent : MonoBehaviour
    {
        public Enemy Enemy;
        public string Name;
        public float Health;
        public float Energy;
        public float Damage;
        public float AttackSpeed;
        private IEnemyCombatSystem _combatSystem;
        void Start()
        {
            Enemy = new Enemy(Name, Health, Energy,Damage,AttackSpeed) {Position = transform.position};
            _combatSystem = new CombatSystem(Enemy);
        }

        void Update()
        {
            Enemy.Run();
            if(Enemy.Angry)
            _combatSystem.Attack();
        }

        void FixedUpdate()
        {
            Enemy.EnergyRegeneration();
        }

        void LateUpdate()
        {
            if (Messenger.IsListenerReady(name+"Health",name+"MaxHealth",name+"Energy",name+"MaxEnergy"))
            {
                Enemy.AddBroadcasts(Name);
            }
        }
    }
}
