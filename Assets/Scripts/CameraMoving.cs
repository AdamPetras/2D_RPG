using Assets.Scripts.InventoryFolder.CraftFolder;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraMoving : MonoBehaviour
    {

        private GameObject _player;
        private const float ZOOMSPEED = 1;
        private const float SMOTHSPEED = 5f;
        private const float MAXZOOM = 5f;
        private const float MINZOOM = 1f;
        private float _cameraOrtho;
        private Vector3 _default;
        void Start()
        {

            _player = GameObject.Find("Player");
            _cameraOrtho = Camera.main.orthographicSize;
            _default = new Vector3(0, 0, -10);    //defaultní hodnota (střed monitoru)
        }

        void Update()
        {
            if (!ComponentCraftMenu.Draw)
            {
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (scroll != 0f)
                {

                    _cameraOrtho -= scroll*ZOOMSPEED;
                    _cameraOrtho = Mathf.Clamp(_cameraOrtho, MINZOOM, MAXZOOM);
                }
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, _cameraOrtho,
                    SMOTHSPEED*Time.deltaTime);
            }
        }

        void LateUpdate()
        {
            transform.position = _player.transform.position + _default;
        }
    }
}
