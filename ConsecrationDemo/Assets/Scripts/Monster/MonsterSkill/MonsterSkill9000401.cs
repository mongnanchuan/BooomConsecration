public class MonsterSkill9000401 : MonsterSkillBase
{

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000401);
    }

}