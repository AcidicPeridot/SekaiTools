using SekaiTools.UI.GenericInitializationParts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.AssetDownloaderInitialize
{
    public class GIP_AssetListSettings : MonoBehaviour, IGenericInitializationPart
    {
        public Toggle toggle_StartWith;
        public InputField if_StartWith;

        public bool UseStartWith => toggle_StartWith.isOn;

        public string GetStartWithString()
        {
            return if_StartWith.text;
        }

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            if (UseStartWith && string.IsNullOrEmpty(GetStartWithString()))
                errors.Add("Empty Catalog start string");
            return GenericInitializationCheck.GetErrorString("Asset Filter setting error", errors);
        }
    }
}