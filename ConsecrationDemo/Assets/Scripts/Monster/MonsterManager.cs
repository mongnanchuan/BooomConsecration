using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public int levelID;
    public List<int[]> monsterGroupIDs;
    public List<int[]> monsterGroupPoss;
    public int currentGroupCount;

    public void MonsterGroupInit()
    {
        var levelConfig = ConfigManager.Instance.GetConfig<LevelConfig>(levelID);
        monsterGroupIDs.Add(levelConfig.group1ID);
        monsterGroupIDs.Add(levelConfig.group2ID);
        monsterGroupIDs.Add(levelConfig.group3ID);
        monsterGroupPoss.Add(levelConfig.group1Pos);
        monsterGroupPoss.Add(levelConfig.group2Pos);
        monsterGroupPoss.Add(levelConfig.group3Pos);
        currentGroupCount = 0;

        SetNewMonsters();
    }

    public void SetNewMonsters()
    {
        int[] currentGroupID = monsterGroupIDs[currentGroupCount];
        int[] currrentGroupPos = monsterGroupPoss[currentGroupCount];

        for (int i = 0; i < currentGroupID.Length; i++)
        {

        }


        currentGroupCount++;
    }



}
