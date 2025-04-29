using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillFactory
{
    private static Dictionary<int, Type> skillDict = new Dictionary<int, Type>();
    public static void Register(int skillID, Type type)
    {
        if (!skillDict.ContainsKey(skillID))
        {
            skillDict[skillID] = type;
        }
    }

    public static MonsterSkillBase MCreate(int skillID)
    {
        if (skillDict.TryGetValue(skillID, out var type))
        {
            return (MonsterSkillBase)Activator.CreateInstance(type);
        }
        Debug.LogError($"未找到技能 ID {skillID} 对应的技能类！");
        return null;
    }

    public static SkillBase PCreate(int skillID)
    {
        if (skillDict.TryGetValue(skillID, out var type))
        {
            return (SkillBase)Activator.CreateInstance(type);
        }
        Debug.LogError($"未找到技能 ID {skillID} 对应的技能类！");
        return null;
    }
}