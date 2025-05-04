using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour
{

    public static ShadowManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private Dictionary<Attribute, GameObject> allShadows = new Dictionary<Attribute, GameObject>();
    public GameObject shadowPrefab;
    public void ShadowInit(Attribute attr)
    {
        if (!allShadows.ContainsKey(attr))
        {
            GameObject shadow = Instantiate(shadowPrefab, this.transform); 
            allShadows[attr] = shadow;
            UpdateShadow(attr); 
        }
    }

    public void UpdateShadow(Attribute attr)
    {
        if (allShadows.TryGetValue(attr, out GameObject shadow))
        {
            Vector3 worldPos = attr.transform.position;
            shadow.transform.position = new Vector3(worldPos.x, 0.2f, worldPos.y);
        }
    }

    public void RemoveShadow(Attribute attr)
    {
        if (allShadows.TryGetValue(attr, out GameObject shadow))
        {
            Destroy(shadow);
            allShadows.Remove(attr);
        }
    }

    public void UpdateAllShadows()
    {
        foreach (var shadow in allShadows.Keys)
        {
            UpdateShadow(shadow);
        }
    }

    private void Update()
    {
        UpdateAllShadows();
    }
}
