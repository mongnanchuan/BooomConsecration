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
    void Start()
    {
/*        BaseAttack.damage = 2;
        BaseAttack.type = 0;
        BaseAttack.range = 1;*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnEnd()
    {
        if(isInPlayerTurn)
        {
            isInPlayerTurn = false;
            //通知MonsterManager行动
        }
    }

}
