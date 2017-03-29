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

    public class TargetEnemy
    {
        public static string TargetName;
        public static ETarget ETarget;

        public TargetEnemy()
        {
            ETarget = ETarget.None;
        }

        public void Hit()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GetRay().collider != null && Enum.GetNames(typeof(ETarget)).Any(s=>s == GetRay().transform.tag))
                {
                        TargetName = GetRay().transform.name;
                        //Debug.Log("Target Position: " + hit.transform.name);
                }
                else
                {
                    TargetName = null;
                    ETarget = ETarget.None;
                }
                // Debug.Log(Target);
            }
        }

        private RaycastHit2D GetRay()
        {
            return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        }
    }
}
