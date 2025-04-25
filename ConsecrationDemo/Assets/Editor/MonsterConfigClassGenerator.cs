using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class MonsterConfigClassGenerator
{
    [MenuItem("Tools/���� MonsterID ��")]
    public static void GenerateMonsterClasses()
    {
        string configPath = $"{Application.dataPath}/Resources/Configs/MonstersConfig.txt";
        string outputDir = $"{Application.dataPath}/Scripts/Monster/MonsterID";

        if (!File.Exists(configPath))
        {
            Debug.LogError($"δ�ҵ������ļ���{configPath}");
            return;
        }

        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        string json = File.ReadAllText(configPath);

        try
        {
            var monsterDict = JsonConvert.DeserializeObject<Dictionary<int, BaseConfig>>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });

            foreach (var kvp in monsterDict)
            {
                int id = kvp.Key;
                string className = $"Monster{id}";
                string filePath = Path.Combine(outputDir, $"{className}.cs");

                // ����ļ��Ƿ��Ѵ��ڣ������������������
                if (File.Exists(filePath))
                {
                    continue;
                }

                // �������ļ�������
                var classContent = $@"public class {className} : MonsterBase
{{

    public override void Init()
    {{
        monster = ConfigManager.Instance.GetConfig<MonstersConfig>({id});
    }}

}}";
                // д���ļ�
                File.WriteAllText(filePath, classContent);
            }

            // ˢ���ʲ����ݿ⣬ȷ�����ļ��� Unity ʶ��
            AssetDatabase.Refresh();
        }
        catch (Exception ex)
        {
            Debug.LogError($"�����л� MonsterConfig.txt ʧ�ܣ�{ex.Message}");
        }

        configPath = $"{Application.dataPath}/Resources/Configs/MonsterSkillsConfig.txt";
        outputDir = $"{Application.dataPath}/Scripts/Monster/MonsterSkill";

        if (!File.Exists(configPath))
        {
            Debug.LogError($"δ�ҵ������ļ���{configPath}");
            return;
        }

        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        string json2 = File.ReadAllText(configPath);

        try
        {
            var monsterDict = JsonConvert.DeserializeObject<Dictionary<int, BaseConfig>>(json2, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });

            foreach (var kvp in monsterDict)
            {
                int id = kvp.Key;
                string className = $"MonsterSkill{id}";
                string filePath = Path.Combine(outputDir, $"{className}.cs");

                // ����ļ��Ƿ��Ѵ��ڣ������������������
                if (File.Exists(filePath))
                {
                    continue;
                }

                // �������ļ�������
                var classContent = $@"
    using System.Collections.Generic;
    public class {className} : MonsterSkillBase
{{
        static MonsterSkill{id}()
    {{
        MonsterSkillFactory.Register({id}, typeof(MonsterSkill{id}));
    }}

    public override void Init()
    {{
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>({id});
    }}

    public override void GetEffects(List<MonsterTempData> monsData, int playerPos)
    {{
        Init();
    }}

}}";
                // д���ļ�
                File.WriteAllText(filePath, classContent);
            }

            // ˢ���ʲ����ݿ⣬ȷ�����ļ��� Unity ʶ��
            AssetDatabase.Refresh();
        }
        catch (Exception ex)
        {
            Debug.LogError($"�����л� MonsterSkillsConfig.txt ʧ�ܣ�{ex.Message}");
        }
    }
}