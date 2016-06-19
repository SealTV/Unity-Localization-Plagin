using System.Linq;
using Localization;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof (LocalizetText))]
[CanEditMultipleObjects]
public class LocalizetTextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LocalizetText localizetText = (LocalizetText) target;

        localizetText._fileEndex = EditorGUILayout.Popup(localizetText._fileEndex,
            LocalizationManager.Instance.LocalizationObjets.Select(o => o.Name).ToArray(),
            GUILayout.ExpandWidth(false));
        if (localizetText._textEndex >=
            LocalizationManager.Instance.LocalizationObjets[localizetText._fileEndex].LocalizationItems.Count)
        {
            localizetText._textEndex =
                LocalizationManager.Instance.LocalizationObjets[localizetText._fileEndex].LocalizationItems.Count - 1;
        }
        localizetText._textEndex = EditorGUILayout.Popup(localizetText._textEndex,
            LocalizationManager.Instance.LocalizationObjets[localizetText._fileEndex].LocalizationItems.Select(
                i => i.Key).ToArray(),
            GUILayout.ExpandWidth(false));

        localizetText.Text.text = LocalizationManager.Instance.GetLocilizedText(localizetText._fileEndex,
            localizetText._textEndex);
    }
}