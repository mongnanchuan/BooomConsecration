
    using System.Collections.Generic;
    public class MonsterSkill9000504 : MonsterSkillBase
{
    //对1/3/5/7造成3点伤害
    static MonsterSkill9000504()
    {
        SkillFactory.Register(9000504, typeof(MonsterSkill9000504));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000504);
    }

    public override List<Effect> GetEffects(MonsterBase mons)
    {
        List<Effect> effects = new List<Effect>();
        
        return effects;
    }

}