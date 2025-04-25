
    using System.Collections.Generic;
    public class MonsterSkill9000501 : MonsterSkillBase
{
        static MonsterSkill9000501()
    {
        MonsterSkillFactory.Register(9000501, typeof(MonsterSkill9000501));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000501);
    }

    public override void GetEffects(List<MonsterTempData> monsData, int playerPos)
    {
        Init();
    }

}