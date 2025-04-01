using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

namespace Vampire
{
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

                    RelicContent newContent = new RelicContent(ranID, amount);
                    DataManager.Instance.Export_RelicData_Ref(id).statContent = new RelicContent[1] { newContent };
                    break;
                case 17:
                    ranID = CSVManager.Instance.Find_StatData_PlayerLvUp_Ran().ID;
                    amount = CSVManager.Instance.Find_StatData_PlayerLvUp(ranID, TierType.Epic);

                    newContent = new RelicContent(ranID, amount);
                    DataManager.Instance.Export_RelicData_Ref(id).statContent = new RelicContent[1] { newContent };
                    break;
                case 18:
                    GameManager_Survivor.Instance.stat.LevelUp();
                    break;
                case 30:
                    List<RelicContent> contentList = new List<RelicContent>();

                    for (int i = 0; i < 2; i++)
                    {
                        ranID = CSVManager.Instance.Find_StatData_PlayerLvUp_Ran().ID;
                        if (i == 1 && ranID == contentList[0].ID)
                        {
                            i--;
                            continue;
                        }

                        amount = CSVManager.Instance.Find_StatData_PlayerLvUp(ranID, TierType.Common);

                        newContent = new RelicContent(ranID, amount);
                        contentList.Add(newContent);
                    }
                    DataManager.Instance.Export_RelicData_Ref(id).statContent = contentList.ToArray();
                    SpawnManager.Instance.Spawn_Droptem_Ran(GameManager_Survivor.Instance.player.Get_Player_RoundPos(5f));
                    break;
                case 32:
                    // ���� ����
                    break;
                case 37:
                    // ���� ���� �ֺ��� �Űܺ���
                    break;
                case 46:
                    int length = CSVManager.Instance.csvList.statDatas_PlayerLvUp.Length;
                    List<StatID_Player> min1List = new List<StatID_Player>();
                    int min1 = int.MaxValue; // �ּڰ� �ʱ�ȭ

                    for (int i = 0; i < length; i++)
                    {
                        // ���� ID�� Stat �� ��������
                        StatID_Player curID = CSVManager.Instance.csvList.statDatas_PlayerLvUp[i].ID;
                        int statValue = GameManager_Survivor.Instance.stat.GetStat(curID, true);

                        // ���ο� �ּڰ� �߰�
                        if (statValue < min1)
                        {
                            min1 = statValue;
                            min1List.Clear(); // ���� �ּڰ� ����Ʈ ����
                            min1List.Add(curID);
                        }
                        // �ּڰ��� ������ �� �߰�
                        else if (statValue == min1)
                        {
                            min1List.Add(curID);
                        }
                    }

                    // ���� ����
                    ranID = min1List[Random.Range(0, min1List.Count)];
                    newContent = new RelicContent(ranID, CSVManager.Instance.Find_StatData_PlayerLvUp(ranID, TierType.Common));
                    DataManager.Instance.Export_RelicData_Ref(id).statContent = new RelicContent[1] { newContent };
                    break;
                case 47:
                    length = CSVManager.Instance.csvList.statDatas_PlayerLvUp.Length;
                    min1List = new List<StatID_Player>();
                    List<StatID_Player> min2List = new List<StatID_Player>();
                    min1 = int.MaxValue; // �ּڰ�
                    int min2 = int.MaxValue; // �� ��° �ּڰ�

                    // ù ��° �ּڰ� Ž�� �� ����Ʈ ����
                    for (int i = 0; i < length; i++)
                    {
                        StatID_Player curID = CSVManager.Instance.csvList.statDatas_PlayerLvUp[i].ID;
                        int statValue = GameManager_Survivor.Instance.stat.GetStat(curID, true);

                        if (statValue < min1)
                        {
                            // ���ο� �ּڰ� �߰�: ������Ʈ �� ���� ����Ʈ �ʱ�ȭ
                            min2 = min1;
                            min2List = new List<StatID_Player>(min1List);
                            min1 = statValue;
                            min1List.Clear();
                            min1List.Add(curID);
                        }
                        else if (statValue == min1)
                        {
                            // ������ �ּڰ� �߰�
                            min1List.Add(curID);
                        }
                        else if (statValue > min1 && statValue < min2)
                        {
                            // �� ��° �ּڰ� �߰�: ������Ʈ
                            min2 = statValue;
                            min2List.Clear();
                            min2List.Add(curID);
                        }
                        else if (statValue == min2)
                        {
                            // ������ �� ��° �ּڰ� �߰�
                            min2List.Add(curID);
                        }
                    }

                    // ���� ���� ����
                    List<StatID_Player> resultList = new List<StatID_Player>();

                    if (min1List.Count >= 2)
                    {
                        // �ּڰ� ����Ʈ���� 2�� ���� ����
                        while (resultList.Count < 2)
                        {
                            StatID_Player randomID = min1List[Random.Range(0, min1List.Count)];
                            if (!resultList.Contains(randomID))
                                resultList.Add(randomID);
                        }
                    }
                    else
                    {
                        // �ּڰ� �ϳ��� �� ��° �ּڰ� �ϳ� ����
                        if (min1List.Count == 1)
                            resultList.Add(min1List[0]);
                        if (min2List.Count > 0)
                            resultList.Add(min2List[Random.Range(0, min2List.Count)]);
                    }

                    contentList = new List<RelicContent>();
                    // ��� ����
                    for (int i = 0; i < 2; i++)
                    {
                        newContent = new RelicContent(resultList[i], CSVManager.Instance.Find_StatData_PlayerLvUp(resultList[i], TierType.Common));
                        contentList.Add(newContent);
                    }
                    DataManager.Instance.Export_RelicData_Ref(id).statContent = contentList.ToArray();
                    break;
                case 49:
                    contentList = new List<RelicContent>();

                    for (int i = 0; i < 2; i++)
                    {
                        ranID = CSVManager.Instance.Find_StatData_PlayerLvUp_Ran().ID;

                        if (i == 1 && contentList[0].ID == ranID)
                        {
                            i--;
                            continue;
                        }

                        newContent = new RelicContent(ranID, (i == 1) ?
                            CSVManager.Instance.Find_StatData_PlayerLvUp(ranID, TierType.Epic) : -CSVManager.Instance.Find_StatData_PlayerLvUp(ranID, TierType.Common));
                        contentList.Add(newContent);
                    }
                    DataManager.Instance.Export_RelicData_Ref(id).statContent = contentList.ToArray();
                    break;
                case 59:
                    Player player = GameManager_Survivor.Instance.player;

                    int curHP = GameManager_Survivor.Instance.stat.curHP;
                    player.Hitted(curHP / 2);
                    break;
                case 60:
                    GameManager_Survivor.Instance.player.rebornCount++;
                    break;
                case 61:
                    DataManager.Instance.EarnCoin(100);
                    break;
            }
        }

        public bool IsHave(int id)
        {
            return relicCollectors.Contains(id);
        }
    }
}
