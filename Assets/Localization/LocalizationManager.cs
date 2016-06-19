using System.Collections.Generic;
using System.Linq;
using Localization.Data;
using UnityEngine;

namespace Localization
{
    [ExecuteInEditMode]
    public class LocalizationManager : MonoBehaviour
    {
        private static LocalizationManager _instance;
        private static readonly object _lock = new object();

        [SerializeField]
        private Languages _currentLang;

        [HideInInspector]
        public List<LocalizationObject> LocalizationObjets = new List<LocalizationObject>();

        public static LocalizationManager Instance
        {
            get
            {
                lock ( _lock )
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<LocalizationManager>();

                        if (FindObjectsOfType<LocalizationManager>().Length > 1)
                        {
                            _instance.LoadData();
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<LocalizationManager>();
                            singleton.name = typeof (LocalizationManager).ToString();

                            DontDestroyOnLoad(singleton);
                        }
                    }

                    _instance.LoadData();
                    return _instance;
                }
            }
        }

        public void LoadData()
        {
            LocalizationObjets.Clear();
            string path = "Localization";
            TextAsset[] texts = Resources.LoadAll<TextAsset>(path);
            foreach ( TextAsset textAsset in texts )
            {
                LocalizationObjets.Add(new LocalizationObject(textAsset.name, textAsset.text));
            }
        }

        public string GetLocilizedText(int fileIndex, int textIndex)
        {
            return LocalizationObjets[fileIndex].LocalizationItems[textIndex].Values[_currentLang];
        }
    }
} 