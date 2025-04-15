using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnmeyAI : MonoBehaviour
{
    private Transform EnemyTf;
    private SpriteRenderer EnemySprite;
    public LevelManager lm;
    public CombatManager cm;
    private GameObject[] AttackAlert;
    public List<GameObject> AlertList = new List<GameObject>();
    public List<SkillsConfig> Skills = new List<SkillsConfig>();
    public int stopDis = 2;
    private GameObject canvasSkill;
    public int ActMark = 0;//1-ÒÆ¶¯ 2-¼¼ÄÜ 
    private bool ActShowing = false;
    // Start is called before the first frame update
    void Start()
    {
        lm = GameObject.FindGameObjectsWithTag("System")[0].GetComponent<LevelManager>();
        cm = GameObject.FindGameObjectsWithTag("System")[0].GetComponent<CombatManager>();
        AttackAlert = lm.AttackAlert;
        EnemyTf = GetComponent<Transform>();
        EnemySprite = GetComponent<SpriteRenderer>();
        canvasSkill = transform.Find("CanvasSkill").gameObject;
        SkillsConfig test = new SkillsConfig();
        test.type = 0;
        test.range = 2;
        test.damage = 1;
        Skills.Add(test);
        ShowAct();
    }

    // Update is called once per frame
    void Update()
    {
        if(cm.isInPlayerTurn && !ActShowing)
        {
            ShowAct();
            //ShowAttackAlert(Skills[0]);
        }
        if(!cm.isInPlayerTurn && ActShowing)
        {
            ExecuteAct();
            //Attack(Skills[0]);
        }
    }

    public void ShowAttackAlert(SkillsConfig targetSkill)
    {
        for(int i = 0; i < targetSkill.range; i++)
        {
            GameObject alertHint = Instantiate(AttackAlert[targetSkill.type], canvasSkill.transform);
            if(!EnemySprite.flipX)
            {
                alertHint.transform.localPosition = new Vector3(-110 * (i + 1), -92, 0);
            }
            else
            {
                alertHint.transform.localPosition = new Vector3(110 * (i + 1), -92, 0);
            }
            AlertList.Add(alertHint);
        }
        //ActShowing = true;
    }

    public void Attack(SkillsConfig targetSkill)
    {
        ActShowing = false;
        float dis_now = Mathf.Abs(cm.Player.GetComponent<Transform>().position.x - EnemyTf.position.x);
        if(dis_now <= targetSkill.range * 1.5f)
        {
            cm.Player.GetComponent<Attribute>().Damage(targetSkill.damage);
        }
        foreach(GameObject alertHint in AlertList)
        {
            Destroy(alertHint);
        }
        AlertList.Clear();
        Invoke("NextTurn", 1f);
    }

    private void NextTurn()
    {
        cm.isInPlayerTurn = true;
    }

    public void EnemyMove(int d)
    {
        ActShowing = false;
        float NewPosX;
        if(!EnemySprite.flipX)
        {
            NewPosX = EnemyTf.position.x - d * 1.5f;
        }
        else
        {
            NewPosX = EnemyTf.position.x + d * 1.5f;
        }
        if (NewPosX <= 6f && NewPosX >= -6f)
        {
            EnemyTf.position = new Vector3(NewPosX, EnemyTf.position.y, EnemyTf.position.z);
        }
        Invoke("NextTurn", 1f);
    }

    public void ShowAct()
    {
        float dis_now = Mathf.Abs(cm.Player.GetComponent<Transform>().position.x - EnemyTf.position.x);
        if (dis_now <= stopDis * 1.5f)
        {
            ActMark = 2;
            ShowAttackAlert(Skills[0]);
        }
        else
        {
            ActMark = 1;
        }
        ActShowing = true;
    }

    public void ExecuteAct()
    {
        if(ActMark == 2)
        {
            Attack(Skills[0]);
        }
        else if(ActMark == 1)
        {
            EnemyMove(1);
        }
    }
}
