using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10019 : SkillBase
{
    //与前方第一个单位交换位置
    static Skill10019()
    {
        SkillFactory.Register(10019, typeof(Skill10019));
    }
    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10019);
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
                type = Effect_Type.ForceJump,
                Taker = attrP,
                Ganker = attrP,
                portalMovePos = taker.PosNow
            };
            effects.Add(effect1);
            Effect effect2 = new Effect()
            {
                type = Effect_Type.ForceJump,
                Taker = taker,
                Ganker = attrP,
                portalMovePos = attrP.PosNow
            };
            effects.Add(effect2);
        }
        else
        {
            TipsManager.Instance.ShowTip("使用失败，前方没有单位");
            return null;
        }

        return effects;
    }

}
