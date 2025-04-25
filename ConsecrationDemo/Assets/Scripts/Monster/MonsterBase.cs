using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterBase : MonoBehaviour
{
    public MonstersConfig monster;
    public Attribute attribute;
    public int count;//���
    public int currentPos;//��ǰλ�ñ��

    public int skillCount;//������
    public int currentSkillID;//��ǰ����ID
    public int currentSkillCount;//��ǰ�������
    public bool isToRight = false;//����

    public bool isOnUse;//�Ƿ�������

    public Transform zone;//װ��в��Χ��ʾ�ĸ��ڵ�
    public GameObject warningZone1;//��в����1��ʾ
    public GameObject warningZone2;//��в����2��ʾ
    public GameObject warningZone3;//��в����3��ʾ
    public GameObject warningZone4;//��в����4��ʾ
    public Vector2 offset;//��в����3/4���λ�õ���ֵ

    //λ�øĶ����֪ͨ
    public event Action<int, int> OnPosSetted;

    public virtual void Init() { }

    //�غϿ�ʼ�����ж�
    public void StartTurn(List<MonsterTempData> monsterData,int playerPos)
    {
        if(isOnUse)
        {
            //Todo:
            //���ܴ���Ч��
            MonsterSkillBase useSkill = MonsterSkillFactory.Create(currentSkillID);
            if(useSkill!=null)
            {
                useSkill.GetEffects(monsterData, playerPos);
            }

            for (int i = 0; i < zone.childCount; i++)
            {
                GameObject.DestroyImmediate(zone.GetChild(i).gameObject);
            }

            currentSkillID = 0;
            isOnUse = false;
            if (currentSkillCount + 1 >= skillCount)
                currentSkillCount = 0;
            else
                currentSkillCount++;

            return;
        }
        else
        {
            if (playerPos > currentPos)
                isToRight = true;
            else
                isToRight = false;

            SetDir();
            
            bool isBeBlock = false;
            int min = Mathf.Min(currentPos, playerPos);
            int max = Mathf.Max(currentPos, playerPos);
            currentSkillID = monster.skillGroup[currentSkillCount];

            for (int i = 0; i < monsterData.Count; i++)
            {
                if (monsterData[i].pos > min && monsterData[i].pos < max)
                    isBeBlock = true;
            }

            var skill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(currentSkillID);
            switch (skill.attactType)
            {
                case 1:
                    if(Mathf.Abs(currentPos-playerPos)<= skill.rangePar)
                    {
                        ReadyToUseSkill(currentSkillID, playerPos);
                        return;
                    }
                    break;
                case 2:
                    ReadyToUseSkill(currentSkillID, playerPos);
                    return;
                case 3:
                    if (isBeBlock)
                        return;
                    ReadyToUseSkill(currentSkillID, playerPos);
                    return;
                case 4:
                    if (isBeBlock)
                        return;
                    ReadyToUseSkill(currentSkillID, playerPos);
                    return;
                default:
                    break;
            }
            //���ż���Ҳ���������ƶ�
            if (isBeBlock)
                return;

            int[] monstersPos = new int[monsterData.Count];
            for (int i = 0; i < monsterData.Count; i++)
            {
                monstersPos[i] = monsterData[i].pos;
            }

            if(monster.moveType == 0)
            {
                int newPos = Move(monstersPos, playerPos);
                attribute.MoveNewPos(newPos);
            }
            return;
        }
    }

    //��������
    public void ReadyToUseSkill(int skillID,int playerPos)
    {
        var skill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(skillID);
        int type = skill.attactType;
        List<int> waringPoss = new List<int>();
        isOnUse = true;
        switch (type)
        {
            case 1:
                if(skill.rangeDir==1)
                {
                    for (int i = 0; i < skill.rangePar; i++)
                    {
                        waringPoss.Add(isToRight? currentPos + 1 + i: currentPos - 1 - i);
                    }
                    foreach (var num in waringPoss)
                    {
                        GameObject.Instantiate(warningZone1,SwitchPos.IntToVector2(num), Quaternion.identity, zone);
                    }
                }
                else if(skill.rangeDir==2)
                {
                    for (int i = 0; i < skill.rangePar; i++)
                    {
                        waringPoss.Add(currentPos + 1 + i);
                        waringPoss.Add(currentPos - 1 - i);
                    }
                    foreach (var num in waringPoss)
                    {
                        GameObject.Instantiate(warningZone1, SwitchPos.IntToVector2(num), Quaternion.identity, zone);
                    }
                }
                break;
            case 2:
                if (skill.posPar[0] == -1)
                {
                    GameObject.Instantiate(warningZone2, SwitchPos.IntToVector2(playerPos), Quaternion.identity, zone);
                }
                else
                {
                    for (int i = 0; i < skill.posPar.Length; i++)
                    {
                        waringPoss.Add(skill.posPar[i]);
                    }
                    foreach (var num in waringPoss)
                    {
                        GameObject.Instantiate(warningZone2, SwitchPos.IntToVector2(num), Quaternion.identity, zone);
                    }
                }
                break;
            case 3:
                GameObject zone3 = GameObject.Instantiate(warningZone3, zone);
                if(isToRight)
                    zone3.transform.localPosition = offset;
                else
                    zone3.transform.localPosition = new Vector2(-offset.x,offset.y);
                break;
            case 4:
                GameObject zone4 = GameObject.Instantiate(warningZone3, zone);
                if (isToRight)
                    zone4.transform.localPosition = offset;
                else
                    zone4.transform.localPosition = new Vector2(-offset.x, offset.y);
                break;
            default:
                break;
        }

    }

    //�ƶ�����
    public int Move(int[] monstersPos, int playerPos)
    {
        if(currentPos > playerPos)
        {
            for (int i = 0; i < monstersPos.Length; i++)
            {
                if (monstersPos[i] == currentPos - 1)
                    return currentPos;
            }
            return currentPos - 1;
        }
        else
        {
            for (int i = 0; i < monstersPos.Length; i++)
            {
                if (monstersPos[i] == currentPos + 1)
                    return currentPos;       
            }
            return currentPos + 1;
        }
    }

    //���Ƴ���
    public void SetDir()
    {
        Vector3 dir = transform.localScale;
        if (isToRight)
            transform.localScale = new Vector3(-Mathf.Abs(dir.x), dir.y, dir.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(dir.x), dir.y, dir.z);
    }

    //����λ�ò��㱨
    public void OnPosBeSet(int posNum)
    {
        currentPos = posNum;
        OnPosSetted?.Invoke(count, currentPos);
    }

}
