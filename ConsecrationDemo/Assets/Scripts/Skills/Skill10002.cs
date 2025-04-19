using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10002 : SkillBase
{
    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10002);
    }
    public override void TakeEffect(GameObject User)
    {
        MakeDamage(User);
    }

}
