using System.Collections.Generic;
using System.Xml;
using Assets.Scripts.InventoryFolder;
using UnityEngine;

namespace Assets.Scripts.Extension
{
    public class XmlDatabaseReader
    {
        private TextAsset _textAsset;
        private readonly List<Dictionary<string, string>> _dbDictionaryList;
        private Dictionary<string, string> _dbDictionary;
        public XmlDatabaseReader(TextAsset textAsset)
        {
            _textAsset = textAsset;
            _dbDictionaryList = new List<Dictionary<string, string>>();
        }
        public void ReadItemsFromDatabase(string firstElement, params string[] otherElements)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(_textAsset.text);
            XmlNodeList itemList = xml.GetElementsByTagName(firstElement);
            foreach (XmlNode itemInfo in itemList)
            {
                XmlNodeList itemContentList = itemInfo.ChildNodes;
                _dbDictionary = new Dictionary<string, string>();   //ID : číslo
                foreach (XmlNode content in itemContentList)
                {
                    Comparing(content,otherElements);
                }
                _dbDictionaryList.Add(_dbDictionary);
                ItemDatabase.Database.Add(new Item(_dbDictionary));
            }
        }

        private void Comparing(XmlNode content ,params string[] otherElements)
        {
            foreach (string element in otherElements)
            {
                if (content.Name == element)
                {
                    _dbDictionary.Add(element, content.InnerText);
                   // Debug.Log(content.InnerText);
                }
            }           
        }

        public List<Dictionary<string, string>> GetDictionaryList()
        {
            return _dbDictionaryList;
        }
    }
}
