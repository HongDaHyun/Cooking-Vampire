using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BtnManager : Singleton<BtnManager>
{
    public GameObject raycastPannel;

    private bool isTouching;

    public void Tab(RectTransform rect)
    {
        if (isTouching)
            return;

        if (!rect.gameObject.activeSelf)
        {
            rect.gameObject.SetActive(true);
            raycastPannel.SetActive(true);
            rect.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            rect.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce).SetUpdate(true);
        }
        else
        {
            raycastPannel.SetActive(false);
            rect.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0.25f).SetEase(Ease.InOutExpo).SetUpdate(true).OnComplete(() => rect.gameObject.SetActive(false));
        }
    }

    public void Tab_NoRayCast(RectTransform rect)
    {
        if (isTouching)
            return;

        if (!rect.gameObject.activeSelf)
        {
            rect.gameObject.SetActive(true);
            rect.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            rect.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce).SetUpdate(true);
        }
        else
        {
            rect.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0.25f).SetEase(Ease.InOutExpo).SetUpdate(true).OnComplete(() => rect.gameObject.SetActive(false));
        }
    }
}
