public class Monster90005 : MonsterBase
{

    public override void Init()
    {
        monster = ConfigManager.Instance.GetConfig<MonstersConfig>(90005);
    }

}