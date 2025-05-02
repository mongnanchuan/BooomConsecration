using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase
{
    public SkillsConfig skill;
    public CombatManager cm;

    public SkillsConfig fgasf;

    public int rangeOffset = 0;
    //public Effect tokenEffect;

    void Start()
    {
        cm = GameObject.FindGameObjectWithTag("System").GetComponent<CombatManager>();
    }
    public virtual void Init() { }
    public virtual void TakeEffect(GameObject User) { }

    public virtual List<Effect> GetEffects()
    {
        List<Effect> effects = new List<Effect>();
        return effects;
    }

    public List<Attribute> GetRoleInArea(List<int> area)
    {
        List<Attribute> attrs = new List<Attribute>();
        foreach (var pos in area)
        {
            foreach (var mons in MonsterManager.Instance.currentMonstersData.Values)
            {
                if (pos == mons.pos)
                    attrs.Add(mons.obj.GetComponent<Attribute>());
            }
        }
        return attrs;
    }


    public Attribute GetFirstFaceRole(int pos,bool isToRight)
    {
        int tempPos = pos;
        while (tempPos<= 8 && tempPos >= 0)
        {
            tempPos = isToRight ? tempPos + 1 : tempPos - 1;
            foreach (var mons in MonsterManager.Instance.currentMonstersData.Values)
            {
                if (mons.pos == tempPos)
                    return mons.obj.GetComponent<Attribute>();
            }
        }
        return null;
    }


    public int GetLastFacePos(int pos, bool isToRight)
    {
        int tempPos = pos;
        while (tempPos <= 8 && tempPos >= 0)
        {
            tempPos = isToRight ? tempPos + 1 : tempPos - 1;
            foreach (var mons in MonsterManager.Instance.currentMonstersData.Values)
            {
                if (mons.pos == tempPos)
                    return isToRight? tempPos - 1 : tempPos +1;
            }
        }
        return isToRight ? tempPos - 1 : tempPos + 1;
    }

    public void Deal20007()
    {
        rangeOffset = 1;
    }

    public List<Effect> DealToken(int tokenID,List<Effect> original)
    {
        List<Effect> newEffects = original;
        Attribute attrP = PlayerPosReport.Instance.attr;

        switch (tokenID)
        {
            case 20004:
                Effect tokenEffect = new Effect()
                {
                    type = Effect_Type.Healing,
                    Taker = attrP,
                    Ganker = attrP,
                    heal = 1
                };
                newEffects.Add(tokenEffect);
                break;
            case 20005:
                if (original != null && original.Count == 0)
                {
                    foreach (var effect in newEffects)
                    {
                        if (effect.type == Effect_Type.MakeDamage)
                            effect.damage += 1;
                    }
                }
                break;
            case 20006:
                if (original != null && original.Count == 0)
                {
                    foreach (var effect in newEffects)
                    {
                        if (effect.type == Effect_Type.ForceMove
                            && effect.Taker != attrP)
                            effect.forceMoveDis += 1;
                    }
                }
                break;
            case 20008:
                attrP.additionalTurn += 1;
                break;
        }
        return newEffects;
    }

    /*
    public void MakeDamage(GameObject User)
    {
        SpriteRenderer UserSp = User.GetComponent<SpriteRenderer>();
        Effect damageEffect = new Effect();
        damageEffect.type = Effect_Type.MakeDamage;
        damageEffect.damage = skill.damage;
        switch (skill.type)
        {
            //近战
            case 1:
                GameObject[] characters = GameObject.FindGameObjectsWithTag("Combat");
                foreach (GameObject character in characters)
                {
                    if (character.name != User.name)
                    {
                        float dis_now = User.GetComponent<Transform>().position.x - character.GetComponent<Transform>().position.x;
                        //方向为身前
                        if(skill.rangeDir == 1)
                        {
                            if ((!UserSp.flipX && dis_now >= 0 && dis_now <= skill.range * 1.5f) || (UserSp.flipX && dis_now <= 0 && dis_now >= -skill.range * 1.5f))
                            {
                                character.GetComponent<Attribute>().HandleEffect(damageEffect);
                            }
                        }
                        //方向为身后
                        else if(skill.rangeDir == 2)
                        {
                            if ((UserSp.flipX && dis_now >= 0 && dis_now <= skill.range * 1.5f) || (!UserSp.flipX && dis_now <= 0 && dis_now >= -skill.range * 1.5f))
                            {
                                character.GetComponent<Attribute>().HandleEffect(damageEffect);
                            }
                        }
                        //方向为身前身后
                        else if(skill.rangeDir == 3)
                        {
                            if (Mathf.Abs(dis_now) <= skill.range * 1.5f)
                            {
                                character.GetComponent<Attribute>().HandleEffect(damageEffect);
                            }
                        }
                    }
                }
                break;
        }
    }
    */
}
