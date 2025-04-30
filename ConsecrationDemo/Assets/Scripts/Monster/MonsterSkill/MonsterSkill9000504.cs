
using System.Collections.Generic;
using UnityEngine;
public class MonsterSkill9000504 : MonsterSkillBase
{
    //��1/3/5/7���3���˺�
    static MonsterSkill9000504()
    {
        SkillFactory.Register(9000504, typeof(MonsterSkill9000504));
    }

    public override void Init()
    {
        monsterSkill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(9000504);
    }

    public override List<Effect> GetEffects(MonsterBase mons)
    {
        List<Effect> effects = new List<Effect>();

        int monsNum1 = Random.Range(1, 5) + 90000;
        int monsNum2 = Random.Range(1, 5) + 90000;
        List<int> summonMons = new List<int>();
        List<int> summonPos = new List<int>();
        summonMons.Add(monsNum1);
        summonMons.Add(monsNum2);

        if(mons.currentPos > PlayerPosReport.Instance.GetPlayerPos())
        {
            int monsPos1 = Random.Range(0, mons.currentPos);
            int monsPos2 = Random.Range(0, mons.currentPos);
            summonPos.Add(monsPos1);
            summonPos.Add(monsPos2);
        }
        else
        {
            int monsPos1 = Random.Range(mons.currentPos+1,9);
            int monsPos2 = Random.Range(mons.currentPos+1,9);
            summonPos.Add(monsPos1);
            summonPos.Add(monsPos2);
        }

        MonsterManager.Instance.SetSummonMonsters(summonMons,summonPos);
        MonsterManager.Instance.SetNewMonsters();

        return effects;
    }

}