using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
           // _enemy.AddBroadcasts(Name);
        }
    }
}
