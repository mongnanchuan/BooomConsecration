using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10002 : SkillBase
{
    //对前方2格造成3点伤害
    static Skill10002()
    {
        SkillFactory.Register(10002, typeof(Skill10002));
    }
    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10002);
    }
    public override List<Effect> GetEffects()
    {
        List<Effect> effects = new List<Effect>();
        Attribute attrP = PlayerPosReport.Instance.attr;
        List<int> area = new List<int>();
        bool dir = attrP.GetComponent<PlayerManager>().isToRight;
        for (int i = 0; i < skill.range + rangeOffset; i++)
        {
            area.Add(dir ? attrP.PosNow + i + 1 : attrP.PosNow - i - 1);
        }

        List<Attribute> taker = new List<Attribute>();
        taker = GetRoleInArea(area);
        foreach (var pos in area)
        {
            CombatManager.Instance.ShowFX(0, pos);
        }
        foreach (var attr in taker)
        {
            Effect effect1 = new Effect()
            {
                type = Effect_Type.MakeDamage,
                Taker = attr,
                Ganker = attrP,
                damage = skill.damage
            };
            effects.Add(effect1);
        }

        return effects;
    }

}
