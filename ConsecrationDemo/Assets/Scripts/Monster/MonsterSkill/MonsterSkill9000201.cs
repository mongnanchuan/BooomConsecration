
    using System.Collections.Generic;
    public class MonsterSkill9000201 : MonsterSkillBase
{
    //对目标目前所在位置造成2点伤害
    static MonsterSkill9000201()
    {
        SkillFactory.Register(9000201, typeof(MonsterSkill9000201));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000201);
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