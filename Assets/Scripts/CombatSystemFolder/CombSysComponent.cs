using System;
using Assets.Scripts.PlayerFolder;
using UnityEngine;

namespace Assets.Scripts.CombatSystemFolder
{
    public enum ETag
    {
        Player,
        Enemy
    }

    public class CombSysComponent : MonoBehaviour
    {

        // Use this for initialization
        private ETag _eTag;
        private TargetEnemy _targetEnemy;
        void Start()
        {
            _eTag = (ETag)Enum.Parse(typeof(ETag), gameObject.tag);
            _targetEnemy = new TargetEnemy();
        }

        // Update is called once per frame
        void Update()
        {
            _targetEnemy.Hit();
        }
    }
}
