using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.GenericInitializationParts
{
    public interface IGenericInitializationPart
    {
        /// <summary>
        /// ������ʱ����null,���򷵻ض����������
        /// �����������Ϊ���¸�ʽ
        /// �������ͣ�
        /// ����1
        /// ����2
        /// ...
        /// </summary>
        /// <returns></returns>
        string CheckIfReady();
    }
}