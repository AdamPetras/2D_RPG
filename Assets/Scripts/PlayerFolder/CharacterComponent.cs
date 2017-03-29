using UnityEngine;

namespace Assets.Scripts.PlayerFolder
{
    public class CharacterComponent:MonoBehaviour
    {
        private Player _player;
        void Start()
        {
            _player = new Player(name,100,100);
        }

        void Update()
        {
            _player.Moving(transform);
            _player.Run();
        }

        void FixedUpdate()
        {
            _player.EnergyRegeneration();
            _player.AddBroadcasts(name);
        }
    }
}
