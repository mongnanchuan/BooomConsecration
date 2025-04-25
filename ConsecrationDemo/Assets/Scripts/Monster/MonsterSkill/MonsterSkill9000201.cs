
    using System.Collections.Generic;
    public class MonsterSkill9000201 : MonsterSkillBase
{
        static MonsterSkill9000201()
    {
        MonsterSkillFactory.Register(9000201, typeof(MonsterSkill9000201));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000201);
    }

    public override void GetEffects(List<MonsterTempData> monsData, int playerPos)
    {
        Init();
    }

}