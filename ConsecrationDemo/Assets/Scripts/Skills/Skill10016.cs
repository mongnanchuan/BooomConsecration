using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10016 : SkillBase
{
    //����1�񣬽�ǰ����һ����λ��ק������λ�ã�֮������ʹ�ü���
    static Skill10016()
    {
        SkillFactory.Register(10016, typeof(Skill10016));
    }
    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10016);
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
            TipsManager.Instance.ShowTip("ʹ��ʧ�ܣ��޷�����");
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

            Effect effect3 = new Effect()
            {
                type = Effect_Type.UseSkillIm,
                Taker = attrP,
                Ganker = attrP,
                //forceMoveDis = attrP.PosNow - monsAttr.PosNow
            };
            effects.Add(effect3);
        }
        return effects;
    }

}
