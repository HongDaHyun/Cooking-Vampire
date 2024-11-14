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
    GameManager_Survivor gm;
    DataManager dm;

    [Title("����")]
    public GameObject raycastPannel;

    [Title("�����̹�")]
    public Slider expSlider;
    public Slider hpSlider;
    public TextMeshProUGUI levelTxt, killTxt, timeTxt, coinTxt;
    public TextMeshProUGUI[] weaponTest_Btn;
    public WeaponUI[] weaponUIs;
    public LvUpPannel lvUpPannel;
    public BossPannel bossPannel;

    private void Start()
    {
        gm = GameManager_Survivor.Instance;
        dm = DataManager.Instance;

        lvUpPannel.Set_StatUI_Player();
    }

    private void LateUpdate()
    {
        Update_HUD();
    }

    private void Update_HUD()
    {
        // EXP_SLIDER
        float cur = gm.exp;
        float max = gm.maxExp;

        float targetValue = cur / max;
        expSlider.value = Mathf.Lerp(expSlider.value, targetValue, Time.deltaTime * 5f);
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
        float maxHp = gm.stat.maxHealth;

        float targetHP = curHp / maxHp;
        hpSlider.value = Mathf.Lerp(hpSlider.value, targetHP, Time.deltaTime * 5f);

        // TEST_BTN
        for (int i = 0; i < weaponTest_Btn.Length; i++)
        {
            weaponTest_Btn[i].text = gm.player.weaponController.availWeapons[i].isMax ?
                $"Weapon {i}\nLv.MAX" : $"Weapon {i}\nLv.{gm.player.weaponController.availWeapons[i].lv}";
        }
    }
}

[System.Serializable]
public class LvUpPannel
{
    public RectTransform transform;
    public StatUpPannel[] statUpPannels;
    public StatUI_Player[] statUI_Players;

    public void Set_StatUpPannels_Ran()
    {
        List<Weapon> weapons = GameManager_Survivor.Instance.player.weaponController.availWeapons.ToList().FindAll(data => !data.isMax);
        List<int> stats = new List<int>();

        bool isWeapon = weapons.Count < 3 || weapons == null ? true : Random.Range(0, 2) == 0; // 0 -> Weapon / 1 -> Stat

        foreach (StatUpPannel pannel in statUpPannels)
        {
            if (isWeapon)
            {
                int ranIndex = Random.Range(0, weapons.Count);
                Weapon weapon = weapons[ranIndex];
                weapons.RemoveAt(ranIndex);

                pannel.SetUI(weapon);
            }
            else
            {
                int ranID;
                do
                {
                    ranID = Random.Range(0, System.Enum.GetValues(typeof(StatType)).Length);
                }
                while (stats.Contains(ranID));

                stats.Add(ranID);
                pannel.SetUI(ranID);
            }
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
    public void Adjust_StatUI_Player(StatID_Player id, int value)
    {
        System.Array.Find(statUI_Players, ui => ui.statID == id).AdjustUI(value);
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