using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.CombatSystemFolder;
using Assets.Scripts.PlayerFolder;
using UnityEngine;

namespace Assets.Scripts.EnemyFolder
{
    public class Enemy : BaseCharacter
    {
        public bool Angry { get; set; }
        public Enemy(string name, float health, float energy,float damage,float attackSpeed) : base(name, health, energy)
        {
            Angry = false;
            Damage = damage;
            AttackSpeed = attackSpeed;
        }

        public override void Run()
        {
            if (EPlayerState == EPlayerState.Dead)
                return;
            if(Angry)
            base.Run();
            else Walk();
        }

        public  override void DamageTaken(EPlayerState eState, float damage)
        {
            Angry = true;
            base.DamageTaken(eState,damage);
        }
    }
}
