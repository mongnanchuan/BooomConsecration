using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10004 : SkillBase
{
    //向前冲撞，对第一个角色造成3点伤害并击退1格
    static Skill10004()
    {
        SkillFactory.Register(10004, typeof(Skill10004));
    }
    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10004);
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
            Effect effect2 = new Effect()
            {
                type = Effect_Type.ForceMove,
                Taker = attrP,
                Ganker = attrP,
                forceMoveDis = taker.PosNow - attrP.PosNow + (dir?-1:1)
            };
            effects.Add(effect2);
            Effect effect3 = new Effect()
            {
                type = Effect_Type.ForceMove,
                Taker = taker,
                Ganker = attrP,
                forceMoveDis = dir ? 1 : -1
            };
            effects.Add(effect3);
        }

        return effects;
    }

}
