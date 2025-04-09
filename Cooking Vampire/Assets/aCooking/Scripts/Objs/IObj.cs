using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cooking;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public abstract class IObj : MonoBehaviour
{
    [Title("�� �����ϱ�")]
    public int objID;
    public Transform sliderObj;
    public bool isPickable;

    // Doing�����̴�
    protected Slider slider;
    private Image fillImg;

    // ������Ʈ
    protected SpriteRenderer sr;
    protected SpriteData spriteData;
    protected Chef chef;

    private void Awake()
    {
        chef = GameManager_Cooking.Instance.chef;
        spriteData = SpriteData.Instance;

        sr = GetComponent<SpriteRenderer>();

        if(sliderObj != null)
        {
            slider = sliderObj.GetChild(0).GetComponent<Slider>();
            fillImg = sliderObj.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>();
        }
    }

    public virtual void Doing()
    {
        if (sliderObj != null)
            StartCoroutine(SliderRoutine());
        else
            DoingFinish();
    }

    protected virtual IEnumerator SliderRoutine()
    {
        sliderObj.gameObject.SetActive(true);

        while (chef.chefMove.isInteract)
        {
            slider.value += Time.deltaTime;

            if(slider.value >= 1f)
            {
                slider.value = 0;
                sliderObj.gameObject.SetActive(false);
                DoingFinish();
                yield break;
            }
            yield return null;
        }
    }
    protected abstract void DoingFinish();

    public void UpdateColor()
    {
        // 0���� 1 ���� ���� �������� ���������� �ʷϻ����� ����
        Color newColor = Color.Lerp(spriteData.Export_Pallate("Red"), spriteData.Export_Pallate("Green"), slider.value);
        fillImg.color = newColor;
    }
}
