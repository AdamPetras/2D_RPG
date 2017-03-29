using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.Scripts.InventoryFolder.CraftFolder
{

    public class CraftSettings
    {
        private Canvas _canvas;
        private GetCraftComponent _getCraftComponent;
        private GameObject _cardPrefab;
        private List<Item> _defaultCardList;
        private List<Item> _cardList;
        private string _searchString;
        private CraftDataOperating _craftDataOperating;
        private EProfesion _eProfesion;
        private bool _finding;
        private const int XSIZE = 500;
        private const int YSIZE = 400;
        public CraftSettings(Canvas canvas)
        {
            _defaultCardList = new List<Item>();
            _cardList = new List<Item>();
            _canvas = canvas;
            _getCraftComponent = new GetCraftComponent(_canvas);
            _craftDataOperating = new CraftDataOperating(_getCraftComponent);
            _eProfesion = EProfesion.None;
            _finding = false;
            SetCraftMenuSize();
            _cardPrefab = Resources.Load("Prefab/ViewCard") as GameObject;
            _getCraftComponent.GetCookingToggle().onValueChanged.AddListener(ChoiseCooking);
            _getCraftComponent.GetSmithingToggle().onValueChanged.AddListener(ChoiseSmithing);
            _getCraftComponent.GetCraftingToggle().onValueChanged.AddListener(ChoiseCrafting);
            _getCraftComponent.GetTailoringToggle().onValueChanged.AddListener(ChoiseTailoring);
            _defaultCardList = ItemDatabase.Database.Where(s => s.EProfesion != EProfesion.None && s.EProfesion != EProfesion.Fishing).ToList();
        }

        private void SetCraftMenuSize()
        {
            _getCraftComponent.GetMainPanel().GetComponent<RectTransform>().sizeDelta = new Vector2(XSIZE,YSIZE);
            _getCraftComponent.GetMainPanel().GetComponent<RectTransform>().localPosition =
                new Vector2(_getCraftComponent.GetMainPanel().GetComponent<RectTransform>().rect.width / 2,
                    _getCraftComponent.GetMainPanel().GetComponent<RectTransform>().rect.height / 2);
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.P) && !ComponentCraftMenu.Draw)
            {
                _canvas.enabled = true;
                ComponentCraftMenu.Draw = true;
            }
            else if(Input.GetKeyDown(KeyCode.P) && ComponentCraftMenu.Draw)
            {
                _canvas.enabled = false;
                ComponentCraftMenu.Draw = false;
            }
            if(_getCraftComponent.GetSearch().GetComponent<InputField>().text != "")   //pokud je naplněno pole s vyhledáváním
            {
                _finding = true;
                _searchString = _getCraftComponent.GetSearch().GetComponent<InputField>().text.ToLower(); //uložení řetězce do proměnné
                CardFound(_defaultCardList);
            }
            else if(_getCraftComponent.GetSearch().GetComponent<InputField>().text == "" && _finding)   //pokud vyhledávám a smažu pole s vyhledáváním
            {
                AllOfList(_cardList, _defaultCardList);
                _finding = false;
            }
            LoadScrollView(_cardList);
        }
        private void LoadScrollView(List<Item> cardList)
        {
            //načtení itemů z listu
            if(cardList.Count != 0)
            {
                //itemy které nejsou instanciované a popř jsou z vybrané složky
                foreach(Item card in cardList.Where(s => !s.Instantiate))
                {
                    GameObject cardObject = Object.Instantiate(_cardPrefab);   //instanciování        
                    card.Instantiate = true;    //nastavení že byl instanciován                  
                    card.Obj = cardObject;  //uložení objektu pro případné odstranění(úpravu)
                    if(card.Icon != null)  //pokud je nějaká ikona
                        cardObject.transform.Find("Image").GetComponent<Image>().sprite = card.Icon;    //uložení ikony do proměnné
                    cardObject.transform.Find("Name").GetComponent<Text>().text = card.Name;    //uložení jména do proměnné                    
                    cardObject.transform.SetParent(_getCraftComponent.GetScrollView().transform.Find("ViewPort").transform, false);    //nastavení rodiče
                    cardObject.transform.localScale = Vector3.one;  //nastavení transformu na scaleování
                    cardObject.name = card.ID.ToString();
                    //Debug.Log("hah");                   
                    cardObject.GetComponent<Button>().onClick.AddListener(delegate { OnClick(int.Parse(cardObject.name)); });
                }
            }
        }

        private void CardFound(List<Item> viewCardList)
        {
            foreach(Item viewCard in viewCardList)
            {
                if(viewCard.Name.ToLower().Contains(_searchString))    //pokud list obsahuje item podle zadaného řetězce
                {
                    if(_eProfesion == EProfesion.None && viewCard.EProfesion != EProfesion.None)    //hledám neurčitý item
                    {
                        FoundCard(_cardList, viewCard);
                        //Debug.Log(viewCard.Name);
                    }
                    else if(_eProfesion == viewCard.EProfesion)     //hledám určený item 
                    {
                        FoundCard(_cardList, viewCard);
                    }
                }
                else
                {
                    RemoveFromList(_cardList, viewCard);
                }
            }
        }

        private void FoundCard(List<Item> viewCardList, Item viewCard)
        {
            if(viewCardList.All(s => s.Name != viewCard.Name))     //ošetření aby nebylo v cyklu vkládáno několik itemů
            {
                viewCardList.Add(viewCard); //přidání do listu
                //Debug.Log("Nalezeno");
            }
        }

        private void AllOfList(List<Item> destinationList, List<Item> sourceList)
        {
            //if (_eProfesion == EProfesion.None) //ošetření pokud je vybrané políčko a vyplní se vyhledávací lišta a vymaže
            ClearList(_cardList);  //vymazání listu podle toho co je vybrané
                                   // else ClearList(sourceList);
            _searchString = _getCraftComponent.GetSearch().GetComponent<InputField>().text = "";    //načtení z vyhledávacího pole do řetězce
            _finding = false;
            foreach(Item viewCard in sourceList)
            {
                if((viewCard.EProfesion == _eProfesion) && !destinationList.Contains(viewCard))  //pokud je zařazen do... a neni již obsažen
                {
                    destinationList.Add(viewCard);  //přidání listu
                }
            }
        }

        private void RemoveFromList(List<Item> viewCardList, Item viewCard)
        {
            if(viewCardList.Contains(viewCard) || _getCraftComponent.GetSearch().GetComponent<InputField>().text.ToLower() == "")
            {
                viewCardList.Remove(viewCard);  //odstranění vybraného objektu z listu
                Remove(viewCard);
            }
        }

        private void ClearList(List<Item> viewCardList)
        {
            foreach(Item viewCard in viewCardList)
            {
                Remove(viewCard);
            }
            viewCardList.Clear();   //clearnutí celého listu
        }

        private void Remove(Item viewCard)
        {
            viewCard.Obj.GetComponent<Button>().onClick.RemoveAllListeners();
            Object.Destroy(viewCard.Obj);  //destroy objektu
            viewCard.Instantiate = false;   //nastavení proměnné kvůli znovu vytvoření stejného objektu
        }

        private void ChoiseCooking(bool value)
        {
            if(value)
            {
                _eProfesion = EProfesion.Cooking;
                AllOfList(_cardList, _defaultCardList);
                //...ošetření (pokud je u ohně atd...)
                //...
            }
            else
            {
                _eProfesion = EProfesion.None;
                ClearList(_cardList);
            }
        }
        private void ChoiseCrafting(bool value)
        {
            if(value)
            {
                _eProfesion = EProfesion.Crafting;
                AllOfList(_cardList, _defaultCardList);
                //...ošetření (pokud je u stolu(craftingtablu) atd...)
                //...
            }
            else
            {
                _eProfesion = EProfesion.None;
                ClearList(_cardList);
            }
        }

        private void ChoiseTailoring(bool value)
        {
            if(value)
            {
                _eProfesion = EProfesion.Tailoring;
                AllOfList(_cardList, _defaultCardList);
                //...ošetření (pokud je u ohně atd...)
                //...
            }
            else
            {
                _eProfesion = EProfesion.None;
                ClearList(_cardList);
            }
        }
        private void ChoiseSmithing(bool value)
        {
            if(value)
            {
                _eProfesion = EProfesion.Smithing;
                AllOfList(_cardList, _defaultCardList);
                //...ošetření (pokud je u pece atd...)
                //...
            }
            else
            {
                _eProfesion = EProfesion.None;
                ClearList(_cardList);
            }
        }

        private void OnClick(int id)
        {
            _craftDataOperating.SelectedItem = Item.IdToItem(id);
            _craftDataOperating.ItemClicked();
        }
    }
}
