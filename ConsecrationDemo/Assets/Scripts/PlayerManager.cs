using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    public GameObject System;
    private Transform PlayerTf;
    private SpriteRenderer PlayerSprite;
    private LevelManager lm;
    private CombatManager cm;
    public SkillBase BaseAttack;
    public Attribute attr;

    public bool isToRight = true;
    public bool isDoing = false;

    //效果处理变量
    public bool isEffectDone = false;
    public int waitCount = 0;

    void Start()
    {
        attr = GetComponent<Attribute>();
        PlayerTf = GetComponent<Transform>();
        PlayerSprite = GetComponent<SpriteRenderer>();
        lm = System.GetComponent<LevelManager>();
        cm = System.GetComponent<CombatManager>();
        GetComponent<Attribute>().PosNow = 4;
        BaseAttack = new Skill80001();
        BaseAttack.Init();
        DOTween.Init();
    }

    // Update is called once per frame
    void Update()
    {
        //处理输入
        if (cm.isInPlayerTurn && !isDoing)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if (isToRight)
                {
                    PlayerSprite.flipX = false;
                    isToRight = false;
                }
                else
                {
                    if(attr.PosNow != 0)
                    {
                        isDoing = true;
                        StartCoroutine(MoveWithEffect(false));
                    }
                    
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if (!isToRight)
                {
                    PlayerSprite.flipX = true;
                    isToRight = true;
                }
                else
                {
                    if(attr.PosNow != 8)
                    {
                        isDoing = true;
                        StartCoroutine(MoveWithEffect(true));
                    }
                    
                }
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                isDoing = true;
                StartCoroutine(UseSkill(10000));
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
                //献祭
                int index = (int)(PlayerTf.position.x / 1.5f + 4);
                if (lm.AltarIcons[index] != null)
                {
                    Altar targetAltar = lm.AltarIcons[index].GetComponent<Altar>();
                    if (targetAltar.SkillIndex == 0 && targetAltar.CD == 0)
                    {
                        GetComponent<Attribute>().Damage(1);
                        targetAltar.SkillIndex = 1;
                        //献祭后显示完全状态
                        lm.AltarBlanks[index].GetComponentInParent<FloorConfig>().ShowCompleteGod(targetAltar);
                    }
                }
            }
        }
    }

    /*
    public void PlayerMove(int d, string Type)
    {
        float NewPosX = PlayerTf.position.x + d * 1.5f;
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Combat");
        if (NewPosX <= 6f && NewPosX >= -6f)
        {
            foreach (GameObject character in characters)
            {
                if (character.name != "MainRole" && character.GetComponent<Transform>().position.x == NewPosX)
                {
                    character.GetComponent<EnemyAI>().EnemyMove(1, "Slide");
                }
            }
            //PlayerTf.position = new Vector3(NewPosX, PlayerTf.position.y, PlayerTf.position.z);
            GetComponent<Attribute>().PosNow += d;
            if(Type == "Jump")
            {
                PlayerTf.DOLocalJump(new Vector3(NewPosX, PlayerTf.position.y, PlayerTf.position.z), 0.5f, 1, 0.2f, false);
            }
            else if(Type == "Slide")
            {
                PlayerTf.DOMove(new Vector3(NewPosX, PlayerTf.position.y, PlayerTf.position.z), 0.2f, false);
            }

            
            cm.TurnEnd();
        }
    }
    */

    public IEnumerator UseSkill(int skillID,int tokenID = 0)
    {
        SkillBase useSkill = SkillFactory.PCreate(skillID);
        useSkill.Init();
        var effects = useSkill.GetEffects();

        if (effects == null || effects.Count == 0)
            isEffectDone = true;

        foreach (var effect in effects)
        {
            yield return StartCoroutine(AddEffectAndHandle(effect));
        }
        yield return new WaitUntil(() => isEffectDone);
        isDoing = false;
        StartCoroutine(cm.TurnEnd());
    }

    //移动和交换位置
    public IEnumerator MoveWithEffect(bool isToRight)
    {
        int newPos = isToRight ? attr.PosNow + 1 : attr.PosNow - 1;
        Vector3 newVec = SwitchPos.IntToVector2(newPos);
        //transform.DOMove(newVec, 0.2f, false);
        transform.DOLocalJump(newVec, 0.5f, 1, 0.2f, false);

        List<Effect> effects = new List<Effect>();

        foreach (var mons in MonsterManager.Instance.currentMonstersData)
        {
            if(mons.Value.pos == newPos)
            {
                Effect effect1 = new Effect()
                {
                    type = Effect_Type.ForceJump,
                    Taker = mons.Value.obj.GetComponent<Attribute>(),
                    Ganker = this.attr,
                    portalMovePos = attr.PosNow
                };
                effects.Add(effect1);
            }
        }
        attr.PosNow = newPos;

       if (effects.Count == 0)
          isEffectDone = true;

       foreach (var effect in effects)
       {
            yield return StartCoroutine(AddEffectAndHandle(effect));
       }
       yield return new WaitUntil(() => isEffectDone);
        //yield return new WaitForSeconds(2f);
        isDoing = false;
       StartCoroutine(cm.TurnEnd());
    }

    private IEnumerator AddEffectAndHandle(Effect effect)
    {
        waitCount++;
        var attr = effect.Taker;

        // 用于跟踪效果处理完成的一个标志
        bool isEffectHandled = false;

        // 处理效果，并在完成时设置标志
        yield return StartCoroutine(attr.HandleEffect(effect, () =>
        {
            isEffectHandled = true;
            waitCount--;
            if (waitCount <= 0) isEffectDone = true;
        }, effect => StartCoroutine(AddEffectAndHandle(effect)))); // 确保使用 StartCoroutine 来传递自身
        // 等待效果处理完成
        yield return new WaitUntil(() => isEffectHandled);
    }

}
