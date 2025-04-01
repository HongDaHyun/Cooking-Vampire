using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace Cooking
{
    public class GameManager_Cooking : Singleton<GameManager_Cooking>
    {
        [Title("플레이어 스탯")]
        public Chef chef;
        public ChefStat chefStat;
    }

    [Serializable]
    public class ChefStat
    {
        [Title("레벨")]
        public int staminaLv = 1;
        public int STAMINA
        {
            get { return staminaLv * 3; }
        }
    }

    public enum StatID_Chef { Stamina }
}
