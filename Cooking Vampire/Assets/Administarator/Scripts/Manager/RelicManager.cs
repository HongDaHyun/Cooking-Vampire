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
            case 8:
            case 10:
                SpawnManager.Instance.Spawn_Droptem_Ran(GameManager_Survivor.Instance.player.Get_Player_RoundPos(5f)); 
                break;
            case 16:
                StatID_Player ranID = CSVManager.Instance.Find_StatData_PlayerLvUp_Ran().ID; ;
                int amount = CSVManager.Instance.Find_StatData_PlayerLvUp(ranID, TierType.Common);

                RelicContent newContent;
                newContent.ID = ranID;
                newContent.amount = amount;
                DataManager.Instance.Export_RelicData_Ref(id).statContent = new RelicContent[1] { newContent };
                break;
            case 17:
                ranID = CSVManager.Instance.Find_StatData_PlayerLvUp_Ran().ID;
                amount = CSVManager.Instance.Find_StatData_PlayerLvUp(ranID, TierType.Epic);

                newContent.ID = ranID;
                newContent.amount = amount;
                DataManager.Instance.Export_RelicData_Ref(id).statContent = new RelicContent[1] { newContent };
                break;
            case 18:
                GameManager_Survivor.Instance.stat.LevelUp();
                break;
            case 20:
                GameManager_Survivor.Instance.stat.SetStat(StatID_Player.CRIT, Mathf.RoundToInt(GameManager_Survivor.Instance.stat.GetStat_Def(StatID_Player.LUK) / 10f));
                break;
            case 30:
                List<RelicContent> contentList = new List<RelicContent>();

                for(int i = 0; i < 2; i++)
                {
                    ranID = CSVManager.Instance.Find_StatData_PlayerLvUp_Ran().ID;
                    if(i == 1 && ranID == contentList[0].ID)
                    {
                        i--;
                        continue;
                    }

                    amount = CSVManager.Instance.Find_StatData_PlayerLvUp(ranID, TierType.Common);

                    newContent.ID = ranID;
                    newContent.amount = amount;

                    contentList.Add(newContent);
                }
                DataManager.Instance.Export_RelicData_Ref(id).statContent = contentList.ToArray();

                SpawnManager.Instance.Spawn_Droptem_Ran(GameManager_Survivor.Instance.player.Get_Player_RoundPos(5f));
                break;
            case 32:
                // 번개 공격
                break;
            case 37:
                // 독성 공격 주변에 옮겨붙음
                break;
            case 41:
                GameManager_Survivor.Instance.stat.SetStat(StatID_Player.DMG, GameManager_Survivor.Instance.stat.GetStat_Def(StatID_Player.BAK));
                break;
            case 46:
                int length = CSVManager.Instance.csvList.statDatas_PlayerLvUp.Length;
                StatID_Player minStatID = StatID_Player.HP;
                int minStat = 999999;

                for(int i = 0; i < length; i++)
                {
                    StatID_Player curID = CSVManager.Instance.csvList.statDatas_PlayerLvUp[i].ID;

                    int curStat = GameManager_Survivor.Instance.stat.GetStat_Def(curID);
                    if (curStat <= minStat)
                    {
                        minStatID = curID;
                        minStat = curStat;
                    }
                }

                newContent.ID = minStatID;
                newContent.amount = CSVManager.Instance.Find_StatData_PlayerLvUp(minStatID, TierType.Common);
                DataManager.Instance.Export_RelicData_Ref(id).statContent = new RelicContent[1] { newContent };
                break;
            case 47:
                // 고치기 (랜덤하게) 위도 마찬가지
                contentList = new List<RelicContent>();
                length = CSVManager.Instance.csvList.statDatas_PlayerLvUp.Length;
                int dupliI = 0;

                for(int j = 0; j < 2; j++)
                {
                    minStatID = StatID_Player.HP;
                    minStat = 999999;

                    for (int i = 0; i < length; i++)
                    {
                        if (dupliI == i && j == 1)
                            continue;
                        StatID_Player curID = CSVManager.Instance.csvList.statDatas_PlayerLvUp[i].ID;

                        int curStat = GameManager_Survivor.Instance.stat.GetStat_Def(curID);
                        if (curStat <= minStat)
                        {
                            minStatID = curID;
                            minStat = curStat;
                            dupliI = i;
                        }
                    }
                    newContent.ID = minStatID;
                    newContent.amount = CSVManager.Instance.Find_StatData_PlayerLvUp(minStatID, TierType.Common);
                    contentList.Add(newContent);
                }

                DataManager.Instance.Export_RelicData_Ref(id).statContent = contentList.ToArray();
                break;
        }
    }

    public bool IsHave(int id)
    {
        return relicCollectors.Contains(id);
    }
}
