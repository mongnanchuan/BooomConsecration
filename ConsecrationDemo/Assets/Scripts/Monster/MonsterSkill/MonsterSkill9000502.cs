
    using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill9000502 : MonsterSkillBase
{
    //对目标目前所在位置造成2点伤害
    static MonsterSkill9000502()
    {
        SkillFactory.Register(9000502, typeof(MonsterSkill9000502));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000502);
    }

    public override List<Effect> GetEffects(MonsterBase mons)
    {
        List<Effect> effects = new List<Effect>();
        List<int> area = new List<int>();
        area.Add(mons.posAddjust);

        foreach (var pos in area)
        {
            CombatManager.Instance.ShowFX(1, pos);
        }

        List<Attribute> getHurt = GetRoleInArea(area);

        foreach (var attr in getHurt)
        {
            if(mons.GetComponent<Attribute>() == attr)
            {
                Debug.Log("打自己怪怪的");
                return effects;
            }
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