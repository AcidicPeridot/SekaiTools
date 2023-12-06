using System.Collections;
using UnityEngine;

namespace SekaiTools.UI
{

    /// <summary>
    /// ������ʾ�����Ϣ��UI���
    /// </summary>
    public class MobInfoViewer : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public UniversalGenerator universalGenerator;

        MobInfoCounter mobInfoCounter;
        public MobInfoCounter MobInfoCounter => mobInfoCounter;

        public void Refresh()
        {
            universalGenerator.Generate(mobInfoCounter.MobInfos.Count, (gobj, id) =>
            {
                gobj.GetComponent<MobInfoViewer_Item>().SetData(this, mobInfoCounter.MobInfos[id]);
            });
        }
    }
}
