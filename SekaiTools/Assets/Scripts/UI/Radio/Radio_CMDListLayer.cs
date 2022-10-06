using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.Radio
{
    public class Radio_CMDListLayer : MonoBehaviour
    {
        public Radio radio;
        [Header("Components")]
        public UniversalGenerator universalGenerator;
        public AutoScroll autoScroll;

        class CMDInfo
        {
            public string cmd;
            public string description;

            public CMDInfo(string cmd, string description)
            {
                this.cmd = cmd;
                this.description = description;
            }
        }

        /// <summary>
        /// �ڵ�̨��ʼ�������������ã�������������������ָ���б�
        /// </summary>
        public void Initialize()
        {
            List<CMDInfo> cMDInfos = new List<CMDInfo>();
            cMDInfos.Add(new CMDInfo(
                "/����", "�ӵ�ǰҳ�淵����ҳ"));
            cMDInfos.Add(new CMDInfo(
                "/��� ��������","�򲥷��б������ָ���ĸ�������֧����ϷProject Sekai�г��ֵĸ���"));
            cMDInfos.Add(new CMDInfo(
                "/�����б�", "�鿴�˵�̨�ĸ����б�"));
            cMDInfos.Add(new CMDInfo(
                "/�����б�", "�鿴��ǰ�����б�"));

            if (radio.serifQueryLayer.enabled)
                cMDInfos.Add(new CMDInfo(
                "/�Ի���ѯ ��ɫ1 ��ɫ2", "��ʾ ��ɫ1 �ᵽ ��ɫ2 �ĶԻ�"));

            universalGenerator.Generate(cMDInfos.Count,
                (gobj, id) =>
                {
                    ItemWithTitleAndContent itemWithTitleAndContent = gobj.GetComponent<ItemWithTitleAndContent>();
                    itemWithTitleAndContent.text_Title.text = cMDInfos[id].cmd;
                    itemWithTitleAndContent.text_Content.text = cMDInfos[id].description;
                });
        }

        public void Play(Action onComplete)
        {
            StartCoroutine(IPlay(onComplete));
        }

        IEnumerator IPlay(Action onComplete)
        {
            yield return 1;
            yield return autoScroll.IPlay(onComplete);
        }
    }
}