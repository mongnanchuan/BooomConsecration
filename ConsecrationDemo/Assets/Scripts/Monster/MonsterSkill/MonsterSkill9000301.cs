
    using System.Collections.Generic;
    public class MonsterSkill9000301 : MonsterSkillBase
{
        static MonsterSkill9000301()
    {
        SkillFactory.Register(9000301, typeof(MonsterSkill9000301));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000301);
    }

    public override List<Effect> GetEffects(List<MonsterTempData> monsData, MonsterBase mons, int playerPos)
    {
        Init();
        List<Effect> effects = new List<Effect>();
        return effects;
    }

}