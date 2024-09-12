using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using TMPro;
using DG.Tweening;

public class PopUpTxt : MonoBehaviour, IPoolObject
{
    TextMeshPro textMeshPro;
    SpriteData spriteData;
    SpawnManager spawnManager;

    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        textMeshPro = GetComponent<TextMeshPro>();
        spriteData = SpriteData.Instance;
        spawnManager = SpawnManager.Instance;
    }

    public void OnGettingFromPool()
    {
    }

    public void SetUI(int amount, PopUpType type, Vector2 pos)
    {
        transform.position = GetRanPos(pos);
        SetType(amount, type);

        PopUp();
    }

    private Vector2 GetRanPos(Vector2 pos)
    {
        return new Vector2(pos.x + Random.Range(-0.5f, 0.5f), pos.y + Random.Range(0f, 0.5f));
    }
    private void SetType(int amount, PopUpType type)
    {
        switch(type)
        {
            case PopUpType.Deal:
                textMeshPro.text = amount.ToString();
                textMeshPro.color = spriteData.Export_Pallate("Bright_Green");
                break;
            case PopUpType.Deal_Crit:
                textMeshPro.text = amount.ToString();
                textMeshPro.color = spriteData.Export_Pallate("Yellow");
                break;
            case PopUpType.Heal:
                textMeshPro.text = $"+{amount}";
                textMeshPro.color = spriteData.Export_Pallate("Bright_Green");
                break;
        }
    }
    private void PopUp()
    {
        Sequence popSeq = DOTween.Sequence().SetUpdate(true);
        popSeq.Append(textMeshPro.transform.DOMoveY(transform.position.y + 1f, 0.5f))
            .Join(textMeshPro.DOFade(0f, 1f).SetEase(Ease.InExpo))
            .OnComplete(() => spawnManager.Destroy_PopUpTxt(this));
    }
}

public enum PopUpType { Deal, Deal_Crit, Heal,  }