using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Windows.Forms;
using SekaiTools.UI.GenericInitializationParts;

namespace SekaiTools.UI.SVDownloaders
{
    public class GIP_SVMusic : MonoBehaviour, IGenericInitializationPart
    {
        public Toggle[] toggleFormats = new Toggle[2];
        public FolderSelectItem saveFolder;
        string[] formats = { ".mp3", ".flac" };

        public string format
        {
            get
            {
                for (int i = 0; i < toggleFormats.Length; i++)
                {
                    if (toggleFormats[i].isOn)
                        return formats[i];
                }
                return formats[0];
            }
        }

        public void Initialize()
        {
            saveFolder.defaultPath = $"{EnvPath.Assets}/music/long";
            saveFolder.ResetPath();
        }

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(saveFolder.SelectedPath))
                errors.Add("��Ч�ı���Ŀ¼");
            return GenericInitializationCheck.GetErrorString("�������ô���",errors);
        }
    }
}