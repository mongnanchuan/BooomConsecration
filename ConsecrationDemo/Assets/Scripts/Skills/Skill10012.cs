using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10012 : SkillBase
{
    //��ǰ�ƶ�1�񣬻��1������Ļغ�
    static Skill10012()
    {
        SkillFactory.Register(10012, typeof(Skill10012));
    }
    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10012);
    }
    public override List<Effect> GetEffects()
    {
        List<Effect> effects = new List<Effect>();
        Attribute attrP = PlayerPosReport.Instance.attr;
        List<int> area = new List<int>();
        bool dir = attrP.GetComponent<PlayerManager>().isToRight;

        int tempPos = GetLastFacePos(attrP.PosNow, dir);

        if(tempPos == attrP.PosNow)
        {
            TipsManager.Instance.ShowTip("ʹ��ʧ�ܣ��޷�ǰ��");
            return null;
        }
        else
        {
            Effect effect1 = new Effect()
            {
                type = Effect_Type.ForceJump,
                Taker = attrP,
                Ganker = attrP,
                portalMovePos = dir ? attrP.PosNow + 1 : attrP.PosNow - 1,
            };
            effects.Add(effect1);
            attrP.additionalTurn += 2;
        }

        return effects;
    }

}
