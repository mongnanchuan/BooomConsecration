using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public int levelID;
    public List<int[]> monsterGroupIDs;
    public List<int[]> monsterGroupPoss;
    public int currentGroupCount;
    public int playerPos;

    public Dictionary<int,int> currentMonsterPos;
    public int count;

    public void GetPlayerPos(int player)
    {
        playerPos = player;
    }

    public void MonsterGroupInit()
    {
        if(monsterGroupIDs == null)
            monsterGroupIDs = new List<int[]>();
        else
            monsterGroupIDs.Clear();

        if (monsterGroupPoss == null)
            monsterGroupPoss = new List<int[]>();
        else
            monsterGroupPoss.Clear();

        if (currentMonsterPos == null)
            currentMonsterPos = new Dictionary<int, int>();
        else
            currentMonsterPos.Clear();

        var levelConfig = ConfigManager.Instance.GetConfig<LevelConfig>(levelID);
        monsterGroupIDs.Add(levelConfig.group1ID);
        monsterGroupIDs.Add(levelConfig.group2ID);
        monsterGroupIDs.Add(levelConfig.group3ID);
        monsterGroupPoss.Add(levelConfig.group1Pos);
        monsterGroupPoss.Add(levelConfig.group2Pos);
        monsterGroupPoss.Add(levelConfig.group3Pos);
        currentGroupCount = 0;
        count = 1;

        SetNewMonsters();
    }

    public void SetNewMonsters()
    {
        int[] currentGroupID = monsterGroupIDs[currentGroupCount];
        int[] currrentGroupPos = monsterGroupPoss[currentGroupCount];

        for (int i = 0; i < currentGroupID.Length; i++)
        {
            int monsterID = currentGroupID[i];
            int monsterPos_TempNum = currrentGroupPos[i];

            HashSet<int> currentPosNum = new(currentMonsterPos.Values);
            currentPosNum.Add(playerPos);
            int monsterPos_Num = FindClosestPos(monsterPos_TempNum, 9, currentPosNum);
            Vector2 monsterPos_Vec = SwitchPos.IntToVector2(monsterPos_Num);

            GameObject prefab = Resources.Load<GameObject>($"Monsters/Monster{monsterID}");
            GameObject monster = GameObject.Instantiate(prefab, monsterPos_Vec, Quaternion.identity, transform);
            if (monsterPos_Num < playerPos)
            {
                Vector3 dir = monster.transform.localScale;
                monster.transform.localScale = new Vector3(-dir.x, dir.y, dir.z);
            }
            currentMonsterPos.Add(count, monsterPos_Num);
            monster.GetComponent<MonsterBase>().count = count;
            count++;
            //string className = $"Monster{currentGroupID[i]}";
            //Type type = Type.GetType(className);
            //if (type != null)
            //{
            //    clone.AddComponent(type);
            //}
            //else
            //{
            //    Debug.LogError("找不到类型：" + className);
            //}
        }

        currentGroupCount++;
    }

    public void GetSkillDamage(int[] posGroup,bool isToRight,int damage)
    {

    }



    public int FindClosestPos(int targetIndex, int mapLength, HashSet<int> occupied)
    {
        for (int offset = 0; offset < mapLength; offset++)
        {
            int left = targetIndex - offset;
            if (left >= 0 && !occupied.Contains(left))
                return left;

            int right = targetIndex + offset;
            if (right < mapLength && !occupied.Contains(right))
                return right;
        }
        Debug.LogError("找不到可用的位置了！");
        return -1;
    }

}
