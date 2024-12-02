using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Redcode.Pools;
using TMPro;
using DG.Tweening;

public class InfoTxt : MonoBehaviour, IPoolObject
{
    public TextMeshProUGUI titleTxt, subTitleTxt, contentsTxt;
    private RectTransform rect;

    SpawnManager sm;

    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        rect = GetComponent<RectTransform>();

        sm = SpawnManager.Instance;
    }

    public void OnGettingFromPool()
    {
        Sequence seq = DOTween.Sequence().SetUpdate(true);
        seq.OnStart(() => rect.localScale = new Vector3(1f, 0f, 1f));
        seq.Append(rect.DOScaleY(1f, 0.3f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce))
            .AppendInterval(3f)
            .Append(rect.DOScaleY(0f, 0.3f).SetEase(Ease.InOutExpo))
            .OnComplete(() => sm.Destroy_InfoTxt(this));
    }

    public void SetText(string title, string subTitle, string contents, RectTransform _rect)
    {
        titleTxt.text = title;
        subTitleTxt.text = subTitle;
        contentsTxt.text = contents;

        rect.SetParent(_rect);
        rect.localScale = Vector3.one;
        float parentHeight = _rect.rect.height;
        rect.anchoredPosition = new Vector2(0, parentHeight / 4f);
    }
}
