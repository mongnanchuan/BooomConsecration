using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosReport : MonoBehaviour
{

    public static PlayerPosReport Instance { get; private set; }

    public Attribute attr;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        attr = GetComponent<Attribute>();
    }

    public int GetPlayerPos()
    {
        return attr.PosNow;
    }
    


    
}
