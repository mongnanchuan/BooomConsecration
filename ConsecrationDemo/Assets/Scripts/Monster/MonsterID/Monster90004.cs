public class Monster90004 : MonsterBase
{

    public override void Init()
    {
        monster = ConfigManager.Instance.GetConfig<MonstersConfig>(90004);
        attribute = GetComponent<Attribute>();
        attribute.HPMax = monster.hp;
        attribute.HP = monster.hp;
        attribute.OnPosChange += OnPosBeSet;

        skillCount = monster.skillGroup.Length;
        currentSkillCount = 0;
        currentSkillID = 0;
        isOnUse = false;
    }

}