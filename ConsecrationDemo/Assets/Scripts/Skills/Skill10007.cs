using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10007 : SkillBase
{
    //血祭前方1格的技能
    static Skill10007()
    {
        SkillFactory.Register(10007, typeof(Skill10007));
    }
    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10007);
    }
    public override List<Effect> GetEffects()
    {
        List<Effect> effects = new List<Effect>();
        Attribute attrP = PlayerPosReport.Instance.attr;
        List<int> area = new List<int>();
        bool dir = attrP.GetComponent<PlayerManager>().isToRight;
        for (int i = 0; i < skill.range + rangeOffset; i++)
        {
            int tempPos = dir ? attrP.PosNow + i + 1 : attrP.PosNow - i - 1;
            if (tempPos > 8 || tempPos < 0)
                continue;

            area.Add(tempPos);
        }

        foreach (var pos in area)
        {
            CombatManager.Instance.ShowFX(0, pos);
        }

        Effect effect1 = new Effect()
        {
            type = Effect_Type.Sacrificing,
            Taker = attrP,
            Ganker = attrP,
            sacrificeID = area
        };
        effects.Add(effect1);

        return effects;
    }

}
