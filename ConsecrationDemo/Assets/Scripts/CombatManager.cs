using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public bool isInPlayerTurn = false;
    public GameObject Player;
    private Transform PlayerTf;
    private SpriteRenderer PlayerSprite;
    private SkillsConfig BaseAttack = new SkillsConfig();
    private LevelManager lm;
    // Start is called before the first frame update
    public Attribute attr;
    void Start()
    {
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
            yield break;
        }
        else
        {
            if (isInPlayerTurn)
                isInPlayerTurn = false;
            yield return new WaitForSeconds(0.5f);
            MonsterManager.Instance.StartMonsterTurn(PlayerPosReport.Instance.GetPlayerPos());
        }
    }

    public void TurnStart()
    {
        isInPlayerTurn = true;
    }

}
