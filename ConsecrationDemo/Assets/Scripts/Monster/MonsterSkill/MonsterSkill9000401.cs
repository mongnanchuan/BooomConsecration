
    using System.Collections.Generic;
    public class MonsterSkill9000401 : MonsterSkillBase
{
    //向前射出能量，对第一个单位造成2点伤害
    static MonsterSkill9000401()
    {
        SkillFactory.Register(9000401, typeof(MonsterSkill9000401));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000401);
    }

    public override List<Effect> GetEffects(MonsterBase mons)
    {
        List<Effect> effects = new List<Effect>();

        Attribute attr = mons.GetComponent<Attribute>();
        Attribute taker = GetFirstFaceRole(attr.PosNow, mons.isToRight);

        if (taker != null)
        {
            Effect effect1 = new Effect()
            {
                type = Effect_Type.MakeDamage,
                Taker = taker,
                Ganker = attr,
                damage = monsterSkill.attact
            };
            effects.Add(effect1);
        }
        return effects;
    }

}