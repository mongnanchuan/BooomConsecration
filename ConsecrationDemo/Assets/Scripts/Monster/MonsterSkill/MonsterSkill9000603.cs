
    using System.Collections.Generic;
    public class MonsterSkill9000603 : MonsterSkillBase
{
    //��1/3/5/7���3���˺�
    static MonsterSkill9000603()
    {
        SkillFactory.Register(9000603, typeof(MonsterSkill9000603));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000603);
    }

    public override List<Effect> GetEffects(MonsterBase mons)
    {
        List<Effect> effects = new List<Effect>();
        List<int> area = new List<int>();

        for (int i = 0; i < monsterSkill.posPar.Length; i++)
        {
            area.Add(monsterSkill.posPar[i]);
        }

        foreach (var pos in area)
        {
            CombatManager.Instance.ShowFX(1, pos);
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