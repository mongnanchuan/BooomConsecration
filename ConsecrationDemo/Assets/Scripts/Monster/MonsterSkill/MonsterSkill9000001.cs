using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill9000001 : MonsterSkillBase
{
    //对前方1格造成2点伤害
    static MonsterSkill9000001()
    {
        SkillFactory.Register(9000001, typeof(MonsterSkill9000001));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000001);
    }

    public override List<Effect> GetEffects(MonsterBase mons)
    {
        List<Effect> effects = new List<Effect>();
        List<int> area = new List<int>();
        for (int i = 0; i < monsterSkill.rangePar; i++)
        {
            int tempNum = mons.isToRight ? mons.currentPos + i + 1 : mons.currentPos - i - 1;
            area.Add(tempNum);
        }
        List<Attribute> getHurt = GetRoleInArea(area);

        foreach (var attr in getHurt)
        {
            Effect effect1 = new Effect()
            {
                type = Effect_Type.MakeDamage,
                Taker = attr,
                Ganker = mons.GetComponent<Attribute>(),
                damage = monsterSkill.attact
            };
            effects.Add(effect1);
        }

        return effects;
    }

}