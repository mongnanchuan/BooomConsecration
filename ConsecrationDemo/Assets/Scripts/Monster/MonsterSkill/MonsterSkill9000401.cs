
    using System.Collections.Generic;
    public class MonsterSkill9000401 : MonsterSkillBase
{
        static MonsterSkill9000401()
    {
        MonsterSkillFactory.Register(9000401, typeof(MonsterSkill9000401));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000401);
    }

    public override void GetEffects(List<MonsterTempData> monsData, int playerPos)
    {
        Init();
    }

}