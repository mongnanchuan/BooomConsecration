public class Monster90001 : MonsterBase
{
    public override void Init()
    {
        monster = ConfigManager.Instance.GetConfig<MonstersConfig>(90001);
        attribute = GetComponent<Attribute>();
        skillCount = monster.skillGroup.Length;
        currentSkillCount = 0;
        currentSkillID = 0;
        isOnUse = false;
        attribute.OnPosChange += OnPosBeSet;
    }

}