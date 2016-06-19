using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Localization.Data
{
    [Serializable]
    public class LocalizationObject
    {
        public readonly string Name;
        public readonly List<LocalizationItem> LocalizationItems;

        public LocalizationObject(string name)
        {
            Name = name;
            LocalizationItems = new List<LocalizationItem>();
        }

        public LocalizationObject(string name, string content)
        {
            Name = name;
            var strings = content.Split('\n');
            var k = strings[0].Split(';');
            LocalizationItems = new List<LocalizationItem>();

            for ( int i = 1; i < strings.Length; i++ )
            {
                var localizationOject = strings[i];
                if (string.IsNullOrEmpty(localizationOject))
                    continue;

                LocalizationItems.Add(new LocalizationItem(localizationOject.Split(';')));
            }
        }

        public override string ToString()
        {
            return string.Format("Key;{0}\n{1}",
                string.Join(";", Enum.GetNames(typeof(Languages))),
                string.Join("\n", LocalizationItems.Select(i => i.ToString()).ToArray()));
        }
    }
}
