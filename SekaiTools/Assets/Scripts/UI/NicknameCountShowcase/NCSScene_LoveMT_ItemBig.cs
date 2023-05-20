﻿using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public class NCSScene_LoveMT_ItemBig : NCSScene_CountMT_Item
    {
        [Header("Components")]
        public Text txtCount;
        public Text txtLabel;

        public override void SetData(int characterId, int count, int talkerId)
        {
            base.SetData(characterId, count, talkerId);
            txtCount.text = $"{count} 次";
            txtLabel.text = $"共被提及";
        }
    }
}