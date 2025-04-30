
    using System.Collections.Generic;
    public class MonsterSkill9000301 : MonsterSkillBase
{
    //向前冲刺，对碰撞的第一个单位造成2点伤害
    static MonsterSkill9000301()
    {
        SkillFactory.Register(9000301, typeof(MonsterSkill9000301));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000301);
    }

    public override List<Effect> GetEffects(MonsterBase mons)
    {
        List<Effect> effects = new List<Effect>();
        Attribute taker = GetFirstFaceRole(mons.currentPos, mons.isToRight);

        if (taker != null)
        {
            Effect effect2 = new Effect()
            {
                type = Effect_Type.ForceMove,
                Taker = mons.GetComponent<Attribute>(),
                Ganker = mons.GetComponent<Attribute>(),
                forceMoveDis = taker.PosNow - mons.currentPos + (mons.isToRight ? -1 : 1)
            };
            effects.Add(effect2);
            Effect effect1 = new Effect()
            {
                type = Effect_Type.MakeDamage,
                Taker = taker,
                Ganker = mons.GetComponent<Attribute>(),
                damage = monsterSkill.attact
            };
            effects.Add(effect1);
        }
        return effects;
    }

}