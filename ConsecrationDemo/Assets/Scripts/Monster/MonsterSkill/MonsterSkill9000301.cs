public class MonsterSkill9000301 : MonsterSkillBase
{

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000301);
    }

}