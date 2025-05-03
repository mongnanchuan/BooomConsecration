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
    public Attribute attr;

    public GameObject bullet;

    public bool isToRight = true;
    public bool isDoing = false;

    //Ч���������
    public bool isEffectDone = false;
    public int waitCount = 0;

    private GameObject BodyObject;
    public Animator anim;

    public bool useDefeat = false;

    public Vector2 shootOffect;
    public int SacrificeTimes = 0;

    private void Awake()
    {
        attr = GetComponent<Attribute>();
        BodyObject = transform.Find("Body")?.gameObject;
    }

    void Start()
    {
        PlayerTf = GetComponent<Transform>();
        lm = System.GetComponent<LevelManager>();
        cm = System.GetComponent<CombatManager>();
        GetComponent<Attribute>().PosNow = 4;
        anim = BodyObject.GetComponent<Animator>();
        DOTween.Init();
    }

    // Update is called once per frame
    void Update()
    {
        //��������
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
            if (Input.GetKeyDown(KeyCode.F))
            {
                isDoing = true;
                anim.SetTrigger("Attack");
                CombatManager.Instance.ShowFX(2, isToRight ? attr.PosNow + 1 : attr.PosNow - 1);
                StartCoroutine(UseSkill(10000));
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(!UseSkillOutside())
                    TipsManager.Instance.ShowTip("�޼��ܿ���ʹ��");
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                //�׼�
                int takeDamage = 0;
                bool success = Sacrifice(out takeDamage);
                if(success)
                {
                    anim.SetTrigger("Sacrifice");
                    
                    GetComponent<Attribute>().Damage(takeDamage,true);
                    SacrificeTimes++;
                    if(SacrificeTimes >= 5)
                    {
                        BodyObject.GetComponent<BodyPartManager>().TurnToThird();
                    }
                    else if(SacrificeTimes >= 3)
                    {
                        BodyObject.GetComponent<BodyPartManager>().TurnToSecond();
                    }
                }
                else
                    TipsManager.Instance.ShowTip("�޼��ܿ���Ѫ��");
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
                anim.SetTrigger("Skill");
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
                    //�׼�����ʾ��ȫ״̬
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
                else
                {
                    TipsManager.Instance.ShowTip("�޷�Ѫ��");
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

        if(useSkill.skill.id == 10017)
        {
            int endPos = -1;
            if (effects == null || effects.Count == 0)
                endPos = isToRight ? 8 : 0;
            else
            {
                foreach (var effect in effects)
                {
                    if (effect.Taker.GetComponent<MonsterBase>() != null && effect.type == Effect_Type.MakeDamage)
                        endPos = effect.Taker.PosNow;
                }
            }
            yield return StartCoroutine(LaunchProjectile(attr.PosNow,endPos));
        }

        if (useSkill.skill.id == 10018)
        {
            int endPos = isToRight ? 8 : 0;
            yield return StartCoroutine(LaunchProjectile(attr.PosNow, endPos));
        }


        if (effects != null && effects.Count != 0)
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

    private IEnumerator LaunchProjectile(int startPos,int endPos)
    {
        GameObject tempBullet = Instantiate(bullet,(Vector2)transform.position + shootOffect, Quaternion.identity,transform);
        Projectile projectile = tempBullet.GetComponent<Projectile>();

        projectile.type = 1; // ������ѡ�������
        projectile.startPos = startPos;
        projectile.endPos = endPos;

        yield return StartCoroutine(projectile.Shoot(shootOffect));
    }

    //�ƶ��ͽ���λ��
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

        // ���ڸ���Ч��������ɵ�һ����־
        bool isEffectHandled = false;

        // ����Ч�����������ʱ���ñ�־
        yield return StartCoroutine(attr.HandleEffect(effect, () =>
        {
            isEffectHandled = true;
            waitCount--;
            if (waitCount <= 0) isEffectDone = true;
        }, effect => StartCoroutine(AddEffectAndHandle(effect)))); // ȷ��ʹ�� StartCoroutine ����������
        // �ȴ�Ч���������
        yield return new WaitUntil(() => isEffectHandled);
    }

    public void ResetPlayer()
    {
        transform.position = new Vector2(0f, 0.2f);
        BodyObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        isToRight = true;
        attr.PosNow = 4;
    }

}
