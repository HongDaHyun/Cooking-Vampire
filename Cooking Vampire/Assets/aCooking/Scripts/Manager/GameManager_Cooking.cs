using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace Cooking
{
    public class GameManager_Cooking : Singleton<GameManager_Cooking>
    {
        [Title("�÷��̾� ����")]
        public Chef chef;
        public ChefStat chefStat;
    }

    [Serializable]
    public class ChefStat
    {
        [Title("����")]
        public int staminaLv = 1;
        public int STAMINA
        {
            get { return staminaLv * 3; }
        }
    }

    public enum StatID_Chef { Stamina }
}
