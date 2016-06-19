using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Localization.Data;
using UnityEditor;
using UnityEngine;

namespace Localization.Editor
{
    public class LocalizationManagerEditorWindow : EditorWindow
    {
        private readonly List<LocalizationObject> _localizationObjects = new List<LocalizationObject>();
        private string[] _fileNames = new string[0];

        private Vector2 _scrollScopePosition;
        private int _lastSelectedLocalizationFile;
        private Vector2 _scrollPosition;
        private LocalizationItem _newItem = new LocalizationItem();

        [MenuItem("Tools/Localization")]
        static void Init()
        {
            LocalizationManagerEditorWindow window = (LocalizationManagerEditorWindow)
                GetWindow(typeof(LocalizationManagerEditorWindow), true, "Licalization Manager");
            window.Refresh();
            // Get existing open window or if none, make a new one:
            window.Show();
        }

        private void Refresh()
        {
            LoadFileContent();
            _fileNames = _localizationObjects.Select(o => o.Name).ToArray();
            _lastSelectedLocalizationFile = 0;
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            using (
                var scrollViewScope = new EditorGUILayout.ScrollViewScope(_scrollScopePosition,
                    GUILayout.ExpandHeight(true)) )
            {
                _scrollScopePosition = scrollViewScope.scrollPosition;

                _lastSelectedLocalizationFile = GUILayout.SelectionGrid(_lastSelectedLocalizationFile, _fileNames.ToArray(), 1, GUILayout.ExpandWidth(false));
                GUILayout.Label("Load or create localization file.", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Refresh", GUILayout.ExpandWidth(false)))
                {
                    Refresh();
                }
                if (GUILayout.Button("Add new file", GUILayout.ExpandWidth(false)))
                {
                    AddNewFile();
                    Refresh();
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginVertical();
           
            try
            {
                Body();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Refresh();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            if(GUILayout.Button("Save", GUILayout.ExpandWidth(false)))
            {
                Save();
            }
        }

        private void Body()
        {
            GUILayout.BeginHorizontal();

            var items = _localizationObjects[_lastSelectedLocalizationFile].LocalizationItems;

            GUILayout.Label("key", GUILayout.Width(100));
            foreach(var lang in Enum.GetNames(typeof(Languages)))
            {
                GUILayout.Label(lang, GUILayout.Width(100));
            }

            GUILayout.EndHorizontal();

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandHeight(true));
            GUILayout.BeginVertical();

            LocalizationItem itemToRemove = null;
            foreach ( LocalizationItem item in items )
            {
                GUILayout.BeginHorizontal();
                DrawLocalizationItem(item);

                if (GUILayout.Button("-", GUILayout.ExpandWidth(false)))
                {
                    itemToRemove = item;
                }
                GUILayout.EndHorizontal();
            }

            if (itemToRemove != null)
            {
                items.Remove(itemToRemove);
            }

            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            _newItem = DrawLocalizationItem(_newItem);
            if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
            {
                if (string.IsNullOrEmpty(_newItem.Key))
                {
                    _newItem.Key = items.Count.ToString();
                }

                items.Add(_newItem);
                _newItem = new LocalizationItem();
            }
            GUILayout.EndHorizontal();
        }

        private LocalizationItem DrawLocalizationItem(LocalizationItem localizationItem)
        {
            localizationItem.Key = GUILayout.TextField(localizationItem.Key, GUILayout.Width(100));
            foreach( var lang in (Languages[])Enum.GetValues(typeof (Languages)))
            {
                if (!localizationItem.Values.ContainsKey(lang))
                {
                    localizationItem.Values.Add(lang, string.Empty);
                }

                localizationItem.Values[lang] = GUILayout.TextField(localizationItem.Values[lang], GUILayout.Width(100));
            }

            return localizationItem;
        }

        private void LoadFileContent()
        {
            _localizationObjects.Clear();
            foreach ( var file in Directory.GetFiles("Assets/Resources/Localization/", "*.csv") )
            {
                using ( var stream = File.Open(file, FileMode.Open, FileAccess.Read) )
                using ( var reader = new StreamReader(stream) )
                {
                    _localizationObjects.Add(new LocalizationObject(stream.Name, reader.ReadToEnd()));
                }
            }
        }

        private void AddNewFile()
        {
            string path = EditorUtility.SaveFilePanel("Save scene", "Assets/Resources/Localization", "Localization", "csv");
        }

        private void Save()
        {
            foreach ( var localizationObject in _localizationObjects )
            {
                File.WriteAllText(localizationObject.Name, localizationObject.ToString(), Encoding.Unicode);
            }
        }
    }
}
