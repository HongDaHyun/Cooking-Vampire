using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Sirenix.OdinInspector;
using DG.Tweening;
using Com.LuisPedroFonseca.ProCamera2D;

namespace Vampire 
{
    public class UIManager : Singleton<UIManager>
    {
        [Title("공용")]
        public GameObject raycastPannel;

        [Title("서바이벌")]
        public Slider expSlider;
        public Slider hpSlider, healSlider;
        public TextMeshProUGUI levelTxt, killTxt, timeTxt, coinTxt;
        public TextMeshProUGUI[] weaponTest_Btn;
        public AtkUI[] atkUIs;
        public LvUpPannel lvUpPannel;
        public BossPannel bossPannel;
        public PlayerIcon_Btn playerIcon;

        SpawnManager sm;
        GameManager_Survivor gm;
        DataManager dm;

        private void Start()
        {
            gm = GameManager_Survivor.Instance;
            dm = DataManager.Instance;
            sm = SpawnManager.Instance;

            lvUpPannel.Set_StatUI_Player();
        }

        private void Update()
        {
            Update_HUD();
        }

        private void Update_HUD()
        {
            // EXP_SLIDER
            float cur = gm.stat.curExp;
            float max = gm.stat.maxExp;

            float targetValue = cur / max;
            if (Mathf.Abs(expSlider.value - targetValue) < 0.0001f)
                expSlider.value = targetValue; // 정확히 1로 설정
            else
                expSlider.value = Mathf.Lerp(expSlider.value, targetValue, Time.deltaTime * 10f);
            if (expSlider.value >= 0.9f && gm.stat.curExp >= gm.stat.maxExp)
                gm.stat.LevelUp();

            // LV_TXT
            levelTxt.text = $"Lv.{gm.stat.level:F0}";

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
            float curHp = gm.stat.curHP;
            float maxHp = gm.stat.HP;

            float targetHP = curHp / maxHp;
            healSlider.value = targetHP;
            hpSlider.value = Mathf.Lerp(hpSlider.value, targetHP, Time.deltaTime * 5f);

            // BOSS_HP_SLIDER
            if (bossPannel.bossSlider.IsActive())
            {
                float bossCur = 0, bossMax = 0;
                List<Enemy> bossList = sm.Find_EnemyLists(AtkType.Boss);
                foreach (Enemy boss in bossList)
                {
                    bossCur += boss.stat.curHp;
                    bossMax += boss.stat.maxHp;
                }
                float bossTarget = bossCur / bossMax;

                bossPannel.bossSlider.value = Mathf.Lerp(bossPannel.bossSlider.value, bossTarget, Time.deltaTime * 5f);
                bossPannel.sliderText.text = $"{bossCur}/{bossMax}";
            }

            // TEST_BTN
            for (int i = 0; i < weaponTest_Btn.Length; i++)
            {
                weaponTest_Btn[i].text = gm.player.atkController.availAtks[i].isMax ?
                    $"Weapon {i}\nLv.MAX" : $"Weapon {i}\nLv.{gm.player.atkController.availAtks[i].lv}";
            }
        }
    }

