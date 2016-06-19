using System;
using System.Collections.Generic;
using System.Linq;

namespace Localization.Data
{
    [Serializable]
    public class LocalizationItem
    {
        public string Key;
        public readonly Dictionary<Languages, string> Values;

        public LocalizationItem()
        {
            Key = string.Empty;
            Values = new Dictionary<Languages, string>();
        }

        public LocalizationItem(string[] localizationOjects)
        {
            Key = localizationOjects[0];
            Values = new Dictionary<Languages, string>();

            Languages[] values = (Languages[])Enum.GetValues(typeof (Languages));
            for ( int i = 0; i < values.Length; i++ )
            {
                Values.Add(values[i], localizationOjects[i + 1]);
            }
        }

        public override string ToString()
        {
            return string.Format("{0};{1};", Key, string.Join(";", Values.Select(v => v.Value.ToString()).ToArray()));
        }
    }
}
