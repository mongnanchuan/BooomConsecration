using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;


public class MonsterBase : MonoBehaviour
{
    public MonstersConfig monster;
    public Attribute attribute;
    public int count;//编号
    public int currentPos;//当前位置编号

    public int skillCount;//技能数
    public int currentSkillID;//当前技能ID
    public int currentSkillCount;//当前技能序号
    public bool isToRight = false;//朝向

    public bool isOnUse;//是否在蓄力
    public Vector2 shootOffect;

    public Transform zone;//装威胁范围显示的父节点
    public GameObject warningZone1;//威胁类型1显示
    public GameObject warningZone2;//威胁类型2显示
    public GameObject warningZone3;//威胁类型3显示
    public GameObject warningZone4;//威胁类型4显示
    public Vector2 offset;//威胁类型3/4相对位置调整值

    public GameObject bullet;
    public GameObject slime;

    //位置改动后的通知
    public event Action<int, int> OnPosSetted;

    //效果处理变量
    public bool isEffectDone = false;
    public int waitCount = 0;

    public int posAddjust = 0;

    public GameObject bodyObject;
    public Animator bodyAnim;

    public virtual void Init() { }

    //回合开始进行判断
    public IEnumerator HandleTurnWithEffect(int playerPos)
    {
        List<MonsterTempData> monsterData = MonsterManager.Instance.currentMonstersData
        .Select(kv => new MonsterTempData { num = kv.Key, pos = kv.Value.pos, obj = kv.Value.obj })
        .ToList();

        if (isOnUse)
        {
            //Todo:
            //技能触发效果
            bodyAnim.SetTrigger("AttackBegin");
            MonsterSkillBase useSkill = SkillFactory.MCreate(currentSkillID);
            useSkill.Init();
            var effects = useSkill.GetEffects(this);

            if (effects == null || effects.Count == 0)
                isEffectDone = true;

            for (int i = 0; i < zone.childCount; i++)
            {
                GameObject.Destroy(zone.GetChild(i).gameObject);
            }

            if (useSkill.monsterSkill.attactType == 4)
            {
                int endPos = -1;
                if (effects == null || effects.Count == 0)
                    endPos = isToRight ? 8 : 0;
                else
                {
                    foreach (var effect in effects)
                    {
                        if (effect.type == Effect_Type.MakeDamage)
                            endPos = effect.Taker.PosNow;
                    }
                }
                yield return StartCoroutine(LaunchProjectile(currentPos, endPos));
            }

            if (useSkill.monsterSkill.attactType == 2)
            {
                if(useSkill.monsterSkill.id == 9000201 || useSkill.monsterSkill.id == 9000502)
                    yield return StartCoroutine(LaunchProjectile2(currentPos, posAddjust));
                else
                {
                    for (int i = 0; i < useSkill.monsterSkill.posPar.Length; i++)
                    {
                        StartCoroutine(LaunchProjectile2(currentPos, useSkill.monsterSkill.posPar[i]));
                    }
                }
            }


            foreach (var effect in effects)
            {
                if (effect.Taker != null)
                    yield return StartCoroutine(AddEffectAndHandle(effect));
            }
            yield return new WaitUntil(() => isEffectDone);

            currentSkillID = 0;
            isOnUse = false;
            if (currentSkillCount + 1 >= skillCount)
                currentSkillCount = 0;
            else
                currentSkillCount++;

            yield break;
        }
        else//不放技能咋办
        {
            if (playerPos > currentPos)
                isToRight = true;
            else
                isToRight = false;

            SetDir();
            
            bool isBeBlock = false;
            int min = Mathf.Min(currentPos, playerPos);
            int max = Mathf.Max(currentPos, playerPos);
            currentSkillID = monster.skillGroup[currentSkillCount];

            for (int i = 0; i < monsterData.Count; i++)
            {
                if (monsterData[i].pos > min && monsterData[i].pos < max)
                    isBeBlock = true;
            }
            var skill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(currentSkillID);
            //Debug.Log(skill.id);
            Transform zoneParent = skill.attactType == 2 ? MonsterManager.Instance.transform : this.transform;
            zone.SetParent(zoneParent);
            switch (skill.attactType)
            {
                case 1:
                    if(Mathf.Abs(currentPos-playerPos)<= skill.rangePar)
                    {
                        ReadyToUseSkill(currentSkillID, playerPos);
                        yield break;
                    }
                    break;
                case 2:
                    ReadyToUseSkill(currentSkillID, playerPos);
                    posAddjust = playerPos;
                    yield break;
                case 3:
                    if (isBeBlock)
                        yield break;
                    ReadyToUseSkill(currentSkillID, playerPos);
                    yield break;
                case 4:
                    if (isBeBlock)
                        yield break;
                    ReadyToUseSkill(currentSkillID, playerPos);
                    yield break;
                case 0:
                    isOnUse = true;
                    StartCoroutine(HandleTurnWithEffect(playerPos));
                    yield break;
                default:
                    break;
            }
            //不放技能也不蓄力就移动
            if (isBeBlock)
                yield break;

            int[] monstersPos = new int[monsterData.Count];
            for (int i = 0; i < monsterData.Count; i++)
            {
                monstersPos[i] = monsterData[i].pos;
            }

            if(monster.moveType == 0)
            {
                int newPos = Move(monstersPos, playerPos);
                attribute.MoveNewPos(newPos);
            }
            yield break;
        }
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

    //蓄力函数
    public void ReadyToUseSkill(int skillID,int playerPos)
    {
        bodyAnim.SetTrigger("ChargeBegin");
        var skill = ConfigManager.Instance.GetConfig<MonsterSkillsConfig>(skillID);
        int type = skill.attactType;
        List<int> waringPoss = new List<int>();
        isOnUse = true;
        switch (type)
        {
            case 1:
                if(skill.rangeDir==1)
                {
                    for (int i = 0; i < skill.rangePar; i++)
                    {
                        waringPoss.Add(isToRight? currentPos + 1 + i: currentPos - 1 - i);
                    }
                    foreach (var num in waringPoss)
                    {
                        GameObject.Instantiate(warningZone1,SwitchPos.IntToVector2(num), Quaternion.identity, zone);
                    }
                }
                else if(skill.rangeDir==2)
                {
                    for (int i = 0; i < skill.rangePar; i++)
                    {
                        waringPoss.Add(currentPos + 1 + i);
                        waringPoss.Add(currentPos - 1 - i);
                    }
                    foreach (var num in waringPoss)
                    {
                        GameObject.Instantiate(warningZone1, SwitchPos.IntToVector2(num), Quaternion.identity, zone);
                    }
                }
                break;
            case 2:
                if (skill.posPar[0] == -1)
                {
                    GameObject.Instantiate(warningZone2, SwitchPos.IntToVector2(playerPos), Quaternion.identity, zone);
                }
                else
                {
                    for (int i = 0; i < skill.posPar.Length; i++)
                    {
                        waringPoss.Add(skill.posPar[i]);
                    }
                    foreach (var num in waringPoss)
                    {
                        GameObject.Instantiate(warningZone2, SwitchPos.IntToVector2(num), Quaternion.identity, zone);
                    }
                }
                break;
            case 3:
                GameObject zone3 = GameObject.Instantiate(warningZone3, zone);
                if(isToRight)
                {
                    zone3.transform.localPosition = offset;
                    zone3.transform.localScale = new Vector3(-0.6f,0.6f,1);
                }
                else
                {
                    zone3.transform.localPosition = new Vector2(-offset.x, offset.y);
                    zone3.transform.localScale = new Vector3(0.6f, 0.6f, 1);
                }
                break;
            case 4:
                GameObject zone4 = GameObject.Instantiate(warningZone4, zone);
                if (isToRight)
                {
                    zone4.transform.localPosition = offset;
                    zone4.transform.localScale = new Vector3(-0.5f, 0.5f, 1);
                }
                else
                {
                    zone4.transform.localPosition = new Vector2(-offset.x, offset.y);
                    zone4.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                }
                break;
            default:
                break;
        }

    }

    //移动函数
    public int Move(int[] monstersPos, int playerPos)
    {
        if(currentPos > playerPos)
        {
            for (int i = 0; i < monstersPos.Length; i++)
            {
                if (monstersPos[i] == currentPos - 1)
                    return currentPos;
            }
            return currentPos - 1;
        }
        else
        {
            for (int i = 0; i < monstersPos.Length; i++)
            {
                if (monstersPos[i] == currentPos + 1)
                    return currentPos;       
            }
            return currentPos + 1;
        }
    }

    //控制朝向
    public void SetDir()
    {
        Vector3 dir = transform.localScale;
        GameObject BodyObject = transform.Find("Body")?.gameObject;
        if (isToRight)
            //transform.localScale = new Vector3(-Mathf.Abs(dir.x), dir.y, dir.z);
            BodyObject.transform.DOLocalRotate(new Vector3(0, 180, 0), 0.2f);
        else
            //transform.localScale = new Vector3(Mathf.Abs(dir.x), dir.y, dir.z);
            BodyObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
    }

    //设置位置并汇报
    public void OnPosBeSet(int posNum)
    {
        currentPos = posNum;
        OnPosSetted?.Invoke(count, currentPos);
    }

    private IEnumerator LaunchProjectile(int startPos, int endPos)
    {
        GameObject tempBullet = Instantiate(bullet, (Vector2)transform.position + shootOffect, Quaternion.identity, transform);
        Projectile projectile = tempBullet.GetComponent<Projectile>();

        projectile.type = 1; // 假设你选择飞行型
        projectile.startPos = startPos;
        projectile.endPos = endPos;

        yield return StartCoroutine(projectile.Shoot(shootOffect));
    }

    private IEnumerator LaunchProjectile2(int startPos, int endPos)
    {
        GameObject tempBullet = Instantiate(slime, (Vector2)transform.position + shootOffect, Quaternion.identity, transform);
        Projectile projectile = tempBullet.GetComponent<Projectile>();

        projectile.type = 2; // 假设你选择飞行型
        projectile.startPos = startPos;
        projectile.endPos = endPos;

        yield return StartCoroutine(projectile.Shoot(shootOffect));
    }

}
