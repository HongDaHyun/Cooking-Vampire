using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Redcode.Pools;

public class RelicUI : MonoBehaviour, IPoolObject
{
    private RelicData relicData;
    private Image iconImg;
    private RectTransform rect;
    private InfoTxtController controller;

    SpawnManager sm;

    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        sm = SpawnManager.Instance;
        iconImg = transform.GetChild(0).GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        controller = GetComponent<InfoTxtController>();
    }

    public void OnGettingFromPool()
    {
    }

    public void SetUI(RelicData data)
    {
        relicData = data;

        iconImg.sprite = data.sprites[2];
    }

    public void OnClick()
    {
        InfoTxt txt = sm.Spawn_InfoTxt(
            relicData.relicName,
            "À¯¹°",
            relicData.contents,
            rect,
            controller
            );

        if(txt != null)
            txt.transform.SetParent(txt.transform.parent?.parent?.parent?.parent);
    }
}
