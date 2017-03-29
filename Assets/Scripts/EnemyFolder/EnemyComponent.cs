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
           // Debug.Log(_enemy.CurrentHealth+ "and" +_enemy.CurrentEnergy);
        }

        void FixedUpdate()
        {
            _enemy.EnergyRegeneration();
            if(TargetEnemy.ETarget == ETarget.Enemy)
            _enemy.AddBroadcasts(Name);
        }
    }
}
