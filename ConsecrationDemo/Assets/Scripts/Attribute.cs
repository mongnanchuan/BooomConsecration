using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;


public class Attribute : MonoBehaviour
{
    public int HP;
    public int HPMax;
    public int PosNow;
    //private GameObject HPObj;
    //private GameObject HPNum;
    private GameObject BodyObject;
    public List<int> BuffPool;

    public float moveSpeed = 20f;
    public int additionalTurn = 0;

    public event Action<int> OnPosChange;

    public Animator BodyAnim;
    

    // Start is called before the first frame update
    void Start()
    {
        BodyObject = transform.Find("Body")?.gameObject;
        BodyAnim = BodyObject.GetComponent<Animator>();
    }

    public void healthInit()
    {
        HealthBarManager.Instance.HealthBarInit(this);
    }

    // Update is called once per frame
    void Update()
    {
        //HPObj.GetComponent<Image>().fillAmount = (float)HP / HPMax;
        //HPNum.GetComponent<Text>().text = HP + "/" + HPMax;
    }

    public IEnumerator HandleEffect(Effect targetEffect,Action onFinished = null, Action<Effect> addEffectCallback = null)
    {
        switch (targetEffect.type)
        {
            case Effect_Type.MakeDamage:
                //StartCoroutine(Damage(targetEffect.damage));
                Damage(targetEffect.damage);
                onFinished?.Invoke();
                break;
            case Effect_Type.Healing:
                Heal(targetEffect.heal);
                onFinished?.Invoke();
                break;
            case Effect_Type.ForceMove:
                yield return StartCoroutine(ForceMove(targetEffect.forceMoveDis, onFinished, addEffectCallback));
                break;
            case Effect_Type.ForceJump:
                yield return StartCoroutine(ForceJump(targetEffect.portalMovePos, onFinished, addEffectCallback));
                break;
        }
    }

    public void Damage(int num)
    {
        if (num > 0)
        {
            if (BodyObject != null)
            {
                if(gameObject.name == "MainRole")
                {
                    BodyAnim.SetTrigger("Hit");
                }
                else
                {
                    BodyObject.transform.DOPunchPosition(0.5f * Vector3.right, 0.2f, 8, 1);
                }
                //BodyObject.transform.DOShakePosition(0.2f, 1f, 2, 50, true);
            }
            int newVal = HP - num;
            if (newVal <= 0)
            {
                HP = 0;
                Die();
            }
            else
            {
                HP = newVal;
            }
        }
    }

    /*    public IEnumerator Damage(int num)
        {
            if (num > 0)
            {
                int newVal = HP - num;
                if (newVal <= 0)
                {
                    HP = 0;
                    Die();
                }
                else
                {
                    HP = newVal;
                }
                if (BodyObject != null)
                {
                    Tweener _tweener = BodyObject.transform.DOPunchPosition(0.5f * Vector3.right, 0.2f, 20, 0.5f);
                    yield return _tweener.WaitForCompletion();
                }
            }
        }*/

    public void Heal(int num)
    {
        if (num > 0)
        {
            if (BodyObject != null)
            {
                //BodyObject.transform.DOPunchPosition(0.5f * Vector3.right, 0.2f, 20, 0.5f);
            }
            int newVal = HP + num;
            if (newVal >= HPMax)
            {
                HP = HPMax;
            }
            else
            {
                HP = newVal;
            }
        }
    }

    public IEnumerator ForceMove(int dis, Action onFinished, Action<Effect> addEffectCallback = null)
    {
        GameObject collisionMons = null;
        int newPos = PosNow;
        for (int i = 0; i < Mathf.Abs(dis); i++)
        {
            int tempPos = PosNow + (i + 1) * (int)Mathf.Sign(dis);
            GameObject obj = MonsterManager.Instance.GetMonsterAtPosition(tempPos);
            if(obj!=null)
            {
                collisionMons = obj;
                break;
            }
            if(tempPos == PlayerPosReport.Instance.GetPlayerPos())
            {
                collisionMons = PlayerPosReport.Instance.gameObject;
                break;
            }
            if(tempPos <= 8 && tempPos >= 0)
            newPos = tempPos;
        }
        if(collisionMons != null && addEffectCallback != null)
        {
            Effect effectToOther = new Effect()
            {
                type = Effect_Type.MakeDamage,
                Taker = collisionMons.GetComponent<Attribute>(),
                damage = 1
            };
            addEffectCallback(effectToOther);
            Effect effectToSelf = new Effect()
            {
                type = Effect_Type.MakeDamage,
                Taker = this, // 当前对象的 Attribute 组件
                damage = 1
            };
            addEffectCallback(effectToSelf);
        }
        PosNow = newPos;
        OnPosChange?.Invoke(PosNow);
        yield return StartCoroutine(MoveWithSpeed(newPos));
        onFinished?.Invoke();
    }

    //强制跳跃
    public IEnumerator ForceJump(int pos, Action onFinished, Action<Effect> addEffectCallback = null)
    {
        bool isJump = false;
        int newPos = PosNow;
        GameObject obj = MonsterManager.Instance.GetMonsterAtPosition(pos);
        if (obj == null || obj.GetComponent<Attribute>().PosNow != pos)
        {
            newPos = pos;
            isJump = true;
        }
        if (isJump)
        {
            yield return StartCoroutine(JumpMove(newPos));
        }

        PosNow = newPos;
        OnPosChange?.Invoke(PosNow);
        onFinished?.Invoke();
    }

    //强制跳跃
    public IEnumerator JumpMove(int pos)
    {
        Vector3 newVec = (Vector3)SwitchPos.IntToVector2(pos);
        yield return transform.DOLocalJump(newVec, 0.5f, 1, 0.2f, false).WaitForCompletion();
    }


    //用一定速度移动到目标位置
    public IEnumerator MoveWithSpeed(int pos)
    {
        Vector2 startPos = transform.position;
        Vector2 endPos = SwitchPos.IntToVector2(pos);
        Tweener _tweener = transform.DOMove(endPos, 0.2f, false);
        yield return _tweener.WaitForCompletion();
        OnPosChange?.Invoke(PosNow);
        //yield break;
    }

    public void MoveNewPos(int num)
    {
        if (num > 8)
            num = 8;
        if (num < 0)
            num = 0;

        PosNow = num;
        Vector2 endPos = SwitchPos.IntToVector2(PosNow);
        transform.DOLocalJump(endPos, 0.5f, 1, 0.2f, false);
        //this.transform.position = SwitchPos.IntToVector2(PosNow);
        OnPosChange?.Invoke(PosNow);
    }

    public void Die()
    {
        if(GetComponent<PlayerManager>() == null)
        {
            MonsterManager.Instance.DestroyMonster(GetComponent<MonsterBase>().count);
        }
        else
        {
            //游戏失败界面
        }
        
    }
}
