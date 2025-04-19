using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase
{
    public SkillsConfig skill;
    public virtual void Init() { }
    public virtual void TakeEffect(GameObject User) { }

    public void MakeDamage(GameObject User)
    {
        SpriteRenderer UserSp = User.GetComponent<SpriteRenderer>();
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
                                character.GetComponent<Attribute>().Damage(skill.damage);
                            }
                        }
                        //方向为身后
                        else if(skill.rangeDir == 2)
                        {
                            if ((UserSp.flipX && dis_now >= 0 && dis_now <= skill.range * 1.5f) || (!UserSp.flipX && dis_now <= 0 && dis_now >= -skill.range * 1.5f))
                            {
                                character.GetComponent<Attribute>().Damage(skill.damage);
                            }
                        }
                        //方向为身前身后
                        else if(skill.rangeDir == 3)
                        {
                            if (Mathf.Abs(dis_now) <= skill.range * 1.5f)
                            {
                                character.GetComponent<Attribute>().Damage(skill.damage);
                            }
                        }
                    }
                }
                break;

        }
    }
}
