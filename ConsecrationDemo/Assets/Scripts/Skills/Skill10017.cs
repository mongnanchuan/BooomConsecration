using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10017 : SkillBase
{
    //射出1道能量，对第一个单位造成2点伤害
    static Skill10017()
    {
        SkillFactory.Register(10017, typeof(Skill10017));
    }
    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10017);
    }
    public override List<Effect> GetEffects()
    {
        List<Effect> effects = new List<Effect>();
        Attribute attrP = PlayerPosReport.Instance.attr;

        bool dir = attrP.GetComponent<PlayerManager>().isToRight;
        Attribute taker = GetFirstFaceRole(attrP.PosNow,dir);

        if(taker != null)
        {
            Effect effect1 = new Effect()
            {
                type = Effect_Type.MakeDamage,
                Taker = taker,
                Ganker = attrP,
                damage = skill.damage
            };
            effects.Add(effect1);
        }

        return effects;
    }

}
