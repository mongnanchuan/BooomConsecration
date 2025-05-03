
using System.Collections.Generic;
using UnityEngine;
public class MonsterSkill9000604 : MonsterSkillBase
{
    //对1/3/5/7造成3点伤害
    static MonsterSkill9000604()
    {
        SkillFactory.Register(9000604, typeof(MonsterSkill9000604));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000604);
    }

    public override List<Effect> GetEffects(MonsterBase mons)
    {
        List<Effect> effects = new List<Effect>();

        int monsNum1 = Random.Range(1, 5) + 90000;
        int monsNum2 = Random.Range(1, 5) + 90000;
        int monsNum3 = Random.Range(1, 5) + 90000;
        List<int> summonMons = new List<int>();
        List<int> summonPos = new List<int>();
        summonMons.Add(monsNum1);
        summonMons.Add(monsNum2);
        summonMons.Add(monsNum3);

        if (mons.currentPos > PlayerPosReport.Instance.GetPlayerPos())
        {
            int monsPos1 = Random.Range(0, mons.currentPos);
            int monsPos2 = Random.Range(0, mons.currentPos);
            int monsPos3 = Random.Range(mons.currentPos + 1, 9);
            summonPos.Add(monsPos1);
            summonPos.Add(monsPos2);
            summonPos.Add(monsPos3);
        }
        else
        {
            int monsPos1 = Random.Range(mons.currentPos+1,9);
            int monsPos2 = Random.Range(mons.currentPos+1,9);
            int monsPos3 = Random.Range(0, mons.currentPos);
            summonPos.Add(monsPos1);
            summonPos.Add(monsPos2);
            summonPos.Add(monsPos3);
        }

        MonsterManager.Instance.SetSummonMonsters(summonMons,summonPos);
        MonsterManager.Instance.SetNewMonsters(true);

        return effects;
    }

}