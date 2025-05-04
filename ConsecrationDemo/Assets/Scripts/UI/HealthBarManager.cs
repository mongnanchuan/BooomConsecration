using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public static HealthBarManager Instance { get; private set; }

    //统一管理字典
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
            GameObject bar = Instantiate(healthBar,this.transform); // 创建血条UI
            allHealthBars[attr] = bar;
            for (int i = 0; i < attr.HPMax; i++)
            {
                Instantiate(healthDot,bar.transform);
            }
            UpdateHealthBar(attr); // 更新血条位置和血量
        }
    }

    public void UpdateHealthBar(Attribute attr)
    {
        if (allHealthBars.TryGetValue(attr, out GameObject bar))
        {
            RectTransform barRect = bar.GetComponent<RectTransform>();
            Vector3 worldPos = attr.transform.position; // 假设 attr 具有 Transform 组件

            Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos + offset); // 将世界坐标转换为屏幕坐标
            if (attr.GetComponent<Monster90005>()!= null || attr.GetComponent<Monster90006>()!= null)
            {
                screenPos = Camera.main.WorldToScreenPoint(worldPos + offset + new Vector3(0f,0.8f,0f));
            }       
            
            barRect.anchoredPosition = screenPos; // 更新血条的位置

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
