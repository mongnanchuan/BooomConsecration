
    using System.Collections.Generic;
    public class MonsterSkill9000502 : MonsterSkillBase
{
    //对0/2/4/6/8造成3点伤害
    static MonsterSkill9000502()
    {
        SkillFactory.Register(9000502, typeof(MonsterSkill9000502));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000502);
    }

    public override List<Effect> GetEffects(MonsterBase mons)
    {
        List<Effect> effects = new List<Effect>();
        List<int> area = new List<int>();

        for (int i = 0; i < monsterSkill.posPar.Length; i++)
        {
            area.Add(monsterSkill.posPar[i]);
        }

        List<Attribute> getHurt = GetRoleInArea(area);

        foreach (var attr in getHurt)
        {
            if (attr == mons.GetComponent<Attribute>())
                continue;
            Effect effect1 = new Effect()
            {
                type = Effect_Type.MakeDamage,
                Taker = attr,
                Ganker = mons.GetComponent<Attribute>(),
                damage = monsterSkill.attact
            };
            effects.Add(effect1);
        }
        return effects;
    }

}