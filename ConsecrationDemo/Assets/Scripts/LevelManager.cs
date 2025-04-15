using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public CombatManager cm;
    public GameObject[] AltarBlanks;
    public GameObject[] AttackAlert;
    public Transform[] CorrectTrans = new Transform[7];
    public GameObject[] AltarIcons = new GameObject[7];
    GameObject[] targetAltarIcon;
    GameObject[] targetPrepare;
    GameObject[] targetCombat;

    // Start is called before the first frame update
    void Start()
    {
        cm = GetComponent<CombatManager>();
        targetAltarIcon = GameObject.FindGameObjectsWithTag("AltarIcon");
        targetPrepare = GameObject.FindGameObjectsWithTag("Prepare");
        targetCombat = GameObject.FindGameObjectsWithTag("Combat");
        foreach (GameObject combatObject in targetCombat)
        {
            combatObject.SetActive(false);
        }
        for (int i = 0; i < 7; i++)
        {
            CorrectTrans[i] = AltarBlanks[i].transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadyAndStart()
    {
        cm.isInPlayerTurn = true;
        foreach (GameObject altarIconObject in targetAltarIcon)
        {
            Altar al = altarIconObject.GetComponent<Altar>();
            if(al.index_before < 0)
            {
                altarIconObject.SetActive(false);
            }
            else
            {
                al.isFinished = true;
            }
        }
        foreach (GameObject combatObject in targetCombat)
        {
            combatObject.SetActive(true);
        }
        foreach (GameObject prepareObject in targetPrepare)
        {
            prepareObject.SetActive(false);
        }
    }
}
