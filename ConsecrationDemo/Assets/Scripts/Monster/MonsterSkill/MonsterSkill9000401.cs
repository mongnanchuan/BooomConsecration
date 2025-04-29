
    using System.Collections.Generic;
    public class MonsterSkill9000401 : MonsterSkillBase
{
        static MonsterSkill9000401()
    {
        SkillFactory.Register(9000401, typeof(MonsterSkill9000401));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000401);
    }

    public override List<Effect> GetEffects(List<MonsterTempData> monsData, MonsterBase mons, int playerPos)
    {
        Init();
        List<Effect> effects = new List<Effect>();
        return effects;
    }

}