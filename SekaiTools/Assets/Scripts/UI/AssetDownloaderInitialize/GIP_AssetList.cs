using SekaiTools.UI;
using SekaiTools.UI.GenericInitializationParts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
}

namespace SekaiTools.UI.AssetDownloaderInitialize
{

    public class GIP_AssetList : MonoBehaviour, IGenericInitializationPart
    {
        public InputField if_URLHead;
        public AssetListGetter assetListGetter;
        public LoadFileSelectItem lfsi_Cookie;

        public string GetURLHead()
        {
            string urlHead = if_URLHead.text;
            if (!urlHead.EndsWith("/"))
                urlHead += '/';
            return urlHead;
        }

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(if_URLHead.text))
                errors.Add("URL��ʼ�ַ���Ϊ��");
            if (string.IsNullOrEmpty(assetListGetter.lfsi_AssetList.SelectedPath))
                errors.Add("δѡ�����ݱ��ļ�");
            if (string.IsNullOrEmpty(lfsi_Cookie.SelectedPath))
                errors.Add("δѡ��Cookie�ļ�");
            return GenericInitializationCheck.GetErrorString("URL�����ݱ����", errors);
        }
    }
}