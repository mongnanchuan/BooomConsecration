using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkillBase
{
    public MonsterSkillsConfig monsterSkill;
    public int playerPos;
    public List<int> monsterPosGroup;

    public virtual void Init() { }

    public virtual List<Effect> GetEffects()
    {
        List<Effect> effects = new List<Effect>();
        return effects;
    }

    public void GetRolePos(int playerPos, List<int> monstersPos)
    {
        this.playerPos = playerPos;
        monsterPosGroup = monstersPos;
    }

}
