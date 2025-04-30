using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkillBase
{
    public MonsterSkillsConfig monsterSkill;

    public virtual void Init() { }

    public virtual List<Effect> GetEffects(MonsterBase mons)
    {
        List<Effect> effects = new List<Effect>();
        return effects;
    }

    public List<Attribute> GetRoleInArea(List<int> area)
    {
        List<Attribute> attrs = new List<Attribute>();
        int playerPos = PlayerPosReport.Instance.GetPlayerPos();
        foreach (var pos in area)
        {
            foreach (var mons in MonsterManager.Instance.currentMonstersData.Values)
            {
                if (pos == mons.pos)
                    attrs.Add(mons.obj.GetComponent<Attribute>());
            }
            if (pos == playerPos)
                attrs.Add(PlayerPosReport.Instance.attr);
        }
        return attrs;
    }

    public Attribute GetFirstFaceRole(int pos, bool isToRight)
    {
        int tempPos = pos;
        int playerPos = PlayerPosReport.Instance.GetPlayerPos();
        while (tempPos <= 8 && tempPos >= 0)
        {
            tempPos = isToRight ? tempPos + 1 : tempPos - 1;
            foreach (var mons in MonsterManager.Instance.currentMonstersData.Values)
            {
                if (mons.pos == tempPos)
                    return mons.obj.GetComponent<Attribute>();
            }
            if (tempPos == playerPos)
                return PlayerPosReport.Instance.attr;
        }
        return null;
    }

    public int GetLastFacePos(int pos, bool isToRight)
    {
        int tempPos = pos;
        int playerPos = PlayerPosReport.Instance.GetPlayerPos();
        while (tempPos <= 8 && tempPos >= 0)
        {
            tempPos = isToRight ? tempPos + 1 : tempPos - 1;
            foreach (var mons in MonsterManager.Instance.currentMonstersData.Values)
            {
                if (mons.pos == tempPos)
                    return isToRight ? tempPos - 1 : tempPos + 1;
            }
            if (tempPos == playerPos)
                return isToRight ? tempPos - 1 : tempPos + 1;
        }
        return isToRight ? tempPos - 1 : tempPos + 1;
    }



}
