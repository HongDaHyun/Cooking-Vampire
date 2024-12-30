using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Redcode.Pools;

public class RelicUI : MonoBehaviour, IPoolObject
{
    private RelicData relicData;
    private Image iconImg;

    UIManager um;

    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        um = UIManager.Instance;
        iconImg = transform.GetChild(0).GetComponent<Image>();
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
        um.lvUpPannel.relicToolTip.Tab(relicData);
    }
}
