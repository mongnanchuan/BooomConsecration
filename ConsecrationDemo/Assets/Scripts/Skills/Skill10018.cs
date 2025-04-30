using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10018 : SkillBase
{
    //射出1道能量，穿过所有单位造成2点伤害
    static Skill10018()
    {
        SkillFactory.Register(10018, typeof(Skill10018));
    }
    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10018);
    }
    public override List<Effect> GetEffects()
    {
        List<Effect> effects = new List<Effect>();
        Attribute attrP = PlayerPosReport.Instance.attr;

        bool dir = attrP.GetComponent<PlayerManager>().isToRight;
        List<Attribute> takers = new List<Attribute>();

        int tempPos = attrP.PosNow;
        while (tempPos <= 8 && tempPos >= 0)
        {
            tempPos = dir ? tempPos + 1 : tempPos - 1;
            foreach (var mons in MonsterManager.Instance.currentMonstersData.Values)
            {
                if (mons.pos == tempPos)
                    takers.Add(mons.obj.GetComponent<Attribute>());
            }
        }

        if (takers.Count != 0)
        {
            foreach (var mons in takers)
            {
                Effect effect1 = new Effect()
                {
                    type = Effect_Type.MakeDamage,
                    Taker = mons,
                    Ganker = attrP,
                    damage = skill.damage
                };
                effects.Add(effect1);
            }
        }

        return effects;
    }

}
