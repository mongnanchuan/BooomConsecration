
    using System.Collections.Generic;
    public class MonsterSkill9000301 : MonsterSkillBase
{
        static MonsterSkill9000301()
    {
        MonsterSkillFactory.Register(9000301, typeof(MonsterSkill9000301));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000301);
    }

    public override void GetEffects(List<MonsterTempData> monsData, int playerPos)
    {
        Init();
    }

}