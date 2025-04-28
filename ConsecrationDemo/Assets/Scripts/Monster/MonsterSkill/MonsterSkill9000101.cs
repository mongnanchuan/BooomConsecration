using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill9000101 : MonsterSkillBase
{
    static MonsterSkill9000101()
    {
        MonsterSkillFactory.Register(9000101, typeof(MonsterSkill9000101));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000101);
    }

    public override List<Effect> GetEffects(List<MonsterTempData> monsData, MonsterBase mons, int playerPos)
    {
        Init();
        List<Effect> effects = new List<Effect>();
        List<int> getHurt = new List<int>();
        for (int i = 0; i < monsterSkill.rangePar; i++)
        {
            int tempNum = mons.isToRight ? mons.currentPos + i + 1 : mons.currentPos - i - 1;
            getHurt.Add(tempNum);
        }
        foreach (var item in monsData)
        {
            foreach (var hurtPos in getHurt)
            {
                if(item.pos==hurtPos)
                {
                    Effect effect1 = new Effect()
                    {
                        type = Effect_Type.MakeDamage,
                        Taker = item.obj.GetComponent<Attribute>(),
                        Ganker = mons.GetComponent<Attribute>(),
                        damage = monsterSkill.attact
                    };
                    effects.Add(effect1);

                    Effect effect2 = new Effect()
                    {
                        type = Effect_Type.ForceMove,
                        Taker = item.obj.GetComponent<Attribute>(),
                        Ganker = mons.GetComponent<Attribute>(),
                        forceMoveDis = mons.isToRight ? 2 : -2
                    };
                    effects.Add(effect2);
                }
                if (playerPos == hurtPos)
                {
                    Effect effect1 = new Effect()
                    {
                        type = Effect_Type.MakeDamage,
                        Taker = PlayerPosReport.Instance.gameObject.GetComponent<Attribute>(),
                        Ganker = mons.GetComponent<Attribute>(),
                        damage = monsterSkill.attact
                    };
                    effects.Add(effect1);

                    Effect effect2 = new Effect()
                    {
                        type = Effect_Type.ForceMove,
                        Taker = PlayerPosReport.Instance.gameObject.GetComponent<Attribute>(),
                        Ganker = mons.GetComponent<Attribute>(),
                        forceMoveDis = mons.isToRight ? 2 : -2
                    };
                    effects.Add(effect2);
                }
            }
        }

        return effects;
    }

}