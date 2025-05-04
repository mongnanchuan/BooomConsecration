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
            int tempM = taker.PosNow;
            int tempP = attrP.PosNow;

            CombatManager.Instance.ShowFX(0, taker.PosNow);

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
