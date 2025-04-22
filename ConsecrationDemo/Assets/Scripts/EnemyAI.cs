using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyAI : MonoBehaviour
{
    private Transform EnemyTf;
    private SpriteRenderer EnemySprite;
    public GameObject BodyObject;
    private Animator BodyAnim;
    public LevelManager lm;
    public CombatManager cm;
    private GameObject[] AttackAlert;
    public List<GameObject> AlertList = new List<GameObject>();
    public List<SkillsConfig> Skills = new List<SkillsConfig>();
    public int stopDis = 2;
    private GameObject canvasSkill;
    public int ActMark = 0;//1-ÒÆ¶¯ 2-¼¼ÄÜ 
    private bool ActShowing = false;
    public bool IsFaceRight = false;
    // Start is called before the first frame update
    void Start()
    {
        lm = GameObject.FindGameObjectsWithTag("System")[0].GetComponent<LevelManager>();
        cm = GameObject.FindGameObjectsWithTag("System")[0].GetComponent<CombatManager>();
        AttackAlert = lm.AttackAlert;
        EnemyTf = GetComponent<Transform>();
        EnemySprite = GetComponent<SpriteRenderer>();
        canvasSkill = transform.Find("CanvasSkill").gameObject;
        BodyObject = transform.Find("Body").gameObject;
        BodyAnim = BodyObject.GetComponent<Animator>();
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
        }
        if(!cm.isInPlayerTurn && ActShowing)
        {
            ExecuteAct();
        }
    }

    public void ShowAttackAlert(SkillsConfig targetSkill)
    {
        BodyAnim.SetTrigger("ChargeBegin");
        for(int i = 0; i < targetSkill.range; i++)
        {
            GameObject alertHint = Instantiate(AttackAlert[targetSkill.type], canvasSkill.transform);
            if(!IsFaceRight)
            {
                alertHint.transform.localPosition = new Vector3(-110 * (i + 1), -92, 0);
            }
            else
            {
                alertHint.transform.localPosition = new Vector3(110 * (i + 1), -92, 0);
            }
            AlertList.Add(alertHint);
        }
    }

    public void Attack(SkillsConfig targetSkill)
    {
        ActShowing = false;
        BodyAnim.SetTrigger("AttackBegin");
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Combat");
        if(targetSkill.type == 0)
        {
            foreach (GameObject character in characters)
            {
                if (character != gameObject)
                {
                    float dis_now = EnemyTf.position.x - character.GetComponent<Transform>().position.x;
                    if (!IsFaceRight && dis_now >= 0 && dis_now <= targetSkill.range * 1.5f)
                    {
                        character.GetComponent<Attribute>().Damage(targetSkill.damage);
                    }
                    else if (IsFaceRight && dis_now <= 0 && dis_now >= -targetSkill.range * 1.5f)
                    {
                        character.GetComponent<Attribute>().Damage(targetSkill.damage);
                    }
                }
            }
        }
        foreach(GameObject alertHint in AlertList)
        {
            Destroy(alertHint);
        }
        AlertList.Clear();
        Invoke("NextTurn", 0.5f);
    }

    private void NextTurn()
    {
        if (cm.Player.GetComponent<Transform>().position.x - EnemyTf.position.x > 0)
        {
            //EnemySprite.flipX = true;
            IsFaceRight = true;
            BodyObject.transform.DOLocalRotate(new Vector3(0, 180, 0), 0.2f);
        }
        else
        {
            //EnemySprite.flipX = false;
            IsFaceRight = false;
            BodyObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
        }
        cm.isInPlayerTurn = true;
    }

    public void EnemyMove(int d, string Type)
    {
        float NewPosX;
        bool HasBlock = false;
        if(!IsFaceRight)
        {
            NewPosX = EnemyTf.position.x - d * 1.5f;
        }
        else
        {
            NewPosX = EnemyTf.position.x + d * 1.5f;
        }
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Combat");
        if (NewPosX <= 6f && NewPosX >= -6f)
        {
            foreach (GameObject character in characters)
            {
                if(character.name != "MainRole" && character.GetComponent<Transform>().position.x == NewPosX)
                {
                    HasBlock = true;
                    break;
                }
            }
            if(!HasBlock)
            {
                //EnemyTf.position = new Vector3(NewPosX, EnemyTf.position.y, EnemyTf.position.z);
                GetComponent<Attribute>().PosNow += d;
                if(Type == "Jump")
                {
                    EnemyTf.DOLocalJump(new Vector3(NewPosX, EnemyTf.position.y, EnemyTf.position.z), 0.5f, 1, 0.2f, false);
                }
                else if(Type == "Slide")
                {
                    EnemyTf.DOMove(new Vector3(NewPosX, EnemyTf.position.y, EnemyTf.position.z), 0.2f, false);
                }
            }
        }
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
            EnemyMove(1, "Jump");
            ActShowing = false;
            Invoke("NextTurn", 0.3f);
        }
    }
}
