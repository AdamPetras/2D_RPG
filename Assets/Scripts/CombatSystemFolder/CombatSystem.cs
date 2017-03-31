using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.EnemyFolder;
using Assets.Scripts.PlayerFolder;
using UnityEngine;

namespace Assets.Scripts.CombatSystemFolder
{
    /// <summary>
    /// Control combat system of enemy and player. It also solve something like attack timing and so on.
    /// </summary>
    public class CombatSystem : IEnemyCombatSystem, IPlayerCombatSystem
    {
        /// <summary>Timer to manage attack speed.</summary>
        private float _attackSpeedTimer;
        /// <summary>Object of player to get his component.</summary>
        private GameObject _playerGo;
        /// <summary>Class to get informations about enemy.</summary>
        private Enemy _enemy;
        /// <summary>
        /// Constructor to create new instace of <see cref="CombatSystem"/>
        /// </summary>
        /// <param name="enemy">Could be enemy but if it will be called in in player comp it should be null</param>
        public CombatSystem(Enemy enemy)
        {
            _playerGo = GameObject.FindGameObjectWithTag("Player");
            _enemy = enemy;
            _attackSpeedTimer = 0;
        }
        /// <summary>
        /// Manage distance of two players if condition is satisfied then there is a counter which used to count attack speed.
        /// </summary>
        /// <param name="player">Input param to get position and so on.</param>
        /// <param name="damageTaken">Input is method to manage health.</param>
        /// <param name="eState">Enum to get state of player or enemy.</param>
        /// <param name="damage">Its constant damage.</param>
        private void AttackService(Player player, Action<EPlayerState, float> damageTaken, EPlayerState eState, float damage)
        {
            if(Vector2.Distance(player.Position, _enemy.Position) < 5)
            {
                _attackSpeedTimer += Time.deltaTime;
                if(_attackSpeedTimer > player.AttackSpeed)
                {
                    if(Vector2.Distance(player.Position, _enemy.Position) < 0.5f)
                    {
                        damageTaken(eState, damage);
                        _attackSpeedTimer = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Manage condition that must be satisfied and call <see cref="AttackService"/>.
        /// </summary>
        void IEnemyCombatSystem.Attack()
        {
            Player player = _playerGo.GetComponent<CharacterComponent>().Player;
            if(_enemy != null)
                AttackService(player, player.DamageTaken, _enemy.EPlayerState, _enemy.Damage);
        }
        /// <summary>
        /// Manage condition that must be satisfied and call <see cref="AttackService"/>.
        /// </summary>
        void IPlayerCombatSystem.Attack()
        {
            if(TargetCharacter.TargetName != null && TargetCharacter.EnumTarget != ETarget.Player)
                _enemy = GameObject.Find(TargetCharacter.TargetName).GetComponent<EnemyComponent>().Enemy;
            else
                _enemy = null;
            Player player = _playerGo.GetComponent<CharacterComponent>().Player;
            if(_enemy != null)
            {
                AttackService(player, _enemy.DamageTaken, player.EPlayerState, player.Damage);
            }
        }
    }

    public interface IEnemyCombatSystem
    {
        /// <summary>
        /// Manage condition that must be satisfied and manage attack of player.
        /// </summary>
        void Attack();
    }

    public interface IPlayerCombatSystem
    {
        /// <summary>
        /// Manage condition that must be satisfied and manage attack of player.
        /// </summary>
        void Attack();
    }
}
