using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Com.LuisPedroFonseca.ProCamera2D;
using Vampire;

public class BtnManager : Singleton<BtnManager>
{
    public void Tab(RectTransform rect)
    {
        if (!rect.gameObject.activeSelf)
        {
            rect.gameObject.SetActive(true);
            UIManager.Instance.raycastPannel.SetActive(true);
            rect.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            rect.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce).SetUpdate(true);
        }
        else
        {
            UIManager.Instance.raycastPannel.SetActive(false);
            rect.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0.25f).SetEase(Ease.InOutExpo).SetUpdate(true).OnComplete(() => rect.gameObject.SetActive(false));
        }
    }
    public void Tab_NoRayCast(RectTransform rect)
    {
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

    public void Stop()
    {
        Time.timeScale = 0;
    }
    public void Resume()
    {
        Time.timeScale = 1;
    }
    public void TestBoss()
    {
        StartCoroutine(UIManager.Instance.bossPannel.CinematicSequence());
    }

    public void ShakeCamera()
    {
        ProCamera2DShake camShake = Camera.main.GetComponent<ProCamera2DShake>();

        camShake.Shake(0);
    }
}
