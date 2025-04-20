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
    private GameObject HPObj;
    private GameObject HPNum;
    private GameObject BodyObject;

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

    public void Damage(int num)
    {
        int newVal = HP - num;
        if(newVal < 0)
        {
            HP = 0;
            Die();
        }
        else if(newVal > HPMax)
        {
            HP = HPMax;
        }
        else
        {
            HP = newVal;
        }
        if(num > 0)
        {
            if(BodyObject != null)
            {
                BodyObject.transform.DOPunchPosition(0.5f * Vector3.right, 0.2f, 20, 0.5f);
            }
        }

    }

    public void Die()
    {

    }
}
