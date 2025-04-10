using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IObj_Slider : IObj
{
    protected bool isDoing;

    // Doing슬라이더
    public Transform sliderObj;
    protected Slider slider;
    private Image fillImg;

    protected override void Awake()
    {
        base.Awake();
        slider = sliderObj.GetChild(0).GetComponent<Slider>();
        fillImg = sliderObj.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>();
    }

    protected virtual IEnumerator SliderRoutine()
    {
        sliderObj.gameObject.SetActive(true);
        chef.chefMove.DoingAnim(true);

        while (chef.chefMove.isInteract)
        {
            slider.value += Time.deltaTime;

            if (slider.value >= 1f)
            {
                slider.value = 0;
                sliderObj.gameObject.SetActive(false);
                chef.chefMove.DoingAnim(false);
                SliderFinish();
                yield break;
            }
            yield return null;
        }
        chef.chefMove.DoingAnim(false);
    }
    protected abstract void SliderFinish();

    public void UpdateColor()
    {
        // 0에서 1 사이 값을 기준으로 빨간색에서 초록색으로 보간
        Color newColor = Color.Lerp(spriteData.Export_Pallate("Red"), spriteData.Export_Pallate("Green"), slider.value);
        fillImg.color = newColor;
    }
}
