public class Monster90002 : MonsterBase
{

    public override void Init()
    {
        monster = ConfigManager.Instance.GetConfig<MonstersConfig>(90002);
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