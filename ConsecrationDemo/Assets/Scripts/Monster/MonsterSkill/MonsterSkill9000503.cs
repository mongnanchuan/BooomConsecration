
    using System.Collections.Generic;
    public class MonsterSkill9000503 : MonsterSkillBase
{
    //向前射出能量，对第一个单位造成3点伤害
    static MonsterSkill9000503()
    {
        SkillFactory.Register(9000503, typeof(MonsterSkill9000503));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000503);
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