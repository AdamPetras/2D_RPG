using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.EnemyFolder;
using Assets.Scripts.PlayerFolder;
using UnityEngine;

namespace Assets.Scripts.CombatSystemFolder
{
    public class CombatSystem : IEnemyCombatSystem, IPlayerCombatSystem
    {
        private float _attackSpeedTimer;
        private float _targDamage;
        private float _targAttSpeed;
        private GameObject _playerGo;
        private Enemy _enemy;
        public CombatSystem(Enemy enemy)
        {
            _playerGo = GameObject.FindGameObjectWithTag("Player");
            _enemy = enemy;
            _attackSpeedTimer = 0;
        }

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

        public void EnemyAttack()
        {
            Player player = _playerGo.GetComponent<CharacterComponent>().Player;
            if(_enemy != null)
                AttackService(player, player.DamageTaken, _enemy.EPlayerState, _enemy.Damage);
        }

        public void PlayerAttack()
        {
            if(TargetCharacter.TargetName != null)
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
        void EnemyAttack();
    }

    public interface IPlayerCombatSystem
    {
        void PlayerAttack();
    }
}
