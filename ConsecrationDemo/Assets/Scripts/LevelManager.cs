using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public CombatManager cm;
    public MonsterManager mm;
    public GameObject[] AltarBlanks;
    public GameObject[] AttackAlert;
    public Transform[] CorrectTrans = new Transform[9];
    public GameObject[] AltarIcons = new GameObject[9];
    public int levelID = 0;
    GameObject[] targetAltarIcon;
    GameObject[] targetPrepare;
    GameObject[] targetCombat;
    GameObject[] targetButton;

    // Start is called before the first frame update
    void Start()
    {
        cm = GetComponent<CombatManager>();
        targetAltarIcon = GameObject.FindGameObjectsWithTag("AltarIcon");
        targetPrepare = GameObject.FindGameObjectsWithTag("Prepare");
        targetCombat = GameObject.FindGameObjectsWithTag("Combat");
        targetButton = GameObject.FindGameObjectsWithTag("CombatButton");
        for (int i = 0; i < 9; i++)
        {
            CorrectTrans[i] = AltarBlanks[i].transform;
        }
        PrepareLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //准备阶段，供玩家排布祭坛
    public void PrepareLevel()
    {
        foreach (GameObject combatObject in targetCombat)
        {
            combatObject.SetActive(false);
        }
        foreach (GameObject buttonObject in targetButton)
        {
            buttonObject.SetActive(false);
        }
        levelID++;
    }
    //准备完毕，进入新关卡
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
        foreach (GameObject buttonObject in targetButton)
        {
            buttonObject.SetActive(true);
        }
        foreach (GameObject prepareObject in targetPrepare)
        {
            prepareObject.SetActive(false);
        }
/*        mm.levelID = levelID;
        mm.MonsterGroupInit();*/
    }
}