    [System.Serializable]
    public struct LvUpPannel
    {
        public RectTransform parentPannel, atkPannel, statPannel;
        public GameObject atkPannel_Atk, atkPannel_Stat;
        public AtkUP_Btn[] atkUps;
        public StatUP_Btn[] statUps;
        public StatUP_Btn[] atkUps_Stats;
        public RelicTooltip relicToolTip;

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
            GameManager_Survivor.Instance.SetCamZoom();
        }
        public void Active_Atk()
        {
            atkPannel.gameObject.SetActive(true);
            statPannel.gameObject.SetActive(false);

            Adjust_StatUI_Player();

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

            foreach (StatUP_Btn btn in btns)
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

            // atkUps 리롤 (업그레이드 할 공격이 남아 있을 때)
            if (maxLength > 0)
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
            // atkUps_Stats 리롤 (모든 공격을 업그레이드 했을 때)
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
            foreach (StatData_Player data in datas)
            {
                statUI_Players[count] = sm.Spawn_StatUI_Player(data);
                count++;
            }
        }
        public void Adjust_StatUI_Player()
        {
            foreach (StatUI_Player ui in statUI_Players)
                ui.AdjustUI();
        }
    }
    [System.Serializable]
    public struct RelicTooltip
    {
        public CanvasGroup group;
        public Image icon;
        public TextMeshProUGUI nameTxt, tierTxt, contentsTxt, explainTxt;

        private void SetTooltip(RelicData data)
        {
            SpriteData spriteData = SpriteData.Instance;
            DataManager dm = DataManager.Instance;

            icon.sprite = data.sprites[2];

            nameTxt.text = data.relicName;
            tierTxt.text = dm.Get_Tier_Name(data.tierType);
            tierTxt.color = spriteData.Export_TierColor(data.tierType);
            contentsTxt.text = GetContent(data);
            explainTxt.text = "-> " + data.explain;
        }
        private string GetContent(RelicData data)
        {
            CSVManager cm = CSVManager.Instance;

            string str = "";
            int length = data.statContent.Length;

            for (int i = 0; i < length; i++)
            {
                RelicContent content = data.statContent[i];
                StatData_Player playerData = cm.Find_StatData_Player(content.ID);
                str += cm.Find_StatData_ContentText(content.amount, playerData.name, playerData.isPercent) + "\n";
            }
            if (data.specialContent.explain != "")
            {
                str += "-> " + string.Format(data.specialContent.explain,
                    cm.Find_StatData_SpecialRelic_ContentText(data.specialContent.specialContents)) + "\n";
            }

            return str;
        }
        public void Tab(RelicData data)
        {
            // 첫 등장
            if (!group.gameObject.activeSelf)
            {
                SetTooltip(data);
                group.gameObject.SetActive(true);
                group.alpha = 0f;

                group.DOFade(1f, 0.5f).SetUpdate(true);
            }
            // 두 번째 등장
            else
            {
                RelicTooltip tip = this;
                Sequence seq = DOTween.Sequence().SetUpdate(true);

                seq.Append(group.DOFade(0f, 0.3f))
                    .AppendCallback(() => tip.SetTooltip(data))
                    .Append(group.DOFade(1f, 0.5f));
            }
        }
    }

    [System.Serializable]
    public class BossPannel
    {
        public bool isCinematic;
        [Title("보스 패널")]
        public GameObject pannel;
        public TextMeshProUGUI bossText;
        [Title("보스 체력바")]
        public Slider bossSlider;
        public TextMeshProUGUI sliderText;

        public IEnumerator CinematicSequence()
        {
            isCinematic = true;
            DataManager dm = DataManager.Instance;
            LevelManager lm = LevelManager.Instance;
            BtnManager bm = BtnManager.Instance;
            SpawnManager sm = SpawnManager.Instance;

            // 1. 패널UI 설정
            EnemyData bossData = dm.Export_BossData(dm.curStage);
            Vector2 spawnPos = lm.SpawnPoint_Ran(0);
            bossText.text = bossData.title;
            pannel.SetActive(true);

            // 2. 모든 적 죽임
            lm.StopCoroutine(lm.levelRoutine);
            sm.KillAll_Enemy();

            // 3. 카메라 시네마틱
            ProCamera2DCinematics cinematic = Camera.main.GetComponent<ProCamera2DCinematics>();

            Effect_X x = SpawnManager.Instance.Spawn_Effect_X(bossData.title, spawnPos, 2f);

            cinematic.CinematicTargets.Clear();
            cinematic.AddCinematicTarget(x.transform, 1f);
            cinematic.Play();

            yield return new WaitUntil(() => x.spawnEnemy != null);

            cinematic.AddCinematicTarget(x.spawnEnemy.transform, 1f);
            cinematic.GoToNextTarget();

            yield return new WaitForSeconds(3f);

            cinematic.Stop();
            cinematic.CinematicTargets.Clear();

            pannel.SetActive(false);
            isCinematic = false;
            bossSlider.gameObject.SetActive(true);
        }
    }
}