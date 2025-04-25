using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class MonsterConfigClassGenerator
{
    [MenuItem("Tools/生成 MonsterID 类")]
    public static void GenerateMonsterClasses()
    {
        string configPath = $"{Application.dataPath}/Resources/Configs/MonstersConfig.txt";
        string outputDir = $"{Application.dataPath}/Scripts/Monster/MonsterID";

        if (!File.Exists(configPath))
        {
            Debug.LogError($"未找到配置文件：{configPath}");
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

                // 检查文件是否已存在，如果存在则跳过生成
                if (File.Exists(filePath))
                {
                    continue;
                }

                // 生成类文件的内容
                var classContent = $@"public class {className} : MonsterBase
{{

    public override void Init()
    {{
        monster = ConfigManager.Instance.GetConfig<MonstersConfig>({id});
    }}

}}";
                // 写入文件
                File.WriteAllText(filePath, classContent);
            }

            // 刷新资产数据库，确保新文件被 Unity 识别
            AssetDatabase.Refresh();
        }
        catch (Exception ex)
        {
            Debug.LogError($"反序列化 MonsterConfig.txt 失败：{ex.Message}");
        }

        configPath = $"{Application.dataPath}/Resources/Configs/MonsterSkillsConfig.txt";
        outputDir = $"{Application.dataPath}/Scripts/Monster/MonsterSkill";

        if (!File.Exists(configPath))
        {
            Debug.LogError($"未找到配置文件：{configPath}");
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

                // 检查文件是否已存在，如果存在则跳过生成
                if (File.Exists(filePath))
                {
                    continue;
                }

                // 生成类文件的内容
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
                // 写入文件
                File.WriteAllText(filePath, classContent);
            }

            // 刷新资产数据库，确保新文件被 Unity 识别
            AssetDatabase.Refresh();
        }
        catch (Exception ex)
        {
            Debug.LogError($"反序列化 MonsterSkillsConfig.txt 失败：{ex.Message}");
        }
    }
}