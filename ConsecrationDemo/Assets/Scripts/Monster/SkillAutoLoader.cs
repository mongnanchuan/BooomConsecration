using System;
using System.Linq;
using UnityEngine;

public static class SkillAutoLoader
{
    // 这个方法在 Unity 加载游戏前就自动执行
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadAllSkills()
    {
        //找到技能基类
        var baseType = typeof(MonsterSkillBase);
        //获取当前 AppDomain 下所有非抽象的技能类（继承自 MonsterSkillBase）
        var allSkillTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract);

        foreach (var type in allSkillTypes)
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);
        }

        var newBaseType = typeof(SkillBase);
        //获取当前 AppDomain 下所有非抽象的技能类（继承自 MonsterSkillBase）
        var newAllSkillTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsSubclassOf(newBaseType) && !t.IsAbstract);

        foreach (var type in newAllSkillTypes)
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);
        }

        Debug.Log($"技能系统已加载 {allSkillTypes.Count()}+ {newAllSkillTypes.Count()}个技能类。");
    }
}
