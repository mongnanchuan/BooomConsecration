using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public int levelID;
    public List<int[]> monsterGroupIDs;//关卡表中的3波怪物ID
    public List<int[]> monsterGroupPoss;//关卡表中的3波怪物位置
    public int currentGroupCount;//目前的组数计数

    //目前的怪物数据，key=怪物编号，value=位置（序号），怪物obj
    public Dictionary<int,(int pos,GameObject obj)> currentMonstersData;
    public int count;//编号时用的序号

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

        if (currentMonstersData == null)
            currentMonstersData = new Dictionary<int, (int,GameObject)>();
        else
            currentMonstersData.Clear();

        var levelConfig = ConfigManager.Instance.GetConfig<LevelConfig>(levelID);
        monsterGroupIDs.Add(levelConfig.group1ID);
        monsterGroupIDs.Add(levelConfig.group2ID);
        monsterGroupIDs.Add(levelConfig.group3ID);
        monsterGroupPoss.Add(levelConfig.group1Pos);
        monsterGroupPoss.Add(levelConfig.group2Pos);
        monsterGroupPoss.Add(levelConfig.group3Pos);
        currentGroupCount = 0;
        count = 1;
    }

    public void SetNewMonsters(int playerPos)
    {
        //获得当前组ID
        int[] currentGroupID = monsterGroupIDs[currentGroupCount];
        //获得当前组位置
        int[] currrentGroupPos = monsterGroupPoss[currentGroupCount];

        for (int i = 0; i < currentGroupID.Length; i++)
        {
            int monsterID = currentGroupID[i];
            int monsterPos_TempNum = currrentGroupPos[i];

            HashSet<int> currentPosNum = new(currentMonstersData.Values.Select(pos => pos.Item1)); 
            currentPosNum.Add(playerPos);
            int monsterPos_Num = FindClosestPos(monsterPos_TempNum, 9, currentPosNum);
            Vector2 monsterPos_Vec = SwitchPos.IntToVector2(monsterPos_Num);

            GameObject prefab = Resources.Load<GameObject>($"Monsters/Monster{monsterID}");
            GameObject monster = GameObject.Instantiate(prefab, monsterPos_Vec, Quaternion.identity, transform);
            
            MonsterBase tempBase = monster.GetComponent<MonsterBase>();
            tempBase.Init();
            if (monsterPos_Num < playerPos)
                tempBase.isToRight = true;
            else
                tempBase.isToRight = false;
            tempBase.SetDir();

            currentMonstersData.Add(count, (monsterPos_Num,monster));
            monster.GetComponent<MonsterBase>().count = count;
            monster.GetComponent<MonsterBase>().currentPos = monsterPos_Num;
            count++;
        }
        currentGroupCount++;
    }

    public void StartMonsterTurn(int playerPos)
    {
        //删除掉已经没有的怪物
        List<int> toRemove = new List<int>();
        foreach (var item in currentMonstersData)
        {
            GameObject obj = item.Value.obj;
            if (obj == null)
            {
                toRemove.Add(item.Key);
            }
        }
        foreach (int key in toRemove)
        {
            currentMonstersData.Remove(key);
        }

        //剩下的怪物按照序号顺序一个一个判断执行内容
        foreach (var item in currentMonstersData.OrderBy(pair => pair.Key))
        {
            MonsterBase currentBase = item.Value.obj.GetComponent<MonsterBase>();
            int[] monsPos = currentMonstersData.Values.Select(v => v.pos).ToArray();
            int newPos = currentBase.StartTurn(monsPos,playerPos);

            //更新字典中的怪物位置
            currentMonstersData[item.Key] = (newPos, item.Value.obj);
        }
    }

    //刷怪时避免占位重复
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
