using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterTempData
{
    public int num;
    public int pos;
    public GameObject obj;
}

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public int levelID;
    public List<int[]> monsterGroupIDs;//关卡表中的3波怪物ID
    public List<int[]> monsterGroupPoss;//关卡表中的3波怪物位置
    public int currentGroupCount;//目前的组数计数

    public List<int> currentGroupID;
    public List<int> currrentGroupPos;

    //目前的怪物数据，key=怪物编号，value=位置（序号），怪物obj
    public Dictionary<int,(int pos,GameObject obj)> currentMonstersData;
    public int count;//编号时用的序号

    //怪物回合行动结束
    public event Action OnPlayerTurnStart;

    public void MonsterGroupInit(int level)
    {
        levelID = level;
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

        SetSummonMonsters(monsterGroupIDs[currentGroupCount].ToList(), monsterGroupPoss[currentGroupCount].ToList());
        currentGroupCount++;
        SetNewMonsters();
    }

    public void SetSummonMonsters(List<int> monsG,List<int> posG)
    {
        currentGroupID = monsG;
        currrentGroupPos = posG;
    }

    public void TimeToNewMonsterGroup()
    {
        // TODO: 实现刷怪提示,实现延迟1回合
        if (currentGroupCount >= 3)
        {
            Debug.Log("怪刷完了要赢了");
            return;
        }
        SetSummonMonsters(monsterGroupIDs[currentGroupCount].ToList(), monsterGroupPoss[currentGroupCount].ToList());
        currentGroupCount++;
        SetNewMonsters();
    }

    public void SetNewMonsters()
    {
        int playerPos = PlayerPosReport.Instance.GetPlayerPos();
        for (int i = 0; i < currentGroupID.Count; i++)
        {
            int monsterID = currentGroupID[i];
            int monsterPos_TempNum = currrentGroupPos[i];

            //Debug.Log(monsterID);
            HashSet<int> currentPosNum = new(currentMonstersData.Values.Select(pos => pos.Item1)); 
            currentPosNum.Add(playerPos);
            int monsterPos_Num = FindClosestPos(monsterPos_TempNum, 9, currentPosNum);

            if (monsterPos_Num == -1)
                return;

            GameObject prefab = Resources.Load<GameObject>($"Monsters/Monster{monsterID}");
            GameObject monster = GameObject.Instantiate(prefab, transform);

            MonsterBase tempBase = monster.GetComponent<MonsterBase>();
            tempBase.Init();
            tempBase.attribute.MoveNewPos(monsterPos_Num);
            tempBase.attribute.healthInit();//初始化血条
            tempBase.OnPosSetted += RecordingPosChange;
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
    }

    private void RecordingPosChange(int monsCount, int newPos)
    {
        if(currentMonstersData.ContainsKey(monsCount))
        currentMonstersData[monsCount] = (newPos, currentMonstersData[monsCount].obj);
    }

    public void StartMonsterTurn(int playerPos)
    {
        StartCoroutine(HandleMonsterTurnCoroutine(playerPos));
    }

    //开始所有怪物回合
    private IEnumerator HandleMonsterTurnCoroutine(int playerPos)
    {
        List<int> toRemove = new List<int>();
        foreach (var mons in currentMonstersData)
        {
            GameObject obj = mons.Value.obj;
            if (obj == null)
            {
                toRemove.Add(mons.Key);
            }
        }
        foreach (int key in toRemove)
            currentMonstersData.Remove(key);

        var currentMonstersList = currentMonstersData.OrderBy(pair => pair.Key).ToList();
        //剩下的怪物按照序号顺序一个一个判断执行内容
        foreach (var item in currentMonstersList)
        {
            if (!item.Value.obj.GetComponent<Attribute>().isDead)
            {
                MonsterBase currentBase = item.Value.obj.GetComponent<MonsterBase>();

                if (currentBase != null)
                    yield return StartCoroutine(currentBase.HandleTurnWithEffect(playerPos));
            }
        }

        if (currentMonstersData.Count <= 0)
            TimeToNewMonsterGroup();

        yield return new WaitForSeconds(0.1f);
        //开始玩家回合
        OnPlayerTurnStart?.Invoke();
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

    public GameObject GetMonsterAtPosition(int pos)
    {
        foreach (var kv in currentMonstersData)
        {
            if (kv.Value.pos == pos)
                return kv.Value.obj;
        }
        return null;
    }

    //怪物的销毁
    public void DestroyMonster(int count)
    {
        if(currentMonstersData.ContainsKey(count))
        {
            Destroy(currentMonstersData[count].obj.GetComponent<MonsterBase>().zone.gameObject);
            HealthBarManager.Instance.RemoveHealthBar(currentMonstersData[count].obj.GetComponent<Attribute>());
            currentMonstersData.Remove(count);
        }
        
    }

}
