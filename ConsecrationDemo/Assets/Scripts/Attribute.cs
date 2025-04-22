using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


public class Attribute : MonoBehaviour
{
    public int HP;
    public int HPMax;
    public int PosNow;
    private GameObject HPObj;
    private GameObject HPNum;
    private GameObject BodyObject;
    public List<int> BuffPool;

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

    public void HandleEffect(Effect targetEffect)
    {
        switch (targetEffect.type)
        {
            case Effect_Type.MakeDamage:
                Damage(targetEffect.damage);
                break;
            case Effect_Type.Healing:
                Heal(targetEffect.heal);
                break;
            case Effect_Type.ForceMove:
                ForceMove(targetEffect.forceMoveDis);
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

    public void ForceMove(int forceMoveDis)
    {
        if (GetComponent<PlayerManager>())
        {
            GetComponent<PlayerManager>().PlayerMove(forceMoveDis, "Slide");
            return;
        }
        else if (GetComponent<EnemyAI>())
        {
            GetComponent<EnemyAI>().EnemyMove(forceMoveDis, "Slide");
        }
    }

    public void Die()
    {

    }
}
