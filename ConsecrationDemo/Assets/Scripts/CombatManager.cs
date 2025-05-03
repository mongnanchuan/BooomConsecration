using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public bool isInPlayerTurn = false;
    public GameObject Player;
    private Transform PlayerTf;
    private SpriteRenderer PlayerSprite;
    private SkillsConfig BaseAttack = new SkillsConfig();
    private LevelManager lm;
    // Start is called before the first frame update
    public Attribute attr;
    public GameObject[] FX;
    void Start()
    {
        lm = GetComponent<LevelManager>();
        if (MonsterManager.Instance != null)
        {
            MonsterManager.Instance.OnPlayerTurnStart += TurnStart;
        }
        attr = PlayerPosReport.Instance.attr;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBarManager.Instance.UpdateAllHealthBars();
    }

    public IEnumerator TurnEnd()
    {
        if(attr.additionalTurn > 0)
        {
            attr.additionalTurn--;
            CDUpdate();
            yield break;
        }
        else
        {
            if (isInPlayerTurn)
                isInPlayerTurn = false;
                CDUpdate();
            yield return new WaitForSeconds(0.5f);

            if(MonsterManager.Instance.isShowing)
            {
                MonsterManager.Instance.SetNewMonsters(false);
            }
            else
            {
                MonsterManager.Instance.StartMonsterTurn(PlayerPosReport.Instance.GetPlayerPos());
            }

        }
    }

    public void TurnStart()
    {
        isInPlayerTurn = true;
    }

    public void CDUpdate()
    {
        for(int i = 0; i < 9; i ++)
        {
            if(lm.AltarIcons[i] != null)
            {
                Altar targetAltar = lm.AltarIcons[i].GetComponent<Altar>();
                if (targetAltar.CD > 0)
                {
                    targetAltar.CD--;
                }
            }
        }
    }

    public void ShowFX(int type, int targetPos)
    {
        //type:1-¼ÀÌ³¹¥»÷ 2-Boss¼¼ÄÜ 3-Íæ¼Ò¹¥»÷
        GameObject SetFX = Instantiate(FX[type]);
        SetFX.transform.position = SwitchPos.IntToVector2(targetPos) + new Vector2(0, 0.7f);
    }

}
