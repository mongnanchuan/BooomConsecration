using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkillBase
{
    public MonsterSkillsConfig monsterSkill;
    public int playerPos;
    public List<int> monsterPosGroup;

    public virtual void Init() { }

    public virtual List<Effect> GetEffects(List<MonsterTempData> monsData, MonsterBase mons, int playerPos)
    {
        List<Effect> effects = new List<Effect>();
        return effects;
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
