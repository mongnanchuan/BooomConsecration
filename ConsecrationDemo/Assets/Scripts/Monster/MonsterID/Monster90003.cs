public class Monster90003 : MonsterBase
{

    public override void Init()
    {
        monster = ConfigManager.Instance.GetConfig<MonstersConfig>(90003);
    }

}