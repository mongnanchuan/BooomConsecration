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
        PlayerTf = Player.GetComponent<Transform>();
        PlayerSprite = Player.GetComponent<SpriteRenderer>();
        BaseAttack.damage = 2;
        BaseAttack.type = 0;
        BaseAttack.range = 1;
        lm = GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //¥¶¿Ì ‰»Î
        if(isInPlayerTurn)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if(PlayerSprite.flipX)
                {
                    PlayerSprite.flipX = false;
                }
                else
                {
                    PlayerMove(-1);
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if(!PlayerSprite.flipX)
                {
                    PlayerSprite.flipX = true;
                }
                else
                {
                    PlayerMove(+1);
                }
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                Attack(BaseAttack);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                int index = (int)(PlayerTf.position.x / 1.5f + 4);
                if (lm.AltarIcons[index] != null)
                {
                    Altar targetAltar = lm.AltarIcons[index].GetComponent<Altar>();
                    if(targetAltar.CD == 0)
                    {
                        targetAltar.UseSkill(Player);
                        TurnEnd();
                    }
                }
            }
        }
    }

    public void TurnEnd()
    {
        if(isInPlayerTurn)
        {
            isInPlayerTurn = false;
        }
    }

    public void PlayerMove(int d)
    {
        float NewPosX = PlayerTf.position.x + d * 1.5f;
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Combat");
        if (NewPosX <= 6f && NewPosX >= -6f)
        {
            foreach(GameObject character in characters)
            {
                if(character.name != "MainRole" && character.GetComponent<Transform>().position.x == NewPosX)
                {
                    character.GetComponent<EnmeyAI>().EnemyMove(1);
                }
            }
            PlayerTf.position = new Vector3(NewPosX, PlayerTf.position.y, PlayerTf.position.z);
            TurnEnd();
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
