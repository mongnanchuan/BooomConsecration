using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public static HealthBarManager Instance { get; private set; }

    //ͳһ�����ֵ�
    private Dictionary<Attribute, GameObject> allHealthBars = new Dictionary<Attribute, GameObject>();

    public GameObject healthBar;
    public GameObject healthDot;
    public Sprite redDot;
    public Sprite blackDot;
    public Vector3 offset;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void HealthBarInit(Attribute attr)
    {
        if (!allHealthBars.ContainsKey(attr))
        {
            GameObject bar = Instantiate(healthBar,this.transform); // ����Ѫ��UI
            allHealthBars[attr] = bar;
            for (int i = 0; i < attr.HPMax; i++)
            {
                Instantiate(healthDot,bar.transform);
            }
            UpdateHealthBar(attr); // ����Ѫ��λ�ú�Ѫ��
        }
    }

    public void UpdateHealthBar(Attribute attr)
    {
        if (allHealthBars.TryGetValue(attr, out GameObject bar))
        {
            RectTransform barRect = bar.GetComponent<RectTransform>();
            Vector3 worldPos = attr.transform.position; // ���� attr ���� Transform ���

            Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos + offset); // ����������ת��Ϊ��Ļ����
            if (attr.GetComponent<Monster90005>()!= null || attr.GetComponent<Monster90006>()!= null)
            {
                screenPos = Camera.main.WorldToScreenPoint(worldPos + offset + new Vector3(0f,0.8f,0f));
            }       
            
            barRect.anchoredPosition = screenPos; // ����Ѫ����λ��

            for (int i = 0; i < bar.transform.childCount; i++)
            {
                if(i < attr.HP)
                {
                    bar.transform.GetChild(i).GetComponent<Image>().sprite = redDot;
                }
                else
                {
                    bar.transform.GetChild(i).GetComponent<Image>().sprite = blackDot;
                }
            }
        }
    }

    public void RemoveHealthBar(Attribute attr)
    {
        if (allHealthBars.TryGetValue(attr, out GameObject bar))
        {
            Destroy(bar);
            allHealthBars.Remove(attr);
        }
    }

    public void UpdateAllHealthBars()
    {
        foreach (var attr in allHealthBars.Keys)
        {
            UpdateHealthBar(attr);
        }
    }

}
