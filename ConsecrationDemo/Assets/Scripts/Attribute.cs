using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Attribute : MonoBehaviour
{
    public int HP;
    public int HPMax;
    private GameObject HPObj;
    private GameObject HPNum;

    // Start is called before the first frame update
    void Start()
    {
        HPObj = transform.Find("Canvas/HP").gameObject;
        HPNum = transform.Find("Canvas/HPNum").gameObject;
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
    }

    public void Die()
    {

    }
}
