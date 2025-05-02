using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10008 : SkillBase
{
    //血祭相邻1格的2个技能
    static Skill10008()
    {
        SkillFactory.Register(10008, typeof(Skill10008));
    }
    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10008);
    }
    public override List<Effect> GetEffects()
    {
        List<Effect> effects = new List<Effect>();
        Attribute attrP = PlayerPosReport.Instance.attr;
        List<int> area = new List<int>();
        //bool dir = attrP.GetComponent<PlayerManager>().isToRight;
        for (int i = 0; i < skill.range + rangeOffset; i++)
        {
            int tempPos1 = attrP.PosNow + i + 1;
            int tempPos2 = attrP.PosNow - i - 1;
            
            if (tempPos1 <= 8 && tempPos1 >= 0)
                area.Add(tempPos1);

            if (tempPos2 <= 8 && tempPos2 >= 0)
                area.Add(tempPos2);
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
