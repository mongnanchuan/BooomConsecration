using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10020 : SkillBase
{
    //与前方第一个单位交换位置
    static Skill10020()
    {
        SkillFactory.Register(10020, typeof(Skill10020));
    }
    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10020);
    }
    public override List<Effect> GetEffects()
    {
        List<Effect> effects = new List<Effect>();
        Attribute attrP = PlayerPosReport.Instance.attr;

        bool dir = attrP.GetComponent<PlayerManager>().isToRight;
        Attribute taker = GetFirstFaceRole(attrP.PosNow,dir);

        if(taker != null)
        {
            int tempM = taker.PosNow;
            int tempP = attrP.PosNow;

            taker.PosNow = -1;
            attrP.PosNow = -1;

            Effect effect1 = new Effect()
            {
                type = Effect_Type.ForceJump,
                Taker = attrP,
                Ganker = attrP,
                portalMovePos = tempM
            };
            effects.Add(effect1);
            Effect effect2 = new Effect()
            {
                type = Effect_Type.ForceJump,
                Taker = taker,
                Ganker = attrP,
                portalMovePos = tempP
            };
            effects.Add(effect2);

            Effect effect3 = new Effect()
            {
                type = Effect_Type.UseSkillIm,
                Taker = attrP,
                Ganker = attrP,
                //portalMovePos = tempP
            };
            effects.Add(effect3);
        }
        else
        {
            TipsManager.Instance.ShowTip("使用失败，前方没有单位");
            PlayerPosReport.Instance.GetComponent<PlayerManager>().useDefeat = true;
            return null;
        }

        return effects;
    }

}
