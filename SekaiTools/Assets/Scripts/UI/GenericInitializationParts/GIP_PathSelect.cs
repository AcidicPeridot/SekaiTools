using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI.GenericInitializationParts
{
    public class GIP_PathSelect : MonoBehaviour,IGenericInitializationPart
    {
        public PathSelectItem[] pathSelectItems;

        public void Initialize()
        {
            foreach (var pathSelectItem in pathSelectItems)
            {
                pathSelectItem.ResetPath();
            }
        }

        public string CheckIfReady()
        {
            List<string> errors = new List<string>();
            foreach (var pathSelectItem in pathSelectItems)
            {
                if (string.IsNullOrEmpty(pathSelectItem.SelectedPath))
                    errors.Add("����δ���õ�Ŀ¼");
                else if(!(pathSelectItem is SaveFileSelectItem)&&
                    !(pathSelectItem is SLFileSelectItem &&!((SLFileSelectItem)pathSelectItem).ifNewFile))
                {
                    if (pathSelectItem is FolderSelectItem)
                    {
                        if (!Directory.Exists(pathSelectItem.SelectedPath))
                            errors.Add($"�ļ��� {pathSelectItem.SelectedPath} ������");
                    }
                    else if (!File.Exists(pathSelectItem.SelectedPath))
                        errors.Add($"�ļ� {pathSelectItem.SelectedPath} ������");
                }
            }

            return GenericInitializationCheck.GetErrorString("Ŀ¼����", errors);
        }
    }
}