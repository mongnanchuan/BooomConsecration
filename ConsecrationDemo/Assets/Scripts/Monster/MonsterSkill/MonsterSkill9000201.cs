
    using System.Collections.Generic;
    public class MonsterSkill9000201 : MonsterSkillBase
{
        static MonsterSkill9000201()
    {
        SkillFactory.Register(9000201, typeof(MonsterSkill9000201));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000201);
    }

    public override List<Effect> GetEffects(List<MonsterTempData> monsData, MonsterBase mons, int playerPos)
    {
        Init();
        List<Effect> effects = new List<Effect>();
        return effects;
    }

}