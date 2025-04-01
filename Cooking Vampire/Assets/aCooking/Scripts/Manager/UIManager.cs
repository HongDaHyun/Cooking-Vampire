using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace Cooking
{
    public class UIManager : Singleton<UIManager>
    {
        [Title("HUD")]
        public Slider staminaSlider;

        GameManager_Cooking gm;

        private void Start()
        {
            gm = GameManager_Cooking.Instance;
        }

        public void Update()
        {
            // Stamina_Slider
            float curStamina = gm.chef.chefMove.curStamina;
            float maxStamina = gm.chefStat.STAMINA;

            float targetStamina = curStamina / maxStamina;
            staminaSlider.value = targetStamina;
            staminaSlider.value = Mathf.Lerp(staminaSlider.value, targetStamina, Time.deltaTime * 5f);
        }
    }
}
