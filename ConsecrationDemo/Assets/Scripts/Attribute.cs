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
    private GameObject HPObj;
    private GameObject HPNum;
    private GameObject BodyObject;
    public List<int> BuffPool;

    public float moveSpeed = 20f;

    public event Action<int> OnPosChange;

    // Start is called before the first frame update
    void Start()
    {
        HPObj = transform.Find("Canvas/HP").gameObject;
        HPNum = transform.Find("Canvas/HPNum").gameObject;
        BodyObject = transform.Find("Body").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        HPObj.GetComponent<Image>().fillAmount = (float)HP / HPMax;
        HPNum.GetComponent<Text>().text = HP + "/" + HPMax;
    }

    public void HandleEffect(Effect targetEffect,Action onFinished = null, Action<Effect> addEffectCallback = null)
    {
        switch (targetEffect.type)
        {
            case Effect_Type.MakeDamage:
                Damage(targetEffect.damage);
                onFinished?.Invoke();
                break;
            case Effect_Type.Healing:
                Heal(targetEffect.heal);
                onFinished?.Invoke();
                break;
            case Effect_Type.ForceMove:
                StartCoroutine(ForceMove(targetEffect.forceMoveDis, onFinished, addEffectCallback));
                break;
        }
    }

    public void Damage(int num)
    {
        if(num > 0)
        {
            if(BodyObject != null)
            {
                BodyObject.transform.DOPunchPosition(0.5f * Vector3.right, 0.2f, 20, 0.5f);
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
        yield return StartCoroutine(MoveWithSpeed(newPos));
        onFinished?.Invoke();
    }

    //用一定速度移动到目标位置
    public IEnumerator MoveWithSpeed(int pos)
    {
        Vector2 startPos = transform.position;
        Vector2 endPos = SwitchPos.IntToVector2(pos);
        float distance = Vector2.Distance(startPos, endPos);
        float duration = distance / moveSpeed; // 总耗时 = 距离 / 速度
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration); // 插值进度 [0, 1]
            transform.position = Vector3.Lerp(startPos, endPos, t); // 插值移动
            yield return null;
        }

        transform.position = endPos;
        PosNow = pos;
        OnPosChange?.Invoke(PosNow);
        yield break;
    }

    public void MoveNewPos(int num)
    {
        if (num > 8)
            num = 8;
        if (num < 0)
            num = 0;

        PosNow = num;
        this.transform.position = SwitchPos.IntToVector2(PosNow);
        OnPosChange?.Invoke(PosNow);
    }

    public void Die()
    {

    }
}
