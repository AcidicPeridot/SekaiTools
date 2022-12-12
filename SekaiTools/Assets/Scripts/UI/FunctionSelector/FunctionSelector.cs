using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.FunctionSelector
{
    public class FunctionSelector : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public Button[] buttons;

        public void Initialize(params Action[] functions)
        {
            if(buttons.Length != functions.Length)
            {
                Debug.LogError("��ť���빦������ƥ��");
            }
            for (int i = 0; i < functions.Length; i++)
            {
                int id = i;
                buttons[id].onClick.AddListener(() =>
                    { window.Close(); });
                if(functions[id]!=null)
                {
                    buttons[id].onClick.AddListener(() =>
                        { functions[id](); });
                }
            }
        }
    }
}