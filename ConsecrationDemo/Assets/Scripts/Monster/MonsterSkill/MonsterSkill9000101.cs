public class MonsterSkill9000101 : MonsterSkillBase
{

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000101);
    }

}