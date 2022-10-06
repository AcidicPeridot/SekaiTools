using SekaiTools.UI.Downloader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.GenericInitializationParts
{
    public class GIP_DownloaderBase : MonoBehaviour , IGenericInitializationPart
    {
        public InputField retryWaitTimeInput;
        public InputField retryTimesInput;
        public Toggle[] existingFileProcessingModeToggles;

        public float retryWaitTime => float.Parse(retryWaitTimeInput.text);
        public int retryTimes => int.Parse(retryTimesInput.text);
        public ExistingFileProcessingMode existingFileProcessingMode
        {
            get
            {
                for (int i = 0; i < existingFileProcessingModeToggles.Length; i++)
                {
                    if (existingFileProcessingModeToggles[i].isOn)
                        return (ExistingFileProcessingMode)i;
                }
                return ExistingFileProcessingMode.Override;
            }
        }

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            try 
            { 
                if(retryWaitTime<=0)
                    errors.Add("�ȴ�ʱ�䲻��Ϊ������0");
            }
            catch { errors.Add("�ȴ�ʱ�������ʽ����ȷ"); }

            try 
            { 
                if(retryTimes<0)
                    errors.Add("���Դ�������Ϊ����"); 
            }
            catch { errors.Add("���Դ��������ʽ����ȷ"); }

            return GenericInitializationCheck.GetErrorString("�������ô���", errors);
        }
    }
}