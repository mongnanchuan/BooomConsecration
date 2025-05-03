
    using System.Collections.Generic;
    public class MonsterSkill9000501 : MonsterSkillBase
{
    //对相邻1格造成3点伤害并击退1格
    static MonsterSkill9000501()
    {
        SkillFactory.Register(9000501, typeof(MonsterSkill9000501));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000501);
    }

    public override List<Effect> GetEffects(MonsterBase mons)
    {
        List<Effect> effects = new List<Effect>();
        Attribute attr = mons.GetComponent<Attribute>();
        List<int> area = new List<int>();
        
        for (int i = 0; i < monsterSkill.rangePar; i++)
        {
            area.Add(attr.PosNow + i + 1);
            area.Add(attr.PosNow - i - 1);
        }

        List<Attribute> taker = new List<Attribute>();
        taker = GetRoleInArea(area);

        foreach (var pos in area)
        {
            CombatManager.Instance.ShowFX(1, pos);
        }

        foreach (var obj in taker)
        {
            Effect effect1 = new Effect()
            {
                type = Effect_Type.MakeDamage,
                Taker = obj,
                Ganker = attr,
                damage = monsterSkill.attact
            };
            effects.Add(effect1);
            Effect effect2 = new Effect()
            {
                type = Effect_Type.ForceMove,
                Taker = obj,
                Ganker = attr,
                forceMoveDis = obj.PosNow - attr.PosNow
            };
            effects.Add(effect2);
        }
        return effects;
    }

}