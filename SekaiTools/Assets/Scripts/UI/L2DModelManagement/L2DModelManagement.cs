﻿using SekaiTools.Live2D;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using System.IO;
using System;

namespace SekaiTools.UI.L2DModelManagement
{
    public class L2DModelManagement : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public L2DModelManagement_InfoArea infoArea;
        public ButtonGenerator2D buttonGenerator2D;
        public Button downloadModelButton;
        public Button loadModelButton;
        public Button loadModelsButton;
        [Header("Prefab")]
        public Window modelDownloaderPrefab;

        OpenFileDialog fileDialog = new OpenFileDialog();
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

        ModelInfo currentModelInfo;
        public ModelInfo CurrentModelInfo => currentModelInfo;

        protected event Action<ModelInfo> onChangeSelection = null;

        private void Awake()
        {
            fileDialog.Title = "Select Model";
            fileDialog.Filter = "Models (*.model3.json)|*.model3.json|Motions (*.motion3.json)|*.motion3.json|Others (*.*)|*.*";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;

            folderBrowserDialog.Description = "Select Model Folder";

            GenerateItem();
        }

        void GenerateItem()
        {
            string[] modelList = L2DModelLoader.ModelList;
            buttonGenerator2D.Generate(modelList.Length,
                (btn, id) =>
                {
                    L2DModelManagement_Item l2DModelManagement_Item = btn.GetComponent<L2DModelManagement_Item>();
                    l2DModelManagement_Item.Initialize(modelList[id]);
                },
                (id) =>
                {
                    currentModelInfo = L2DModelLoader.GetModelInfo(modelList[id]);
                    if(onChangeSelection != null) onChangeSelection(currentModelInfo);
                    infoArea.Refresh();
                });
        }

        public void LoadModel()
        {
            DialogResult dialogResult = fileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            string path = fileDialog.FileName;

            L2DModelLoader.AddLocalModel(path);
            Refresh();
        }

        public void Refresh()
        {
            buttonGenerator2D.ClearButtons();
            GenerateItem();
        }

        public void LoadModels()
        {
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult != DialogResult.OK) return;
            string selectedPath = folderBrowserDialog.SelectedPath;

            L2DModelLoader.AddLocalModels(selectedPath);

            Refresh();
        }

        public void DeleteModel()
        {
            WindowController.ShowCancelOK("Caution", $"Are you sure you want to delete the model {CurrentModelInfo.modelName}?",
                () =>
                {
                    L2DModelLoader.RemoveModel(CurrentModelInfo.modelName);
                    Refresh();
                });
            currentModelInfo = null;
            infoArea.Refresh();
        }

        public void DownloadModel()
        {
            L2DModelDownloader.L2DModelDownloader l2DModelDownloader = window.OpenWindow<L2DModelDownloader.L2DModelDownloader>(modelDownloaderPrefab);
            l2DModelDownloader.window.OnClose.AddListener(() =>
            {
                Refresh();
            });
        }
    }
}