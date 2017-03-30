using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.CombatSystemFolder
{
    public enum ETarget
    {
        Enemy,
        Player,
        None
    }

    public class TargetCharacter
    {
        public static Transform Target;
        public static string TargetName;
        private float _treshHoldTimer;
        private const float _treshHold = 0.2f;

        public TargetCharacter()
        {
            Target = null;
            TargetName = null;
            _treshHoldTimer = 0;
        }


        public void Hit()
        {
            if (Input.GetMouseButton(0))
            {
                _treshHoldTimer += Time.deltaTime;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (_treshHoldTimer < _treshHold)
                {
                    SetTarget();
                }
                else _treshHoldTimer = 0;
            }
        }

        private void SetTarget()
        {
            if(GetRay().collider != null &&
                           Enum.GetNames(typeof(ETarget)).Any(s => s == GetRay().transform.tag))
            {
                Target = GetRay().transform;
                TargetName = Target.name;
                _treshHoldTimer = 0;
            }
            else
            {
                Target = null;
                TargetName = null;
                _treshHoldTimer = 0;
            }
        }

        private RaycastHit2D GetRay()
        {
            return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        }
    }
}
