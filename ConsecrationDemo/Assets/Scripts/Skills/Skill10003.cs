using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10003 : SkillBase
{
    //向前冲撞，对第一个单位造成2点伤害
    static Skill10003()
    {
        SkillFactory.Register(10003, typeof(Skill10003));
    }
    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10003);
    }
    public override List<Effect> GetEffects()
    {
        List<Effect> effects = new List<Effect>();
        Attribute attrP = PlayerPosReport.Instance.attr;

        bool dir = attrP.GetComponent<PlayerManager>().isToRight;
        Attribute taker = GetFirstFaceRole(attrP.PosNow,dir);

        if(taker != null)
        {
            Effect effect2 = new Effect()
            {
                type = Effect_Type.ForceMove,
                Taker = attrP,
                Ganker = attrP,
                forceMoveDis = taker.PosNow - attrP.PosNow + (dir ? -1 : 1)
            };
            effects.Add(effect2);
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
