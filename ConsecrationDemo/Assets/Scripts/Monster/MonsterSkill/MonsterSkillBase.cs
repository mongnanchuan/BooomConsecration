using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkillBase
{
    public MonsterSkillsConfig monsterSkill;
    public int playerPos;

    public virtual void Init() { }

    public virtual List<Effect> GetEffects(List<MonsterTempData> monsData, MonsterBase mons, int playerPos)
    {
        List<Effect> effects = new List<Effect>();
        return effects;
    }

    public List<Attribute> GetRoleInArea(List<int> area)
    {
        List<Attribute> attrs = new List<Attribute>();
        int playerPos = PlayerPosReport.Instance.attr.PosNow;
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



}
