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
    public List<int[]> monsterGroupIDs;//�ؿ����е�3������ID
    public List<int[]> monsterGroupPoss;//�ؿ����е�3������λ��
    public int currentGroupCount;//Ŀǰ����������

    //Ŀǰ�Ĺ������ݣ�key=�����ţ�value=λ�ã���ţ�������obj
    public Dictionary<int,(int pos,GameObject obj)> currentMonstersData;
    public int count;//���ʱ�õ����

    //����غ��ж�����
    public event Action OnPlayerTurnStart;

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
        //��õ�ǰ��ID
        int[] currentGroupID = monsterGroupIDs[currentGroupCount];
        //��õ�ǰ��λ��
        int[] currrentGroupPos = monsterGroupPoss[currentGroupCount];

        for (int i = 0; i < currentGroupID.Length; i++)
        {
            int monsterID = currentGroupID[i];
            int monsterPos_TempNum = currrentGroupPos[i];

            HashSet<int> currentPosNum = new(currentMonstersData.Values.Select(pos => pos.Item1)); 
            currentPosNum.Add(playerPos);
            int monsterPos_Num = FindClosestPos(monsterPos_TempNum, 9, currentPosNum);

            GameObject prefab = Resources.Load<GameObject>($"Monsters/Monster{monsterID}");
            GameObject monster = GameObject.Instantiate(prefab, transform);
            monster.GetComponent<Attribute>().MoveNewPos(monsterPos_Num);

            MonsterBase tempBase = monster.GetComponent<MonsterBase>();
            tempBase.Init();
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
        currentGroupCount++;
    }

    private void RecordingPosChange(int monsCount, int newPos)
    {
        currentMonstersData[monsCount] = (newPos, currentMonstersData[monsCount].obj);
    }

    public void StartMonsterTurn(int playerPos)
    {
        StartCoroutine(HandleMonsterTurnCoroutine(playerPos));
    }

    //��ʼ���й���غ�
    private IEnumerator HandleMonsterTurnCoroutine(int playerPos)
    {
        //ɾ�����Ѿ�û�еĹ���
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
            currentMonstersData.Remove(key);

        //ʣ�µĹ��ﰴ�����˳��һ��һ���ж�ִ������
        foreach (var item in currentMonstersData.OrderBy(pair => pair.Key))
        {
            MonsterBase currentBase = item.Value.obj.GetComponent<MonsterBase>();
            yield return StartCoroutine(currentBase.HandleTurnWithEffect(playerPos));
        }
        //��ʼ��һغ�
        OnPlayerTurnStart?.Invoke();
    }

    //ˢ��ʱ����ռλ�ظ�
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
        Debug.LogError("�Ҳ������õ�λ���ˣ�");
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
}
