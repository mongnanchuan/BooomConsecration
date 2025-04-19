using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public bool isInPlayerTurn = false;
    public GameObject Player;
    private Transform PlayerTf;
    private SpriteRenderer PlayerSprite;
    private SkillsConfig BaseAttack = new SkillsConfig();
    private LevelManager lm;
    // Start is called before the first frame update
    void Start()
    {
/*        BaseAttack.damage = 2;
        BaseAttack.type = 0;
        BaseAttack.range = 1;*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnEnd()
    {
        if(isInPlayerTurn)
        {
            isInPlayerTurn = false;
            //通知MonsterManager行动
        }
    }

    public void Attack(SkillsConfig targetSkill)
    {
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Combat");
        if(targetSkill.type == 0)
        {
            foreach (GameObject character in characters)
            {
                if(character.name != "MainRole")
                {
                    float dis_now = Player.GetComponent<Transform>().position.x - character.GetComponent<Transform>().position.x;
                    if (!PlayerSprite.flipX && dis_now >= 0 && dis_now <= targetSkill.range * 1.5f)
                    {
                        character.GetComponent<Attribute>().Damage(targetSkill.damage);
                    }
                    else if (PlayerSprite.flipX && dis_now <= 0 && dis_now >= -targetSkill.range * 1.5f)
                    {
                        character.GetComponent<Attribute>().Damage(targetSkill.damage);
                    }
                }
            }
        }
        TurnEnd();
    }
}
