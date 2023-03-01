using SekaiTools.UI.L2DModelSelect;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.GenericInitializationParts
{
    public class GIP_ModelSelector : MonoBehaviour, IGenericInitializationPart
    {
        public L2DModelSelectArea modelSelectArea;

        public Dictionary<string, SelectedModelInfo> KeyValuePairs => modelSelectArea.KeyValuePairs;

        public void Initialize(L2DModelSelectArea_ItemSettings[] settings) => modelSelectArea.Initialize(settings);
        public void Clear() => modelSelectArea.Clear();

        public string CheckIfReady()
        {
            L2DModelSelectArea.StatusEnum status = modelSelectArea.Status;
            if (status == L2DModelSelectArea.StatusEnum.MissingModel)
                return GenericInitializationCheck.GetErrorString("ģ�ʹ���", "����δ���õ�ģ��");
            else if(status == L2DModelSelectArea.StatusEnum.MissingAnimationSet)
                return GenericInitializationCheck.GetErrorString("ģ�ʹ���", "����δ���ö�������ģ��");
            return null;
        }
    }
}