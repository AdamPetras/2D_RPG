using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.CombatSystemFolder;
using UnityEngine;

namespace Assets.Scripts.EnemyFolder
{
    public class EnemyComponent : MonoBehaviour
    {
        private Enemy _enemy;
        public string Name;
        public float Health;
        public float Energy;
        void Start()
        {
            _enemy = new Enemy(Name,Health,Energy);
        }

        void Update()
        {
            _enemy.Run();
        }

        void FixedUpdate()
        {
            _enemy.EnergyRegeneration();
            
        }

        void LateUpdate()
        {
            if (Messenger.IsListenerReady("EnemyHealth","EnemyMaxHealth","EnemyEnergy","EnemyMaxEnergy"))
            {
                _enemy.AddBroadcasts(Name);
            }
        }
    }
}
