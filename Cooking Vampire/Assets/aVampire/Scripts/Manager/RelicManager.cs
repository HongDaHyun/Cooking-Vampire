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
                    // 번개 공격
                    break;
                case 37:
                    // 독성 공격 주변에 옮겨붙음
                    break;
                case 46:
                    int length = CSVManager.Instance.csvList.statDatas_PlayerLvUp.Length;
                    List<StatID_Player> min1List = new List<StatID_Player>();
                    int min1 = int.MaxValue; // 최솟값 초기화

                    for (int i = 0; i < length; i++)
                    {
                        // 현재 ID와 Stat 값 가져오기
                        StatID_Player curID = CSVManager.Instance.csvList.statDatas_PlayerLvUp[i].ID;
                        int statValue = GameManager_Survivor.Instance.stat.GetStat(curID, true);

                        // 새로운 최솟값 발견
                        if (statValue < min1)
                        {
                            min1 = statValue;
                            min1List.Clear(); // 이전 최솟값 리스트 제거
                            min1List.Add(curID);
                        }
                        // 최솟값과 동일한 값 발견
                        else if (statValue == min1)
                        {
                            min1List.Add(curID);
                        }
                    }

                    // 랜덤 선택
                    ranID = min1List[Random.Range(0, min1List.Count)];
                    newContent = new RelicContent(ranID, CSVManager.Instance.Find_StatData_PlayerLvUp(ranID, TierType.Common));
                    DataManager.Instance.Export_RelicData_Ref(id).statContent = new RelicContent[1] { newContent };
                    break;
                case 47:
                    length = CSVManager.Instance.csvList.statDatas_PlayerLvUp.Length;
                    min1List = new List<StatID_Player>();
                    List<StatID_Player> min2List = new List<StatID_Player>();
                    min1 = int.MaxValue; // 최솟값
                    int min2 = int.MaxValue; // 두 번째 최솟값

                    // 첫 번째 최솟값 탐색 및 리스트 생성
                    for (int i = 0; i < length; i++)
                    {
                        StatID_Player curID = CSVManager.Instance.csvList.statDatas_PlayerLvUp[i].ID;
                        int statValue = GameManager_Survivor.Instance.stat.GetStat(curID, true);

                        if (statValue < min1)
                        {
                            // 새로운 최솟값 발견: 업데이트 및 기존 리스트 초기화
                            min2 = min1;
                            min2List = new List<StatID_Player>(min1List);
                            min1 = statValue;
                            min1List.Clear();
                            min1List.Add(curID);
                        }
                        else if (statValue == min1)
                        {
                            // 동일한 최솟값 추가
                            min1List.Add(curID);
                        }
                        else if (statValue > min1 && statValue < min2)
                        {
                            // 두 번째 최솟값 발견: 업데이트
                            min2 = statValue;
                            min2List.Clear();
                            min2List.Add(curID);
                        }
                        else if (statValue == min2)
                        {
                            // 동일한 두 번째 최솟값 추가
                            min2List.Add(curID);
                        }
                    }

                    // 랜덤 선택 로직
                    List<StatID_Player> resultList = new List<StatID_Player>();

                    if (min1List.Count >= 2)
                    {
                        // 최솟값 리스트에서 2개 랜덤 선택
                        while (resultList.Count < 2)
                        {
                            StatID_Player randomID = min1List[Random.Range(0, min1List.Count)];
                            if (!resultList.Contains(randomID))
                                resultList.Add(randomID);
                        }
                    }
                    else
                    {
                        // 최솟값 하나와 두 번째 최솟값 하나 선택
                        if (min1List.Count == 1)
                            resultList.Add(min1List[0]);
                        if (min2List.Count > 0)
                            resultList.Add(min2List[Random.Range(0, min2List.Count)]);
                    }

                    contentList = new List<RelicContent>();
                    // 결과 설정
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
