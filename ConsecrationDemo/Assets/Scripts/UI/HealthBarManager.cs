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
            // 更新血条位置和宽度
            RectTransform barRect = bar.GetComponent<RectTransform>();
            barRect.anchoredPosition = SwitchPos.IntToUIPosition(attr.PosNow);

            for (int i = 0; i < bar.transform.childCount; i++)
            {
                if(i < attr.HP)
                {
                    bar.transform.GetChild(i).GetComponent<Image>().color = Color.white;
                }
                else
                {
                    bar.transform.GetChild(i).GetComponent<Image>().color = Color.black;
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
