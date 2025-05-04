using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10006 : SkillBase
{
    //吸收相邻单位各1点生命值
    static Skill10006()
    {
        SkillFactory.Register(10006, typeof(Skill10006));
    }
    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10006);
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
            Effect effect2 = new Effect()
            {
                type = Effect_Type.Healing,
                Taker = attrP,
                Ganker = attrP,
                heal = skill.damage
            };
            effects.Add(effect2);
        }

        return effects;
    }

}
