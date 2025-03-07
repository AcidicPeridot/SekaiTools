using SekaiTools.DecompiledClass;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class AssetListGetter : MonoBehaviour
    {
        public LoadFileSelectItem lfsi_AssetList;
        public Dropdown dd_AssetListType;

        private void Awake()
        {
            dd_AssetListType.options = new List<Dropdown.OptionData>()
            {
                new Dropdown.OptionData("Encrypted MessagePack Data"),
                new Dropdown.OptionData("Decrypted MessagePack Data"),
                new Dropdown.OptionData("JSON directly converted by MessagePack"),
                new Dropdown.OptionData("Directly readable JSON")
            };
        }

        public BundleRoot GetBundleRoot()
        {
            switch ((AssetListType)dd_AssetListType.value)
            {
                case AssetListType.Undecrypted:
                    WindowController.windowController.SendMessage("Error", "This function is not yet complete");
                    break;
                case AssetListType.MessagePack:
                    byte[] msgPack = File.ReadAllBytes(lfsi_AssetList.SelectedPath);
                    string json = MessagePackConverter.ToJSONAssetList(
                                                msgPack);
                    return JsonUtility.FromJson<BundleRoot>(
                        json);
                case AssetListType.RawJSON:
                    return JsonHelper.getJsonArray<BundleRoot>(
                        MessagePackConverter.ModifyAssetListJSON(
                            File.ReadAllText(lfsi_AssetList.SelectedPath)))[0];
                case AssetListType.JSON:
                    return JsonHelper.getJsonArray<BundleRoot>(
                            File.ReadAllText(lfsi_AssetList.SelectedPath))[0];
                default:
                    break;
            }
            return null;
        }
    }
}
