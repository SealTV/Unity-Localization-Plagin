using UnityEngine;
using UnityEngine.UI;

namespace Localization
{
    [ExecuteInEditMode]
    [RequireComponent(typeof (Text))]
    public class LocalizetText : MonoBehaviour
    {
        public int _fileEndex;
        public int _textEndex;

        private Text _text;
        public Text Text
        {
            get { return _text ?? (_text = GetComponent<Text>()); }
        }

        private void Start()
        {
            Text.text = LocalizationManager.Instance.GetLocilizedText(_fileEndex, _textEndex);
        }
    }
}