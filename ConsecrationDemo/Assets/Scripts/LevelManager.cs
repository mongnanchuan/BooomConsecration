using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public CombatManager cm;
    public GameObject UICanvas;
    public GameObject[] AltarBlanks;
    public GameObject[] TokenBlanks;
    public GameObject[] AttackAlert;
    public Transform[] AltarCorrectTrans = new Transform[9];
    public GameObject[] AltarIcons = new GameObject[9];
    public Transform[] TokenCorrectTrans = new Transform[9];
    public GameObject[] TokenIcons = new GameObject[9];
    public int levelID;
    public int Selecting = 0; //0-未选择 1-选择祭坛中 2-选择信物中
    public GameObject SelectPrefab;
    public bool Preparing;
    public GameObject ConfirmSelectButton;
    public GameObject ReadyButton;
    public GameObject Title;
    public GameObject HPManager;
    public int SelectIndex = 0;
    public List<GameObject> AllTokenIcons = new List<GameObject>();
    public List<GameObject> AllAltarIcons = new List<GameObject>();
    private List<GameObject> NotUseTokenIcons = new List<GameObject>();
    private List<GameObject> NotUseAltarIcons = new List<GameObject>();
    GameObject[] targetAltarIcon;
    GameObject[] targetPrepare;
    GameObject[] targetCombat;
    GameObject[] targetButton;
    GameObject[] targetFloor;
    private GameObject[] ToSpawnObject = new GameObject[2];

    public GameObject defeatPanel;
    public GameObject victoryPanel;

    // Start is called before the first frame update
    void Start()
    {
        cm = GetComponent<CombatManager>();
        //targetAltarIcon = GameObject.FindGameObjectsWithTag("AltarIcon");
        targetPrepare = GameObject.FindGameObjectsWithTag("Prepare");
        targetCombat = GameObject.FindGameObjectsWithTag("Combat");
        targetButton = GameObject.FindGameObjectsWithTag("CombatButton");
        targetFloor = GameObject.FindGameObjectsWithTag("Floor");
        for (int i = 0; i < 9; i++)
        {
            AltarCorrectTrans[i] = AltarBlanks[i].transform;
            TokenCorrectTrans[i] = TokenBlanks[i].transform;
        }
        AltarAndTokenReset();
        PrepareLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if(Selecting > 0 && SelectIndex > 0)
        {
            ConfirmSelectButton.SetActive(true);
        }
        else
        {
            ConfirmSelectButton.SetActive(false);
        }
    }
    //掉落阶段，从当前没获得的祭坛和信物中各选两个
    public void ShowAltarDrop()
    {
        Selecting = 1;
        for(int i = 0; i < 2; i++)
        {
            int random = Random.Range(0, NotUseAltarIcons.Count);
            GameObject selectButton = Instantiate(SelectPrefab, UICanvas.transform);
            selectButton.GetComponent<DropSelect>().dropType = 1;
            selectButton.GetComponent<DropSelect>().Index = i + 1;
            selectButton.GetComponent<DropSelect>().PrefabObject = NotUseAltarIcons[random];
            selectButton.GetComponent<RectTransform>().localPosition = new Vector2(-400 + i*800, 20);
            NotUseAltarIcons.RemoveAt(random);
            if(NotUseAltarIcons.Count == 0)
            {
                break;
            }
        }
    }
    public void ShowTokenDrop()
    {
        GameObject[] targetSelects;
        targetSelects = GameObject.FindGameObjectsWithTag("Select");
        foreach (GameObject select in targetSelects)
        {
            Destroy(select);
        }
        Selecting = 2;
        for (int i = 0; i < 2; i++)
        {
            int random = Random.Range(0, NotUseTokenIcons.Count);
            GameObject selectButton = Instantiate(SelectPrefab, UICanvas.transform);
            selectButton.GetComponent<DropSelect>().dropType = 2;
            selectButton.GetComponent<DropSelect>().Index = i + 1;
            selectButton.GetComponent<DropSelect>().PrefabObject = NotUseTokenIcons[random];
            selectButton.GetComponent<RectTransform>().localPosition = new Vector2(-400 + i * 800, 20);
            NotUseTokenIcons.RemoveAt(random);
            if (NotUseAltarIcons.Count == 0)
            {
                break;
            }
        }

    }
    //准备阶段，供玩家选择掉落与排布祭坛
    public void PrepareLevel()
    {
        Title.GetComponent<Text>().text = "选择你的战利品";
        Preparing = true;
        cm.Player.GetComponent<PlayerManager>().ResetPlayer();
        HPManager.SetActive(false);
        foreach (GameObject combatObject in targetCombat)
        {
            combatObject.SetActive(false);
        }
        foreach (GameObject buttonObject in targetButton)
        {
            buttonObject.SetActive(false);
        }
        targetAltarIcon = GameObject.FindGameObjectsWithTag("AltarIcon");
        //所有altar切回献祭前
        foreach (GameObject altarIconObject in targetAltarIcon)
        {
            Altar al = altarIconObject.GetComponent<Altar>();
            al.CD = ConfigManager.Instance.GetConfig<SkillsConfig>(ConfigManager.Instance.GetConfig<AltarsConfig>(al.currentID).Skill1).cooldown;
            al.isFinished = false;
            AltarBlanks[al.index_before].GetComponentInParent<FloorConfig>().BackToIcon(al);
        }
        ShowAltarDrop();
        
        //levelID++;
    }
    //准备完毕，进入新关卡
    public void ReadyAndStart()
    {
        targetAltarIcon = GameObject.FindGameObjectsWithTag("AltarIcon");
        foreach (GameObject altarIconObject in targetAltarIcon)
        {
            Altar al = altarIconObject.GetComponent<Altar>();
            if(al.index_before < 0)
            {
                //altarIconObject.SetActive(false);
                //提示需要放置所有祭坛
                TipsManager.Instance.ShowTip("需将所有祭坛放置完毕");
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
                al.CD = 0;
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
        ReadyButton.SetActive(false);
        HPManager.SetActive(true);
        MonsterManager.Instance.MonsterGroupInit(levelID);
        PlayerPosReport.Instance.attr.healthInit();
    }

    public void AltarAndTokenReset()
    {
        NotUseAltarIcons.Clear();
        AllAltarIcons.ForEach(i => NotUseAltarIcons.Add(i));
        NotUseTokenIcons.Clear();
        AllTokenIcons.ForEach(i => NotUseTokenIcons.Add(i));
    }

    public void OnComfirmSelect()
    {
        GameObject[] targetSelects;
        GameObject ToSpawn;
        targetSelects = GameObject.FindGameObjectsWithTag("Select");
        foreach (GameObject select in targetSelects)
        {
            if (select.GetComponent<DropSelect>().Index == SelectIndex)
            {
                ToSpawn = select.GetComponent<DropSelect>().PrefabObject;
                if (ToSpawn.CompareTag("AltarIcon"))
                {
                    ToSpawnObject[0] = ToSpawn;
                    SelectIndex = 0;
                    ShowTokenDrop();
                }
                else if(ToSpawn.CompareTag("TokenIcon"))
                {
                    ToSpawnObject[1] = ToSpawn;
                    SelectIndex = 0;
                    ShowInstance();
                }
            }
            //把没选的放回池子
            else
            {
                ToSpawn = select.GetComponent<DropSelect>().PrefabObject;
                if (ToSpawn.CompareTag("AltarIcon"))
                {
                    NotUseAltarIcons.Add(ToSpawn);
                }
                else if (ToSpawn.CompareTag("TokenIcon"))
                {
                    NotUseTokenIcons.Add(ToSpawn);
                }
            }
        }
    }

    public void ShowInstance()
    {
        Title.GetComponent<Text>().text = "排布你的祭坛和信物";
        Selecting = 0;
        GameObject[] targetSelects;
        targetSelects = GameObject.FindGameObjectsWithTag("Select");
        foreach (GameObject select in targetSelects)
        {
            Destroy(select);
        }
        GameObject ToSetAltarIcon = Instantiate(ToSpawnObject[0]);
        ToSetAltarIcon.transform.position = new Vector3(0, 1.5f, 0);
        GameObject ToSetTokenIcon = Instantiate(ToSpawnObject[1]);
        ToSetTokenIcon.transform.position = new Vector3(-5f + (ToSetTokenIcon.GetComponent<Token>().currentID - 20000)*1f, -4.45f, 0);
        ToSpawnObject[0] = null;
        ToSpawnObject[1] = null;
        ReadyButton.SetActive(true);
    }

    public void GameDefeat()
    {
        cm.isInPlayerTurn = false;
        defeatPanel.SetActive(true);
    }

    public void GameVictory()
    {
        cm.isInPlayerTurn = false;
        victoryPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        //Scene scene = SceneManager.GetActiveScene();
        //SceneManager.UnloadSceneAsync(scene);
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }
}
