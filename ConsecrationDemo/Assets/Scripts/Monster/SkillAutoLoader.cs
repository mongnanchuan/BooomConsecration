using System;
using System.Linq;
using UnityEngine;

public static class SkillAutoLoader
{
    // ��������� Unity ������Ϸǰ���Զ�ִ��
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadAllSkills()
    {
        //�ҵ����ܻ���
        var baseType = typeof(MonsterSkillBase);
        //��ȡ��ǰ AppDomain �����зǳ���ļ����ࣨ�̳��� MonsterSkillBase��
        var allSkillTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract);

        foreach (var type in allSkillTypes)
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);
        }

        var newBaseType = typeof(SkillBase);
        //��ȡ��ǰ AppDomain �����зǳ���ļ����ࣨ�̳��� MonsterSkillBase��
        var newAllSkillTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsSubclassOf(newBaseType) && !t.IsAbstract);

        foreach (var type in newAllSkillTypes)
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);
        }

        Debug.Log($"����ϵͳ�Ѽ��� {allSkillTypes.Count()}+ {newAllSkillTypes.Count()}�������ࡣ");
    }
}
