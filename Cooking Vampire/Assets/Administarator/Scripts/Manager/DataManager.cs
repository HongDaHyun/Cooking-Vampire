using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public StageType curStage;
}

public enum StageType { Grass = 0, Cave, Swarm, Forest }