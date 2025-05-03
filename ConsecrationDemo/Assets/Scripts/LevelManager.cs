using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public CombatManager cm;

    public GameObject[] AltarBlanks;
    public GameObject[] TokenBlanks;
    public GameObject[] AttackAlert;
    public Transform[] AltarCorrectTrans = new Transform[9];
    public GameObject[] AltarIcons = new GameObject[9];
    public Transform[] TokenCorrectTrans = new Transform[9];
    public GameObject[] TokenIcons = new GameObject[9];
    public int levelID;
    public bool Preparing;
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
            AltarCorrectTrans[i] = AltarBlanks[i].transform;
            TokenCorrectTrans[i] = TokenBlanks[i].transform;
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
        Preparing = true;
        foreach (GameObject combatObject in targetCombat)
        {
            combatObject.SetActive(false);
        }
        foreach (GameObject buttonObject in targetButton)
        {
            buttonObject.SetActive(false);
        }
        //levelID++;
    }
    //准备完毕，进入新关卡
    public void ReadyAndStart()
    {
        foreach (GameObject altarIconObject in targetAltarIcon)
        {
            Altar al = altarIconObject.GetComponent<Altar>();
            if(al.index_before < 0)
            {
                //altarIconObject.SetActive(false);
                //提升需要放置所有祭坛
                return;
            }
            else
            {
                if(TokenIcons[al.index_before] != null)
                {
                    al.tokenID = TokenIcons[al.index_before].GetComponent<Token>().currentID;
                }
                else
                {
                    al.tokenID = 0;
                }
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
        Preparing = false;
        cm.isInPlayerTurn = true;
        MonsterManager.Instance.MonsterGroupInit(levelID);
        PlayerPosReport.Instance.attr.healthInit();
    }
}
