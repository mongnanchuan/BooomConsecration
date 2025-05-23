using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10000 : SkillBase
{
    //对前方1格造成1点伤害
    static Skill10000()
    {
        SkillFactory.Register(10000, typeof(Skill10000));
    }

    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10000);
    }
    public override List<Effect> GetEffects()
    {
        List<Effect> effects = new List<Effect>();
        Attribute attrP = PlayerPosReport.Instance.attr;
        List<int> area = new List<int>();
        bool dir = attrP.GetComponent<PlayerManager>().isToRight;
        for (int i = 0; i < skill.range; i++)
        {
            area.Add(dir ? attrP.PosNow + i+1 : attrP.PosNow - i-1);
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
            //Debug.Log(skill.damage);
        }

        return effects;
    }

}
