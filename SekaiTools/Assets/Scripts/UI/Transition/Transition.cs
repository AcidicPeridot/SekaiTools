﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.Transition
{
    public abstract class Transition : MonoBehaviour
    {
        public string itemName;
        public Sprite preview;
        public float transitionTime = 2;
        public RectTransform targetTransform;

        public abstract IEnumerator TransitionCoroutine(IEnumerator changeCoroutine);
        public abstract IEnumerator TransitionCoroutine(Action changeAction);
        public abstract TransitionYieldInstruction StartTransition(IEnumerator changeCoroutine);
        public abstract TransitionYieldInstruction StartTransition(Action changeAction);
        public virtual TransitionYieldInstruction StartTransition()
        {
            TransitionYieldInstruction transitionYieldInstruction = new TransitionYieldInstruction();
            StartTransition(() => transitionYieldInstruction._keepWaiting = false);
            return transitionYieldInstruction;
        }

        public TransitionYieldInstruction StartTransition(float holdTime)
        {
            return StartTransition(WaitCoroutine(holdTime));
        }

        public IEnumerator TransitionCoroutine(float holdTime)
        {
            yield return TransitionCoroutine(WaitCoroutine(holdTime));
        }

        IEnumerator WaitCoroutine(float holdTime)
        {
            yield return new WaitForSeconds(holdTime);
        }

        public abstract string SaveSettings();
        public abstract void LoadSettings(string serialisedSettings);

        public abstract ConfigUIItem[] configUIItems { get; }

        public abstract void Abort();

        public virtual string DefaultSerializedTransition => string.Empty;
    }

    /// <summary>
    /// 等待至changeCoroutine/changeAction执行
    /// </summary>
    public class TransitionYieldInstruction : CustomYieldInstruction
    {
        public override bool keepWaiting => _keepWaiting;
        public bool _keepWaiting = true;
    }
}