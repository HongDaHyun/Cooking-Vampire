using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Sirenix.OdinInspector;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    [Title("����")]
    public GameObject raycastPannel;

    [Title("�����̹�")]
    public Slider expSlider;
    public Slider hpSlider;
    public TextMeshProUGUI levelTxt, killTxt, timeTxt, coinTxt;
    public TextMeshProUGUI[] weaponTest_Btn;
    public AtkUI[] atkUIs;
    public LvUpPannel lvUpPannel;
    public BossPannel bossPannel;

    private void Start()
    {
        lvUpPannel.Set_StatUI_Player();
    }

    private void LateUpdate()
    {
        Update_HUD();
    }

    private void Update_HUD()
    {
        GameManager_Survivor gm = GameManager_Survivor.Instance;
        DataManager dm = DataManager.Instance;

        // EXP_SLIDER
        float cur = gm.exp;
        float max = gm.maxExp;

        float targetValue = cur / max;
        if (Mathf.Abs(expSlider.value - targetValue) < 0.0001f)
            expSlider.value = targetValue; // ��Ȯ�� 1�� ����
        else
            expSlider.value = Mathf.Lerp(expSlider.value, targetValue, Time.deltaTime * 10f);
        if (expSlider.value >= 1 && gm.exp >= gm.maxExp)
            gm.Player_LevelUp();

        // LV_TXT
        levelTxt.text = $"Lv.{gm.level:F0}";

        // KILL_TXT
        killTxt.text = $"x{gm.killCount:F0}";

        // TIMER_TXT
        float remainTime = gm.MAX_GAMETIME - gm.curGameTime;
        int min = Mathf.FloorToInt(remainTime / 60);
        int sec = Mathf.FloorToInt(remainTime % 60);
        timeTxt.text = $"{min:D2}:{sec:D2}";

        // COIN_TXT
        coinTxt.text = dm.coin.ToString();

        // HP_SLIDER
        float curHp = gm.health;
        float maxHp = gm.stat.HP;

        float targetHP = curHp / maxHp;
        hpSlider.value = Mathf.Lerp(hpSlider.value, targetHP, Time.deltaTime * 5f);

        // TEST_BTN
        for (int i = 0; i < weaponTest_Btn.Length; i++)
        {
            weaponTest_Btn[i].text = gm.player.atkController.availAtks[i].isMax ?
                $"Weapon {i}\nLv.MAX" : $"Weapon {i}\nLv.{gm.player.atkController.availAtks[i].lv}";
        }
    }
}

[System.Serializable]
public class LvUpPannel
{
    public RectTransform parentPannel, atkPannel, statPannel;
    public GameObject atkPannel_Atk, atkPannel_Stat;
    public AtkUP_Btn[] atkUps;
    public StatUP_Btn[] statUps;
    public StatUP_Btn[] atkUps_Stats;
    [HideInInspector] public StatUI_Player[] statUI_Players;

    public void Tab(BtnManager bm)
    {
        bm.Tab(parentPannel);

        Active_Stat();

        bm.Stop();
    }
    public void UnTab(BtnManager bm)
    {
        bm.Tab(parentPannel);

        bm.Resume();
    }
    public void Active_Atk()
    {
        atkPannel.gameObject.SetActive(true);
        statPannel.gameObject.SetActive(false);

        Reroll_AtkUPs();
    }
    public void Active_Stat()
    {
        atkPannel.gameObject.SetActive(false);
        statPannel.gameObject.SetActive(true);

        Reroll_StatUPs(statUps);
    }

