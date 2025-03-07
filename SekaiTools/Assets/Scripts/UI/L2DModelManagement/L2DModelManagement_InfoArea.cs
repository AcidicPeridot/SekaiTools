﻿using UnityEngine;
using UnityEngine.UI;
using SekaiTools.Live2D;

namespace SekaiTools.UI.L2DModelManagement
{
    public class L2DModelManagement_InfoArea : MonoBehaviour
    {
        public L2DModelManagement l2DModelManagement;
        [Header("Components")]
        public Button btnDelete;
        public Text txtName;
        public Text txtInfo;
        public Image imgPreview;

        public void Refresh()
        {
            ModelInfo modelInfo = l2DModelManagement.CurrentModelInfo;
            if(modelInfo == null)
            {
                SetDataNull();
                return;
            }
            txtName.text = modelInfo.modelName;
            txtInfo.text = modelInfo.ifInbuilt ? "Built-in model" : $"Local model, from{modelInfo.modelPath}";
            imgPreview.sprite = L2DModelLoader.GetPreview(modelInfo.modelName);

            if (btnDelete != null)
            {
                if (modelInfo.ifInbuilt)
                {
                    btnDelete.interactable = false;
                }
                else
                {
                    btnDelete.interactable = true;
                }
            }
        }

        public void SetDataNull()
        {
            txtName.text = "Please select a model";
            txtInfo.text = string.Empty;
            imgPreview.sprite = L2DModelLoader.GetPreview(null);
            if (btnDelete != null)
                btnDelete.interactable = false;
        }
    }
}