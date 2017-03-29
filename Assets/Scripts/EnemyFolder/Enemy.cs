using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.PlayerFolder;

namespace Assets.Scripts.EnemyFolder
{
    public class Enemy : CharacterBase
    {
        public bool Angry { get; set; }
        public Enemy(string name, float health, float energy) : base(name, health, energy)
        {
            Angry = false;
        }

        public override void Run()
        {
            if(Angry)
            base.Run();
            else Walk();
        }
    }
}
