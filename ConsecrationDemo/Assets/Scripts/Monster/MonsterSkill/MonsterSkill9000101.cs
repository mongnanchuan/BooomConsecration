using System.Collections.Generic;

public class MonsterSkill9000101 : MonsterSkillBase
{
    static MonsterSkill9000101()
    {
        MonsterSkillFactory.Register(9000101, typeof(MonsterSkill9000101));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000101);
    }

    public override void GetEffects(List<MonsterTempData> monsData, int playerPos)
    {
        Init();
    }

}