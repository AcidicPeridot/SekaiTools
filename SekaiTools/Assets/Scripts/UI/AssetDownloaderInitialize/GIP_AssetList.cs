using SekaiTools.DecompiledClass;
using SekaiTools.UI;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.AssetDownloaderInitialize
{
    public enum AssetListType { Undecrypted, MessagePack, RawJSON, JSON }

    public class GIP_AssetList : MonoBehaviour, IGenericInitializationPart
    {
        public InputField if_URLHead; 
        public LoadFileSelectItem lfsi_AssetList;
        public Dropdown dd_AssetListType;

        public void Initialize()
        {
            //��ö��AssetListTypeһһ��Ӧ
            dd_AssetListType.options = new List<Dropdown.OptionData>()
            {
                new Dropdown.OptionData("δ���ܵ�MessagePackͨ��"),
                new Dropdown.OptionData("���ܵ�MessagePackͨ��"),
                new Dropdown.OptionData("MessagePackֱ��ת���ɵ�JSON,δ������"),
                new Dropdown.OptionData("��ֱ�Ӷ�ȡ��JSON")
            };
        }

        public string GetURLHead()
        {
            string urlHead = if_URLHead.text;
            if (!urlHead.EndsWith("/"))
                urlHead += '/';
            return urlHead;
        }

        public BundleRoot GetBundleRoot()
        {
            switch ((AssetListType)dd_AssetListType.value)
            {
                case AssetListType.Undecrypted:
                    WindowController.windowController.SendMessage("����", "�˹���ģ�黹δ���");
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

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(if_URLHead.text))
                errors.Add("URL��ʼ�ַ���Ϊ��");
            if (string.IsNullOrEmpty(lfsi_AssetList.SelectedPath))
                errors.Add("δѡ�����ݱ��ļ�");
            return GenericInitializationCheck.GetErrorString("URL�����ݱ����", errors);
        }
    }
}