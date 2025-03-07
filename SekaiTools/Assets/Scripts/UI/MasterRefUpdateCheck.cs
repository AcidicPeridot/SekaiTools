using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    public class MasterRefUpdateCheck : MonoBehaviour
    {
        public List<MasterRefUpdateItem> masterRefUpdateItems;
        public UniversalGenerator universalGenerator;
        public Button btnUpdateAll;
        public MonoBehaviour updateOn;
        public Text txtUpdate;

        const int MAX_INTERVAL_DAYS = 1;

        public List<string> GetErrors()
        {
            List<string> errors = new List<string>();
            if (masterRefUpdateItems.Count <= 0) return errors;
            DateTime firstItemTime = File.GetLastWriteTime(masterRefUpdateItems[0].SavePath);
            DateTime oldestUpdateTime = firstItemTime;
            DateTime newestUpdateTime = firstItemTime;
            foreach (var masterRefUpdateItem in masterRefUpdateItems)
            {
                if (!File.Exists(masterRefUpdateItem.SavePath))
                {
                    errors.Add($"Data table {masterRefUpdateItem.masterName} not found");
                }
                else
                {
                    DateTime updateTime = File.GetLastWriteTime(masterRefUpdateItem.SavePath);
                    if (updateTime < oldestUpdateTime) oldestUpdateTime = updateTime;
                    if (updateTime > newestUpdateTime) newestUpdateTime = updateTime;
                }
            }

            if (newestUpdateTime.AddDays(-MAX_INTERVAL_DAYS) > oldestUpdateTime)
                errors.Add("The oldest update of a master table is too old, try updating all tables");

            return errors;
        }

        private void Awake()
        {
            if (btnUpdateAll)
                btnUpdateAll.onClick.AddListener(() => UpdateAll());
        }

        public void SetMasterRefUpdateItems(params IEnumerable<string>[] tableNames)
        {
            universalGenerator.ClearItems();
            HashSet<string> tableNameSet = new HashSet<string>();
            foreach (var nameSet in tableNames)
            {
                tableNameSet.UnionWith(nameSet);
            }

            List<string> tableNameList = new List<string>(tableNameSet);
            tableNameList.Sort();
            masterRefUpdateItems = new List<MasterRefUpdateItem>();
            universalGenerator.Generate(tableNameSet.Count,
                (gobj, id) =>
                {
                    MasterRefUpdateItem masterRefUpdateItem = gobj.GetComponent<MasterRefUpdateItem>();
                    masterRefUpdateItem.infoText.text = string.Empty;
                    masterRefUpdateItem.masterName = tableNameList[id];
                    masterRefUpdateItem.Refresh();
                    masterRefUpdateItems.Add(masterRefUpdateItem);
                });
        }

        public void UpdateAll()
        {
            StopAllCoroutines();
            StartCoroutine(IUpdateAll(updateOn));
        }

        IEnumerator IUpdateAll(MonoBehaviour updateOn)
        {
            if (btnUpdateAll) btnUpdateAll.interactable = false;
            if(txtUpdate) txtUpdate.text = "Update DYNAMIC";

            foreach (var masterRefUpdateItem in masterRefUpdateItems)
            {
                yield return masterRefUpdateItem.UpdateMasterOn(updateOn);
            }

            if (btnUpdateAll) btnUpdateAll.interactable = true;
            if(txtUpdate) txtUpdate.text = "All updates completed";
        }
    }
}