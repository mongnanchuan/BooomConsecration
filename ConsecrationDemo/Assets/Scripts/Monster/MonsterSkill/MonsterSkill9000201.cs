public class MonsterSkill9000201 : MonsterSkillBase
{

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000201);
    }

}