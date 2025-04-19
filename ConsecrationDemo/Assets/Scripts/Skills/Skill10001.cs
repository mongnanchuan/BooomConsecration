using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill10001 : SkillBase
{
    public override void Init()
    {
        skill = ConfigManager.Instance.GetConfig<SkillsConfig>(10001);
    }
    public override void TakeEffect(GameObject User)
    {
        MakeDamage(User);
    }

}