    public void Reroll_StatUPs(StatUP_Btn[] btns)
    {
        CSVManager cm = CSVManager.Instance;
        List<StatID_Player> stats = new List<StatID_Player>();

        foreach(StatUP_Btn btn in btns)
        {
            StatID_Player ranID;
            do
                ranID = cm.Find_StatData_PlayerLvUp_Ran().ID;
            while (stats.Contains(ranID));

            stats.Add(ranID);

            btn.SetBtn(ranID);
        }
    }
    public void Reroll_AtkUPs()
    {
        List<Atk> atks = GameManager_Survivor.Instance.player.atkController.availAtks.ToList().FindAll(data => !data.isMax);
        int maxLength = Mathf.Min(atks.Count, 3);

        // atkUps ���� (���׷��̵� �� ������ ���� ���� ��)
        if(maxLength > 0)
        {
            atkPannel_Atk.SetActive(true);
            atkPannel_Stat.SetActive(false);

            foreach (AtkUP_Btn btn in atkUps)
                btn.gameObject.SetActive(false);

            for (int i = 0; i < maxLength; i++)
            {
                int ranID = Random.Range(0, atks.Count);
                Atk weapon = atks[ranID];
                atks.RemoveAt(ranID);

                atkUps[i].gameObject.SetActive(true);
                atkUps[i].SetBtn(weapon);
            }
        }
        // atkUps_Stats ���� (��� ������ ���׷��̵� ���� ��)
        else
        {
            atkPannel_Atk.SetActive(false);
            atkPannel_Stat.SetActive(true);

            Reroll_StatUPs(atkUps_Stats);
        }
    }

    public void Set_StatUI_Player()
    {
        CSVManager cm = CSVManager.Instance;
        SpawnManager sm = SpawnManager.Instance;

        StatData_Player[] datas = cm.csvList.statDatas_Player;
        statUI_Players = new StatUI_Player[datas.Length];

        int count = 0;
        foreach(StatData_Player data in datas)
        {
            statUI_Players[count] = sm.Spawn_StatUI_Player(data);
            count++;
        }
    }
    public void Adjust_StatUI_Player(StatID_Player id)
    {
        System.Array.Find(statUI_Players, ui => ui.statID == id).AdjustUI();
    }
}

[System.Serializable]
public class BossPannel
{
    public Image redPannel;

    public void SetUI()
    {
        BtnManager bm = BtnManager.Instance;
        DataManager dm = DataManager.Instance;
        LevelManager lm = LevelManager.Instance;

        bm.Stop(); // ����

        float delay = 1f; // ��Ʈ�� �⺻ ������
        List<Image> warningImgs = redPannel.GetComponentsInChildren<Image>().ToList();
        warningImgs.RemoveAt(0); // �θ� ������Ʈ ����

        // ��Ʈ�� ������ ����
        Sequence warningSeq = DOTween.Sequence().SetUpdate(true)
            .OnStart(() => {
                redPannel.gameObject.SetActive(true); // ���� ��� Ȱ��ȭ
                // ���� ��� �ʱ� ���İ� ����
                Color red = redPannel.color;
                red.a = 0.2f;
                redPannel.color = red;
                // �ڽ� �̹����� �ʱ� ���İ� 0���� ����
                foreach (Image img in warningImgs)
                    img.color = new Vector4(1, 1, 1, 0);
            });
        Tween redPannelFadeTween = redPannel.DOFade(0.5f, delay).SetLoops(-1, LoopType.Yoyo).SetUpdate(true); // �ݺ��� Ʈ��(�ڽ� �̹��� FadeIn)
        // Ʈ�� �ݺ�
        for (int i = 0; i < warningImgs.Count; i++)
        {
            // 4��° ���� FadeIn �ӵ� ������
            if (i % 4 == 0 && i != 0)
            {
                delay = Mathf.Max(0.1f, delay / 2f);
                warningSeq.AppendCallback(() => {
                    redPannelFadeTween.Kill();
                    redPannelFadeTween = redPannel.DOFade(0.5f, delay).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
                });
            }
            warningSeq.Append(warningImgs[i].DOFade(1f, delay));
        }
        warningSeq.AppendCallback(() => redPannelFadeTween.Kill());
        // ������ ����
        warningSeq.Append(redPannel.DOFade(0f, 1f))
            .OnComplete(() => {
                redPannel.gameObject.SetActive(false);
                bm.Resume();
                SpawnManager.Instance.Spawn_Effect_X(dm.Export_BossData(dm.curStage).title, lm.SpawnPoint_Ran(0), 2f);
            });
        foreach (Image img in warningImgs)
            warningSeq.Join(img.DOFade(0f, 1f));
    }
}