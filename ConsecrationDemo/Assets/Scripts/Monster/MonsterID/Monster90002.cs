public class Monster90002 : MonsterBase
{

    public override void Init()
    {
        monster = ConfigManager.Instance.GetConfig<MonstersConfig>(90002);
    }

}