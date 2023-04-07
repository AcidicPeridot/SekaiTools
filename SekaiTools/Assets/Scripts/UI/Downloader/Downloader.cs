using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace SekaiTools.UI.Downloader
{
    public enum ExistingFileProcessingMode 
    { 
        /// <summary>
        /// ���������ļ�
        /// </summary>
        Override,
        /// <summary>
        /// ���������ļ�
        /// </summary>
        Pass,
        /// <summary>
        /// ���������ļ�hash����ͬ������
        /// </summary>
        CheckHash 
    }

    public class DownloadFileInfo
    {
        public string url;
        public string savePath;
        public string hash;
        public string cookie;

        public DownloadFileInfo(string url, string savePath, string hash = null, string cookie = null)
        {
            this.url = url;
            this.savePath = savePath;
            this.hash = hash;
            this.cookie = cookie;
        }
    }

    public enum DownloadResult
    {
        Null,
        Failure,
        Complete,
        PassExist,
        PassSameHash
    }

    public class DownloaderLogItem
    {
        public DateTime startTime;
        public DateTime endTime;
        public DownloadFileInfo downloadFileInfo;
        public DownloadResult downloadResult;
        public string error = null;

        public DownloaderLogItem(DateTime startTime, DownloadFileInfo downloadFileInfo)
        {
            this.startTime = startTime;
            this.downloadFileInfo = downloadFileInfo;
        }
    }

    public class DownloadResultCount
    {
        public int countFailure = 0;
        public int countComplete = 0;
        public int countPassExist = 0;
        public int countPassSameHash = 0;

        public int CountPass => countPassExist + countPassSameHash;
    }

    public class Downloader : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Text txtDownload;
        public Downloader_PerecntBar perecntBar;
        public MessageAreaTypeA messageArea;
        public List<Button> logViewButtons;
        public List<Button> logSaveButtons;
        [Header("Settings")]
        public float retryWaitTime = 3;
        public int retryTimes = 5;
        public ExistingFileProcessingMode existingFileProcessingMode = ExistingFileProcessingMode.Override;

        DownloadFileInfo[] downloadFiles;
        List<DownloaderLogItem> downloaderLog = new List<DownloaderLogItem>();
        bool isDone = false;
        bool disableLogView = false;

        public bool IsDone => isDone;
        public DownloaderLogItem[] DownloaderLog => downloaderLog.ToArray();
        public bool HasError
        {
            get
            {
                foreach (var downloaderLogItem in downloaderLog)
                {
                    if (downloaderLogItem.downloadResult == DownloadResult.Failure)
                        return true;
                }
                return false;
            }
        }

        public event Action OnComplete;

        public class Settings
        {
            public float retryWaitTime = 3;
            public int retryTimes = 5;
            public ExistingFileProcessingMode existingFileProcessingMode = ExistingFileProcessingMode.Override;
            public DownloadFileInfo[] downloadFiles;
            public bool disableLogView = false;
        }

        public void Initialize(Settings settings)
        {
            retryWaitTime = settings.retryWaitTime;
            retryTimes = settings.retryTimes;
            existingFileProcessingMode = settings.existingFileProcessingMode;
            downloadFiles = settings.downloadFiles;
            disableLogView = settings.disableLogView;

            if(settings.disableLogView)
            {
                foreach (var button in logViewButtons)
                {
                    button.interactable = false;
                }
            }
            foreach (var button in logSaveButtons)
            {
                button.interactable = false;
            }

            window.OnClose.AddListener(() =>
            {
                StopAllCoroutines();
                if (currentGetRequest != null && !currentGetRequest.isDone)
                {
                    currentGetRequest.Abort();
                    currentGetRequest.Dispose();
                }
                if (!string.IsNullOrEmpty(tempFilePath) && File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
            });
        }

        public void StartDownload()
        {
            StartCoroutine(IDownload());
        }

        string tempFilePath = null;
        UnityWebRequest currentGetRequest = null;
        IEnumerator IDownload()
        {
            RefreshTextDownload();
            for (int i = 0; i < downloadFiles.Length; i++)
            {
                DownloadFileInfo downloadFileInfo = downloadFiles[i];
                DownloaderLogItem downloaderLogItem = new DownloaderLogItem(DateTime.Now, downloadFileInfo);
                string fileName = Path.GetFileName(downloadFileInfo.savePath);
                yield return null;

                RefreshTextDownload();
                perecntBar.priority = (float)i / downloadFiles.Length;
                perecntBar.info = $"�������� {Path.GetFileName(downloadFileInfo.savePath)}";

                string dir = Path.GetDirectoryName(downloadFileInfo.savePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                //����ļ��Ƿ����
                switch (existingFileProcessingMode)
                {
                    case ExistingFileProcessingMode.Override:
                        break;
                    case ExistingFileProcessingMode.Pass:
                        if (File.Exists(downloadFileInfo.savePath))
                        {
                            messageArea.AddLine($"{fileName} �Ѵ��ڣ�����");
                            downloaderLogItem.endTime = DateTime.Now;
                            downloaderLogItem.downloadResult = DownloadResult.PassExist;
                            downloaderLog.Add(downloaderLogItem);
                            continue;
                        }
                        break;
                    case ExistingFileProcessingMode.CheckHash:
                        throw new System.NotImplementedException();
                        break;
                    default:
                        break;
                }

                if (string.IsNullOrEmpty(tempFilePath))
                    tempFilePath = Path.GetTempFileName();
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);

                //�����Դ���������
                for (int t = 0; t < retryTimes; t++)
                {
                    using (UnityWebRequest getRequest = UnityWebRequest.Get(downloadFileInfo.url))
                    {
                        getRequest.downloadHandler = new DownloadHandlerFile(tempFilePath, false);
                        if(!string.IsNullOrEmpty(downloadFileInfo.cookie))
                        {
                            getRequest.SetRequestHeader("cookie", downloadFileInfo.cookie);
                        }
                        currentGetRequest = getRequest;
                        getRequest.SendWebRequest();
                        messageArea.AddLine($"{fileName} ��ʼ����");
                        while (!getRequest.isDone)
                        {
                            yield return null;
                        }
                        string responseCode = getRequest.responseCode.ToString();
                        if (getRequest.error != null)
                        {
                            perecntBar.info = $"���ڵȴ�����({retryWaitTime}s) {fileName}";
                            messageArea.AddLine($"{fileName} ����ʧ�ܣ�{getRequest.error}");
                            if (t == retryTimes - 1)
                            {
                                messageArea.AddLine($"{fileName} ����������Դ���");
                                downloaderLogItem.error = getRequest.error;
                                downloaderLogItem.downloadResult = DownloadResult.Failure;
                                if (File.Exists(downloadFileInfo.savePath))
                                    File.Delete(downloadFileInfo.savePath);
                            }
                            yield return new WaitForSeconds(retryWaitTime);
                            perecntBar.info = $"��{t + 1}������ {fileName}";
                        }
                        else
                        {
                            if (File.Exists(downloadFileInfo.savePath))
                                File.Delete(downloadFileInfo.savePath);
                            string saveFolder = Path.GetDirectoryName(downloadFileInfo.savePath);
                            if (!Directory.Exists(saveFolder))
                                Directory.CreateDirectory(saveFolder);
                            File.Copy(tempFilePath, downloadFileInfo.savePath);
                            messageArea.AddLine($"{fileName} �������");
                            downloaderLogItem.downloadResult = DownloadResult.Complete;
                            currentGetRequest = null;
                            break;
                        }
                        currentGetRequest = null;
                    }
                }
                downloaderLogItem.endTime = DateTime.Now;
                downloaderLog.Add(downloaderLogItem);
            }
            RefreshTextDownload();
            perecntBar.priority = 1;
            perecntBar.info = "�����";
            OnComplete?.Invoke();
            if (!disableLogView)
            {
                foreach (var button in logSaveButtons)
                {
                    button.interactable = true;
                }
            }

            yield break;
        }

        private void RefreshTextDownload()
        {
            DownloadResultCount downloadResultCount = GetDownloadResultCount();
            txtDownload.text = $"���� �ɹ�{downloadResultCount.countComplete} ����{downloadResultCount.CountPass} ʧ��{downloadResultCount.countFailure} / {downloadFiles.Length}���ļ�";
        }

        public void ViewErrorLog()
        {
            WindowController.ShowLog("������־", GetLog(true));
        }

        public void ViewLog()
        {
            WindowController.ShowLog("������־", GetLog(true));
        }

        public void SaveLog()
        {
            SaveFileDialog saveFileDialog
                = FileDialogFactory.GetSaveFileDialog(FileDialogFactory.FILTER_TXT);
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK)
                return;

            File.WriteAllText(saveFileDialog.FileName, GetLog(false));
        }

        public string GetLog(bool onlyShowError)
        {
            List<string> outStrs = new List<string>();
            foreach (var logItem in downloaderLog)
            {
                if (onlyShowError&&logItem.downloadResult == DownloadResult.Complete)
                    continue;
                switch (logItem.downloadResult)
                {
                    case DownloadResult.Null:
                        break;
                    case DownloadResult.Failure:
                        outStrs.Add($"[{logItem.endTime:T}] ����ʧ�� {logItem.error}\n{logItem.downloadFileInfo.url}");
                        break;
                    case DownloadResult.Complete:
                        outStrs.Add($"[{logItem.endTime:T}] �������\n{logItem.downloadFileInfo.url}");
                        break;
                    case DownloadResult.PassExist:
                        outStrs.Add($"[{logItem.endTime:T}] �Ѵ��ڣ�����\n{logItem.downloadFileInfo.url}");
                        break;
                    case DownloadResult.PassSameHash:
                        outStrs.Add($"[{logItem.endTime:T}] hash��ͬ������\n{logItem.downloadFileInfo.url}");
                        break;
                    default:
                        break;
                }
            }
            return string.Join("\n", outStrs);
        }

        public void EnableLogView()
        {
            disableLogView = false;
            foreach (var button in logViewButtons)
            {
                button.interactable = true;
            }
            if(isDone)
            {
                foreach (var button in logSaveButtons)
                {
                    button.interactable = true;
                }
            }
        }

        public void DisableLogView()
        {
            disableLogView = true;
            foreach (var button in logViewButtons)
            {
                button.interactable = false;
            }
            if (isDone)
            {
                foreach (var button in logSaveButtons)
                {
                    button.interactable = false;
                }
            }
        }

        public DownloadResultCount GetDownloadResultCount()
        {
            DownloadResultCount downloadResultCount = new DownloadResultCount();
            foreach (var downloaderLogItem in downloaderLog)
            {
                switch (downloaderLogItem.downloadResult)
                {
                    case DownloadResult.Null:
                        break;
                    case DownloadResult.Failure:
                        downloadResultCount.countFailure++;
                        break;
                    case DownloadResult.Complete:
                        downloadResultCount.countComplete++;
                        break;
                    case DownloadResult.PassExist:
                        downloadResultCount.countPassExist++;
                        break;
                    case DownloadResult.PassSameHash:
                        downloadResultCount.countPassSameHash++;
                        break;
                    default:
                        break;
                }
            }
            return downloadResultCount;
        }
    }
}