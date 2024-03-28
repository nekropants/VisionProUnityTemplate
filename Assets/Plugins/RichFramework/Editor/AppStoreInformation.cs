using System;
using UnityEngine;

namespace Plugins.RichFramework.Editor
{
    [Serializable]
    public class AppStoreInformation
    {
        [SerializeField] private string _description;
        [SerializeField] private string _privacyPolicy;
    }
}