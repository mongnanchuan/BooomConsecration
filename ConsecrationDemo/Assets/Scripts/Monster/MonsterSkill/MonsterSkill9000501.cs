public class MonsterSkill9000501 : MonsterSkillBase
{

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000501);
    }

}