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
        // 0���� 1 ���� ���� �������� ���������� �ʷϻ����� ����
        Color newColor = Color.Lerp(spriteData.Export_Pallate("Red"), spriteData.Export_Pallate("Red"), value);
        fillImg.color = newColor;
    }
}
