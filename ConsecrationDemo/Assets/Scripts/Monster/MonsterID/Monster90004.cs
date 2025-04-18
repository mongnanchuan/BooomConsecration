public class Monster90004 : MonsterBase
{

    public override void Init()
    {
        monster = ConfigManager.Instance.GetConfig<MonstersConfig>(90004);
    }

}