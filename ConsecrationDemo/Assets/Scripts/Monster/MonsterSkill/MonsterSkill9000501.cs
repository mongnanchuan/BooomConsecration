
    using System.Collections.Generic;
    public class MonsterSkill9000501 : MonsterSkillBase
{
        static MonsterSkill9000501()
    {
        SkillFactory.Register(9000501, typeof(MonsterSkill9000501));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000501);
    }

    public override List<Effect> GetEffects(List<MonsterTempData> monsData, MonsterBase mons, int playerPos)
    {
        Init();
        List<Effect> effects = new List<Effect>();
        return effects;
    }

}