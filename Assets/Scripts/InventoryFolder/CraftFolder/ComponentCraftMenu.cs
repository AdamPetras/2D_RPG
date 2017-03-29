using UnityEngine;

namespace Assets.Scripts.InventoryFolder.CraftFolder
{
    public class ComponentCraftMenu : MonoBehaviour
    {

        private CraftSettings _craftSettings;
        public static bool Draw;

        // Use this for initialization
        void Start()
        {
            _craftSettings = new CraftSettings(GetComponent<Canvas>());
        }

        // Update is called once per frame
        void Update()
        {
            _craftSettings.Update();
        }
    }
}
