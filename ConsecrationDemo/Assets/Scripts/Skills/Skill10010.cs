using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10010 : SkillBase
{
    //对前方和后方1格造成3点伤害
    static Skill10010()
    {
        SkillFactory.Register(10010, typeof(Skill10010));
    }
    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10010);
    }
    public override List<Effect> GetEffects()
    {
        List<Effect> effects = new List<Effect>();
        Attribute attrP = PlayerPosReport.Instance.attr;
        List<int> area = new List<int>();
        bool dir = attrP.GetComponent<PlayerManager>().isToRight;
        for (int i = 0; i < skill.range + rangeOffset; i++)
        {
            area.Add(attrP.PosNow + i + 1);
            area.Add(attrP.PosNow - i - 1);
        }

        foreach (var pos in area)
        {
            CombatManager.Instance.ShowFX(0, pos);
        }

        List<Attribute> taker = new List<Attribute>(); 
        taker = GetRoleInArea(area);

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
