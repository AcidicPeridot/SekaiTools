using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;

namespace SekaiTools.UI
{
    public class SaveFileSelectItem : PathSelectItem
    {
        public string fileFilter = "All(*.*) | *.*";

        public override void SelectPath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "�����ļ�";
            saveFileDialog.Filter = fileFilter;
            saveFileDialog.RestoreDirectory = true;
            if (!string.IsNullOrEmpty(SelectedPath)) saveFileDialog.FileName = SelectedPath;

             DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;

            pathInputField.text = saveFileDialog.FileName;

            onPathChange.Invoke(SelectedPath);
        }
    }
}