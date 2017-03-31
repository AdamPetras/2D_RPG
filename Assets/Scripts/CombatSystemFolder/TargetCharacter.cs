using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.CombatSystemFolder
{
    /// <summary>
    /// Enum to set type of target.
    /// </summary>
    public enum ETarget
    {
        Enemy,
        Player,
        None
    }

    /// <summary>
    /// Control targeting of any character that is really in game. It also target player like a target.
    /// </summary>
    public class TargetCharacter
    {
        /// <summary>Static transform of target to get any informations about him.</summary>
        public static Transform Target;
        /// <summary>Static enum about target to get what target is it.</summary>
        public static ETarget EnumTarget;
        /// <summary>Static name of target to get his name.</summary>
        public static string TargetName;
        /// <summary>Timer to manage clicking of mouse if i will want to hold.</summary>
        private float _treshHoldTimer;
        /// <summary>Constant for <see cref="_treshHoldTimer"/></summary>
        private const float _treshHold = 0.2f;
        /// <summary>
        /// The constructor of <see cref="TargetCharacter"/>
        /// </summary>
        public TargetCharacter()
        {
            Target = null;
            TargetName = null;
            _treshHoldTimer = 0;
            EnumTarget = ETarget.None;
        }

        /// <summary>
        /// Manage mouse clicking and then call <see cref="SetTarget"/>
        /// </summary>
        public void Hit()
        {
            if(Input.GetMouseButton(0))
            {
                _treshHoldTimer += Time.deltaTime;
            }
            if(Input.GetMouseButtonUp(0))
            {
                if(_treshHoldTimer < _treshHold)
                {
                    SetTarget();
                }
                else
                    _treshHoldTimer = 0;
            }
        }
        /// <summary>
        /// Manage condition and then call <see cref="SetValues"/> to set the values while collide.
        /// </summary>
        private void SetTarget()
        {
            if(GetRay().collider != null && Enum.GetNames(typeof(ETarget)).Any(s => s == GetRay().transform.tag))
            {
                SetValues(GetRay().transform, GetRay().transform.name, (ETarget)Enum.Parse(typeof(ETarget), GetRay().transform.tag));
            }
            else
            {
                SetValues(null,null,ETarget.None);
            }
        }

        /// <summary>
        /// It sets values of static variables that are used in the other classes.
        /// </summary>
        /// <param name="target">Parameter that sets transform of target</param>
        /// <param name="name">This is the name of target</param>
        /// <param name="eTarget">Enum type of target tag</param>
        private void SetValues(Transform target, string name, ETarget eTarget)
        {
            Target = target;
            TargetName = name;
            EnumTarget = eTarget;
            _treshHoldTimer = 0;
        }
        /// <summary>
        /// Manage collision of mouse with any collider from cursor position.
        /// </summary>
        /// <returns>Raycasthit of mouse cursor</returns>
        private RaycastHit2D GetRay()
        {
            return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        }
    }
}
