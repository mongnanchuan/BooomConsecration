using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject System;
    private Transform PlayerTf;
    private SpriteRenderer PlayerSprite;
    private LevelManager lm;
    private CombatManager cm;
    public SkillBase BaseAttack;
    // Start is called before the first frame update
    void Start()
    {
        PlayerTf = GetComponent<Transform>();
        PlayerSprite = GetComponent<SpriteRenderer>();
        lm = System.GetComponent<LevelManager>();
        cm = System.GetComponent<CombatManager>();
        BaseAttack = new Skill80001();
        BaseAttack.Init();
    }

    // Update is called once per frame
    void Update()
    {
        //¥¶¿Ì ‰»Î
        if (cm.isInPlayerTurn)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if (PlayerSprite.flipX)
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
                if (!PlayerSprite.flipX)
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
                BaseAttack.TakeEffect(gameObject);
                cm.TurnEnd();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                int index = (int)(PlayerTf.position.x / 1.5f + 4);
                if (lm.AltarIcons[index] != null)
                {
                    Altar targetAltar = lm.AltarIcons[index].GetComponent<Altar>();
                    if (targetAltar.CD == 0)
                    {
                        targetAltar.UseSkill(gameObject);
                        cm.TurnEnd();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                int index = (int)(PlayerTf.position.x / 1.5f + 4);
                if (lm.AltarIcons[index] != null)
                {
                    Altar targetAltar = lm.AltarIcons[index].GetComponent<Altar>();
                    if (targetAltar.SkillIndex == 0)
                    {
                        GetComponent<Attribute>().Damage(1);
                        targetAltar.SkillIndex = 1;
                    }
                }
            }
        }
    }

    public void PlayerMove(int d)
    {
        float NewPosX = PlayerTf.position.x + d * 1.5f;
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Combat");
        if (NewPosX <= 6f && NewPosX >= -6f)
        {
            foreach (GameObject character in characters)
            {
                if (character.name != "MainRole" && character.GetComponent<Transform>().position.x == NewPosX)
                {
                    character.GetComponent<EnmeyAI>().EnemyMove(1);
                }
            }
            PlayerTf.position = new Vector3(NewPosX, PlayerTf.position.y, PlayerTf.position.z);
            cm.TurnEnd();
        }
    }
}
