using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RelicManager : Singleton<RelicManager>
{
    [ReadOnly] public List<int> relicCollectors;

    [Title("Inst")]
    public SpriteRenderer relic2;

    public void SpecialCollect(int id)
    {
        switch (id)
        {
            case 2:
                relic2.gameObject.SetActive(true);

                relic2.DOFade(0.3f, 2f);
                break;
        }
    }

    public bool IsHave(int id)
    {
        return relicCollectors.Contains(id);
    }
}
