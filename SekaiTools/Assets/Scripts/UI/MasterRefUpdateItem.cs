using SekaiTools.SekaiViewerInterface;
using SekaiTools.SekaiViewerInterface.Utils;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class MasterRefUpdateItem : MonoBehaviour
    {
        public string masterName;
        public ServerRegion serverRegion = ServerRegion.jp;
        public Text nameText;
        public Text lastUpdateTimeText;
        public Text infoText;
        public Button updateButton;


        public string TableName
        {
            get => nameText.text;
            set
            {
                masterName = value;
                infoText.text = string.Empty;
                Refresh();
            }
        }

        public event Action OnTableUpdated;

        public string SavePath => Path.Combine(EnvPath.Sekai_master_db_diff[serverRegion], masterName + ".json");
        public string Url => $"{SekaiViewerInterface.Utils.Url.MasterUrl[SekaiViewer.masterSever, serverRegion]}/{masterName}.json";

        private void Awake()
        {
            infoText.text = string.Empty;
            if (!string.IsNullOrEmpty(masterName))
                Refresh();
        }

        public void Refresh()
        {
            string masterName = this.masterName;
            if(serverRegion!=ServerRegion.jp)
            {
                switch (serverRegion)
                {
                    case ServerRegion.jp:
                        break;
                    case ServerRegion.tw:
                        masterName += "�����У�";
                        break;
                    case ServerRegion.cn:
                        masterName += "�����У�";
                        break;
                    case ServerRegion.en:
                        masterName += "��Ӣ�ģ�";
                        break;
                    case ServerRegion.kr:
                        masterName += "�����ģ�";
                        break;
                    default:
                        break;
                }
            }
            nameText.text = masterName;
            lastUpdateTimeText.text =
                File.Exists(SavePath) ?
                "�ϴθ��� " + File.GetLastWriteTime(SavePath).ToString() :
                "���ݱ����ڣ���������ݱ�";
        }

        public void UpdateMaster()
        {
            StartCoroutine(IUpdateMaster());
        }

        public Coroutine UpdateMasterOn(MonoBehaviour updateOn = null)
        {
            if(updateOn != null)
                return updateOn.StartCoroutine(IUpdateMaster());
            else
                return StartCoroutine(IUpdateMaster());
        }

        private void OnDestroy()
        {
            if (string.IsNullOrEmpty(tempFilePath)
                && File.Exists(tempFilePath))
                File.Delete(tempFilePath);
        }

        string tempFilePath = null;
        IEnumerator IUpdateMaster()
        {
            updateButton.interactable = false;

            if (string.IsNullOrEmpty(tempFilePath)
                && File.Exists(tempFilePath))
                File.Delete(tempFilePath);
            tempFilePath = Path.GetTempFileName();
            using (UnityWebRequest getRequest = UnityWebRequest.Get(Url))
            {
                getRequest.downloadHandler = new DownloadHandlerFile(tempFilePath, false);
                getRequest.SendWebRequest();
                while (!getRequest.isDone)
                {
                    yield return null;
                }
                if (getRequest.error != null)
                {
                    infoText.text = getRequest.error;
                }
                else
                {
                    if (File.Exists(SavePath))
                        File.Delete(SavePath);
                    string path = Path.GetDirectoryName(SavePath);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    File.Copy(tempFilePath, SavePath);
                    infoText.text = "�������";
                    OnTableUpdated?.Invoke();
                }
            }

            Refresh();
            updateButton.interactable = true;
        }
    }
}