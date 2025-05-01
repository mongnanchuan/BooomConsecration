using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10015 : SkillBase
{
    //后退1格，将前方第一个单位拉拽到相邻位置
    static Skill10015()
    {
        SkillFactory.Register(10015, typeof(Skill10015));
    }
    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10015);
    }
    public override List<Effect> GetEffects()
    {
        List<Effect> effects = new List<Effect>();
        Attribute attrP = PlayerPosReport.Instance.attr;
        List<int> area = new List<int>();
        bool dir = attrP.GetComponent<PlayerManager>().isToRight;

        int tempPos = GetLastFacePos(attrP.PosNow, !dir);
        Attribute monsAttr = GetFirstFaceRole(attrP.PosNow, dir);

        if(tempPos == attrP.PosNow)
        {
            TipsManager.Instance.ShowTip("使用失败，无法后退");
            PlayerPosReport.Instance.GetComponent<PlayerManager>().useDefeat = true;
            return null;
        }
        else
        {
            Effect effect1 = new Effect()
            {
                type = Effect_Type.ForceJump,
                Taker = attrP,
                Ganker = attrP,
                portalMovePos = dir ? attrP.PosNow - 1 : attrP.PosNow + 1,
            };
            effects.Add(effect1);

            if(monsAttr!= null)
            {
                Effect effect2 = new Effect()
                {
                    type = Effect_Type.ForceMove,
                    Taker = monsAttr,
                    Ganker = attrP,
                    forceMoveDis = attrP.PosNow - monsAttr.PosNow
                };
                effects.Add(effect2);
            }
        }
        return effects;
    }

}
