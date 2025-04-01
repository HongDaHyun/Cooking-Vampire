using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private SpriteData spriteData;

    private Slider slider;
    private Image fillImg;

    private void Awake()
    {
        spriteData = SpriteData.Instance;
        slider = GetComponent<Slider>();
        fillImg = slider.transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        slider.value += Time.deltaTime;
    }

    public void UpdateColor(float value)
    {
        // 0에서 1 사이 값을 기준으로 빨간색에서 초록색으로 보간
        Color newColor = Color.Lerp(spriteData.Export_Pallate("Red"), spriteData.Export_Pallate("Red"), value);
        fillImg.color = newColor;
    }
}
