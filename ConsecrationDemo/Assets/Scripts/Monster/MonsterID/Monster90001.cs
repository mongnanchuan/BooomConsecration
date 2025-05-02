using UnityEngine;

public class Monster90001 : MonsterBase
{
    public override void Init()
    {
        monster = ConfigManager.Instance.GetConfig<MonstersConfig>(90001);
        attribute = GetComponent<Attribute>();
        attribute.HPMax = monster.hp;
        attribute.HP = monster.hp;
        attribute.OnPosChange += OnPosBeSet;

        skillCount = monster.skillGroup.Length;
        currentSkillCount = 0;
        currentSkillID = 0;
        isOnUse = false;

        bodyObject = transform.Find("Body").gameObject;
        bodyAnim = bodyObject.GetComponent<Animator>();

    }

}