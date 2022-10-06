using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SekaiTools.UI.Radio;

namespace SekaiTools.UI.RadioInitialize
{
    public class GIP_RadioWelcome : MonoBehaviour
    {
        public InputField inputField_TipStayTime;

        public Radio_WelcomeLayer.Settings Settings
        {
            get
            {
                Radio_WelcomeLayer.Settings settings = new Radio_WelcomeLayer.Settings();
                settings.tipStayTime = float.Parse(inputField_TipStayTime.text);
                settings.tips = new List<string>()
                {
                    "���͵�Ļ \"/��� ������\" ���",
                    "����������пո���ɾȥ�ո���滻Ϊ�»���",
                    "��跶Χ������ϷProject Sekai�еĸ���",
                    "���͵�Ļ \"/�����б�\" �鿴��̨���и���",
                    "Ҳ����ʹ�ø�����������������ID���",
                    "�ڵ��ָ��֮�������ɫ���ƣ�����ѡ������汾",
                    "ʾ�� \"��� Forward an_khn\" ����ѡ���ӡ����Ϥ��ݳ���Forward"
                };
                return settings;
            }
        }
    }
}