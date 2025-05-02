using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
    public GameObject System;
    private Transform PlayerTf;
    private LevelManager lm;
    private CombatManager cm;
    public SkillBase BaseAttack;
    public Attribute attr;

    public bool isToRight = true;
    public bool isDoing = false;

    //效果处理变量
    public bool isEffectDone = false;
    public int waitCount = 0;

    private GameObject BodyObject;

    public bool useDefeat = false;

    void Start()
    {
        attr = GetComponent<Attribute>();
        PlayerTf = GetComponent<Transform>();
        BodyObject = transform.Find("Body")?.gameObject;
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
                    //PlayerSprite.flipX = false;
                    BodyObject.transform.DOLocalRotate(new Vector3(0, 180, 0), 0.2f);
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
                    //PlayerSprite.flipX = true;
                    BodyObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
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
                if(!UseSkillOutside())
                    TipsManager.Instance.ShowTip("无技能可以使用");
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                //献祭
                int takeDamage = 0;
                bool success = Sacrifice(out takeDamage);
                if(success)
                    GetComponent<Attribute>().Damage(takeDamage);
                else
                    TipsManager.Instance.ShowTip("无技能可以血祭");
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

    public bool UseSkillOutside()
    {
        int index = attr.PosNow;
        if (lm.AltarIcons[index] != null)
        {
            Altar targetAltar = lm.AltarIcons[index].GetComponent<Altar>();
            if (targetAltar.CD == 0)
            {
                int skill = targetAltar.GetSkillInfo();
                Token token = null;
                if (lm.TokenIcons[index] != null)
                {
                    token = lm.TokenIcons[index].GetComponent<Token>();
                }
                if(token!= null)
                {
                    StartCoroutine(UseSkill(skill, token.currentID, targetAltar));
                }
                else
                {
                    StartCoroutine(UseSkill(skill, 0, targetAltar));
                }
                return true;
            }
        }
        //Debug.Log(lm.AltarIcons[index]);
        return false;
    }

    public bool Sacrifice(out int damage, List<int> skillPosG = null)
    {
        List<int> sPos = new List<int>();
        damage = 0;
        if (skillPosG == null)
            sPos.Add(attr.PosNow);
        else
            sPos = skillPosG;
        //int index = (int)(PlayerTf.position.x / 1.5f + 4);
        foreach (var index in sPos)
        {
            if (lm.AltarIcons[index] != null)
            {
                Altar targetAltar = lm.AltarIcons[index].GetComponent<Altar>();
                int token = targetAltar.tokenID;

                if (targetAltar.SkillIndex == 0 && targetAltar.CD == 0)
                {
                    targetAltar.SkillIndex = 1;
                    //献祭后显示完全状态
                    lm.AltarBlanks[index].GetComponentInParent<FloorConfig>().ShowCompleteGod(targetAltar);
                    damage += 1;

                    if (token == 20009)
                        damage -= 1;

                    if (token == 20003)
                    {
                        damage += 1;
                        List<int> front = new List<int>();
                        int tempPos = isToRight ? index + 1 : index - 1;

                        if (tempPos >= 0 && tempPos <= 8)
                            front.Add(tempPos);

                        int tempD;
                        Sacrifice(out tempD, front);
                    }
                    return true;
                }
            }
        }
        return false;
    }


    public void TestSkill(int id)
    {
        StartCoroutine(UseSkill(id));
    }

    public IEnumerator UseSkill(int skillID,int tokenID = 0,Altar al = null)
    {
        SkillBase useSkill = SkillFactory.PCreate(skillID);
        useSkill.Init();

        if (tokenID == 20007)
            useSkill.Deal20007();

        var effects = useSkill.GetEffects();

        if (tokenID != 0 && tokenID != 20007)
            effects = useSkill.DealToken(tokenID, effects);

        if (effects == null || effects.Count == 0)
            isEffectDone = true;

        if(effects != null && effects.Count != 0)
        {
            foreach (var effect in effects)
            {
                if(effect.Taker != null)
                yield return StartCoroutine(AddEffectAndHandle(effect));
            }
            yield return new WaitUntil(() => isEffectDone);
        }
        
        isDoing = false;

        if (!useDefeat)
        {
            if (al != null)
            {
                al.IntoCD();
                if(tokenID == 20002)
                {
                    List<Altar> tempAl = new List<Altar>();
                    foreach (var item in lm.AltarIcons)
                    {
                        if (item == null)
                            continue;

                        if (item.GetComponent<Altar>().CD != 0)
                            tempAl.Add(item.GetComponent<Altar>());
                    }
                    int ramdomNum = Random.Range(0, tempAl.Count);
                    tempAl[ramdomNum].CD = 0;


                }
            }
                
            StartCoroutine(cm.TurnEnd());
        }
        else
            useDefeat = false;
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
