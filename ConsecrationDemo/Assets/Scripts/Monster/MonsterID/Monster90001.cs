public class Monster90001 : MonsterBase
{

    public override void Init()
    {
        monster = ConfigManager.Instance.GetConfig<MonstersConfig>(90001);
    }

}