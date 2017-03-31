using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.CombatSystemFolder;
using Assets.Scripts.PlayerFolder;
using UnityEngine;

namespace Assets.Scripts.EnemyFolder
{
    /// <summary>
    /// Class that manage everything about enemy and his actions.
    /// </summary>
    public class Enemy : BaseCharacter
    {
        /// <summary> Variable to get it enemy is angry or not.</summary>
        public bool Angry { get; set; }
        /// <summary>
        /// Constuctor of <see cref="Enemy"/>
        /// </summary>
        /// <param name="name">Enemy name.</param>
        /// <param name="health">Enemy health.</param>
        /// <param name="energy">Enemy energy.</param>
        /// <param name="damage">Enemy damage.</param>
        /// <param name="attackSpeed">Enemy attack speed.</param>
        public Enemy(string name, float health, float energy,float damage,float attackSpeed) : base(name, health, energy)
        {
            Angry = false;
            Damage = damage;
            AttackSpeed = attackSpeed;
        }
        /// <summary>
        /// Override method <see cref="Run"/> because enemy must control it by itself.
        /// </summary>
        public override void Run()
        {
            if (EPlayerState == EPlayerState.Dead)
                return;
            if(Angry)
            base.Run();
            else Walk();
        }
        /// <summary>
        /// Override method <see cref="DamageTaken"/> because enemy have to do it otherwise.
        /// </summary>
        /// <param name="eState">State to get state of character.</param>
        /// <param name="damage">Constant damage</param>
        public  override void DamageTaken(EPlayerState eState, float damage)
        {
            Angry = true;
            base.DamageTaken(eState,damage);
        }
    }
}
