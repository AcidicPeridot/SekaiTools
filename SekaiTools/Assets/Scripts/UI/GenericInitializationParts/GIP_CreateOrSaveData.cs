using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.GenericInitializationParts
{
    public abstract class GIP_CreateOrSaveData : MonoBehaviour, IGenericInitializationPart
    {
        [Header("Components")]
        public SaveFileSelectItem file_SaveData;
        public LoadFileSelectItem file_LoadData;

        bool ifNewFile = true;

        public string SelectedDataPath => ifNewFile ? file_SaveData.SelectedPath : file_LoadData.SelectedPath;
        public bool IfNewFile => ifNewFile;

        public virtual string CheckIfReady()
        {
            return GenericInitializationCheck.GetErrorString("Ŀ¼����", GetErrorList());
        }

        protected virtual List<string> GetErrorList()
        {
            List<string> errorList = new List<string>();
            if (ifNewFile && string.IsNullOrEmpty(file_SaveData.SelectedPath))
            {
                errorList.Add("��Ч��Ŀ¼");
            }
            else if (!ifNewFile)
            {
                if (string.IsNullOrEmpty(file_LoadData.SelectedPath))
                    errorList.Add("��Ч��Ŀ¼");
                if (!File.Exists(file_LoadData.SelectedPath))
                    errorList.Add("�ļ�������");
            }
            return errorList;
        }

        public void SwitchMode_Create() => ifNewFile = true;
        public void SwitchMode_Load() => ifNewFile = false;
    }
}