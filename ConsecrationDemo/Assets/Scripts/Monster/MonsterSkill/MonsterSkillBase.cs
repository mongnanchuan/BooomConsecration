using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkillBase
{
    public MonsterSkillsConfig monsterSkill;
    public int playerPos;
    public List<int> monsterPosGroup;

    public virtual void Init() { }

    public virtual void GetEffects(List<MonsterTempData> monsData, int playerPos)
    {
        List<Effect> effects = new List<Effect>();
        return;
    }

    public void SkillTypeDeal(int id)
    {
        var skill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(id);
        switch (skill.attactType)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }
    }



}
