using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10013 : SkillBase
{
    //将相邻单位击退1格
    static Skill10013()
    {
        SkillFactory.Register(10013, typeof(Skill10013));
    }
    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10013);
    }
    public override List<Effect> GetEffects()
    {
        List<Effect> effects = new List<Effect>();
        Attribute attrP = PlayerPosReport.Instance.attr;
        List<int> area = new List<int>();
        bool dir = attrP.GetComponent<PlayerManager>().isToRight;
        for (int i = 0; i < skill.range; i++)
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
                type = Effect_Type.ForceMove,
                Taker = attr,
                Ganker = attrP,
                forceMoveDis = attr.PosNow - attrP.PosNow
            };
            effects.Add(effect1);
        }


        return effects;
    }

}
