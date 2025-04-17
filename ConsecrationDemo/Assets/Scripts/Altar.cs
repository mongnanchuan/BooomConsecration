using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Altar : MonoBehaviour
{
    public Vector3 startPos;
    private Collider2D collider2D;
    private Button button;
    public int Type;
    public int index_before = -1;
    public LevelManager lm;
    private GameObject InfoCanvas;
    private bool isDragging = false;
    public bool isFinished = false;
    public List<SkillsConfig> Skills = new List<SkillsConfig>();
    public int SkillIndex = 0;
    public int CD;
    private GameObject CDText;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        collider2D = GetComponent<Collider2D>();
        InfoCanvas = transform.GetChild(0).gameObject;
        lm = GameObject.FindWithTag("System").GetComponent<LevelManager>();
        SkillsConfig test1 = new SkillsConfig();
        test1.type = 10001;
        test1.damage = 2;
        test1.cooldown = 3;
        Skills.Add(test1);
        SkillsConfig test2 = new SkillsConfig();
        test2.type = 10002;
        test2.damage = 2;
        Skills.Add(test2);
        CD = 0;
        CDText = transform.Find("Canvas1/CDText").gameObject;
    }
    private void OnMouseDrag()
    {
        if (!isFinished)
        {
            isDragging = true;
            InfoCanvas.SetActive(false);
            transform.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                             Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        }
    }
    private void OnMouseUp()
    {
        if (!isFinished)
        {
            for (int i = 0; i < 7; i++)
            {
                if (near(lm.CorrectTrans[i]))
                {
                    move(i);
                    isDragging = false;
                    return;
                }
            }
            move(-1);
            isDragging = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CDText.GetComponent<Text>().text = CD.ToString();
    }

    private void OnMouseEnter()
    {
        if (!isDragging)
        {
            InfoCanvas.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (!isDragging)
        {
            InfoCanvas.SetActive(false);
        }
    }

    private bool near(Transform correctTrans)
    {
        return (Mathf.Abs(transform.position.x - correctTrans.position.x) <= 0.5f &&
           Mathf.Abs(transform.position.y - correctTrans.position.y) <= 0.5f);
    }

    private void move(int id)
    {
        if (index_before >= 0)
        {
            lm.AltarIcons[index_before] = null;
        }
        if (id >= 0)
        {
            if (lm.AltarIcons[id] != null)
            {
                if (index_before >= 0)
                {
                    lm.AltarIcons[index_before] = lm.AltarIcons[id];
                    lm.AltarIcons[id].transform.position = new Vector2(lm.CorrectTrans[index_before].position.x, lm.CorrectTrans[index_before].position.y);
                    lm.AltarIcons[id].GetComponent<Altar>().index_before = index_before;
                }
                else
                {
                    lm.AltarIcons[id].transform.position = lm.AltarIcons[id].GetComponent<Altar>().startPos;
                    lm.AltarIcons[id].GetComponent<Altar>().index_before = -1;
                }
            }
            lm.AltarIcons[id] = gameObject;
            index_before = id;
            transform.position = new Vector2(lm.CorrectTrans[id].position.x, lm.CorrectTrans[id].position.y);
        }
        else
        {
            transform.position = startPos;
            index_before = -1;
        }
    }

    public void UseSkill(GameObject Trigger)
    {
        Transform TriggerTf = Trigger.GetComponent<Transform>();
        SpriteRenderer TriggerSprite = Trigger.GetComponent<SpriteRenderer>();
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Combat");
        SkillsConfig Skill = Skills[SkillIndex];
        switch (Skill.type)
        {
            case 10001:
                foreach (GameObject character in characters)
                {
                    float dis_now = TriggerTf.position.x - character.GetComponent<Transform>().position.x;
                    if (!TriggerSprite.flipX && dis_now >= 0 && dis_now <= 1 * 1.5f)
                    {
                        character.GetComponent<Attribute>().Damage(Skill.damage);
                    }
                    else if (TriggerSprite.flipX && dis_now <= 0 && dis_now >= -1 * 1.5f)
                    {
                        character.GetComponent<Attribute>().Damage(Skill.damage);
                    }
                }
                break;
            case 10002:
                foreach (GameObject character in characters)
                {
                    float dis_now = Mathf.Abs(TriggerTf.position.x - character.GetComponent<Transform>().position.x);
                    if (dis_now <= 1 * 1.5f)
                    {
                        character.GetComponent<Attribute>().Damage(Skill.damage);
                    }
                }
                SkillIndex = 0;
                break;
        }
        CD = Skills[0].cooldown;
    }

    public void Sacrifice()
    {
        if(SkillIndex == 0)
        {
            SkillIndex++;
        }

    }
}
