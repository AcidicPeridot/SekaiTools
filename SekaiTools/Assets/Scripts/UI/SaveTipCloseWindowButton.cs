using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI
{
    public class SaveTipCloseWindowButton : MonoBehaviour
    {
        public Window window;
        Func <string> saveFilePathGetter;

        public Func<string> SaveFilePathGetter { get => saveFilePathGetter; set => saveFilePathGetter = value; }

        public void Initialize(Func<string> saveFilePathGetter)
        {
            this.SaveFilePathGetter = saveFilePathGetter;
        }

        public void Close()
        {
            string message;
            if(!File.Exists(saveFilePathGetter()))
            {
                message = "����û�б����ļ�";
            }
            else
            {
                message = $"�ϴα�����{File.GetLastWriteTime(saveFilePathGetter()):F}\nδ����ĸ��Ľ���ʧ";
            }

            WindowController.ShowCancelOK("ȷ��Ҫ�˳���", message,()=>window.Close());
        }
    }
}