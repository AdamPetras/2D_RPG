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

    public class TargetComponent : MonoBehaviour
    {

        // Use this for initialization
        private TargetCharacter _targetCharacter;
        void Start()
        {
            _targetCharacter = new TargetCharacter();
        }

        // Update is called once per frame
        void Update()
        {
            _targetCharacter.Hit();
        }
    }
}
