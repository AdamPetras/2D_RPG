using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.InventoryFolder.CraftFolder
{
    public class GetCraftComponent
    {
        private Canvas _canvas;
        public GetCraftComponent(Canvas canvas)
        {
            _canvas = canvas;
        }

        public InputField GetSearch()
        {
            return GetMainPanel().transform.Find("Search").GetComponentInChildren<InputField>();
        }

        public Transform GetMainPanel()
        {
            return _canvas.transform.Find("Panel");
        }

        public Transform GetScrollView()
        {
            return GetMainPanel().transform.Find("ScrollView");
        }

        public Transform GetButtonPanel()
        {
            return GetMainPanel().transform.Find("ButtonPanel");
        }

        public Transform GetInfoPanel()
        {
            return GetMainPanel().transform.Find("InfoPanel");
        }

        public Transform GetCraftNeedPanel()
        {
            return GetInfoPanel().transform.Find("NeedPanel");
        }

        public Transform GetProductPanel()
        {
            return GetInfoPanel().transform.Find("ProductPanel");
        }

        public Button GetCraftButton()
        {
            return GetMainPanel().transform.Find("Craft").GetComponent<Button>();
        }

        public Toggle GetCookingToggle()
        {
            return GetButtonPanel().transform.Find("Cooking").GetComponent<Toggle>();
        }
        public Toggle GetSmithingToggle()
        {
            return GetButtonPanel().transform.Find("Smithing").GetComponent<Toggle>();
        }
        public Toggle GetCraftingToggle()
        {
            return GetButtonPanel().transform.Find("Crafting").GetComponent<Toggle>();
        }
        public Toggle GetTailoringToggle()
        {
            return GetButtonPanel().transform.Find("Tailoring").GetComponent<Toggle>();
        }
    }
}
