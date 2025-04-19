using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
    public void UseSkill(GameObject User, SkillBase targetSkill)
    {
        targetSkill.TakeEffect(User);
    }
}
