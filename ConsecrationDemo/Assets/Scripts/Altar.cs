using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Altar : MonoBehaviour
{
    public Vector3 startPos;
    private Collider2D collider2D;
    private Button button;
    public int currentID;
    public int index_before = -1;
    public LevelManager lm;
    private GameObject InfoCanvas;
    private bool isDragging = false;
    public bool isFinished = false;
    public List<SkillBase> Skills = new List<SkillBase>();
    public int SkillIndex = 0;
    public int CD;
    public GameObject CDText;
    public Sprite GodImage;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        collider2D = GetComponent<Collider2D>();
        InfoCanvas = transform.GetChild(0).gameObject;
        lm = GameObject.FindWithTag("System").GetComponent<LevelManager>();
        string skillName1 = "Skill" + ConfigManager.Instance.GetConfig<AltarsConfig>(currentID).Skill1.ToString();
        string skillName2 = "Skill" + ConfigManager.Instance.GetConfig<AltarsConfig>(currentID).Skill2.ToString();
        var skill1 = (SkillBase)System.Activator.CreateInstance(Type.GetType(skillName1));
        var skill2 = (SkillBase)System.Activator.CreateInstance(Type.GetType(skillName1));
        skill1.Init();
        skill2.Init();
        /*        SkillsConfig skill1 = ConfigManager.Instance.GetConfig<SkillsConfig>(ConfigManager.Instance.GetConfig<AltarsConfig>(currentID).Skill1);
                SkillsConfig skill2 = ConfigManager.Instance.GetConfig<SkillsConfig>(ConfigManager.Instance.GetConfig<AltarsConfig>(currentID).Skill2);*/
        Skills.Add(skill1);
        Skills.Add(skill2);
        CD = 0;
        string imagePath = "Gods/" + ConfigManager.Instance.GetConfig<AltarsConfig>(currentID).name + "god";
        GodImage = Resources.Load(imagePath, typeof(Sprite)) as Sprite ;
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
            for (int i = 0; i < 9; i++)
            {
                if (near(lm.AltarCorrectTrans[i]))
                {
                    move(i);
                    isDragging = false;
                    return;
                }
            }
            //回到初始位置
            move(-1);
            isDragging = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //CDText.GetComponent<Text>().text = CD.ToString();
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
        return (Mathf.Abs(transform.position.x - correctTrans.position.x) <= 0.75f &&
           Mathf.Abs(transform.position.y - correctTrans.position.y) <= 0.75f);
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
                    lm.AltarIcons[id].transform.position = new Vector2(lm.AltarCorrectTrans[index_before].position.x, lm.AltarCorrectTrans[index_before].position.y);
                    lm.AltarIcons[id].GetComponent<Altar>().index_before = index_before;
                    lm.AltarIcons[id].GetComponent<Altar>().startPos = lm.AltarIcons[id].transform.position;
                }
                else
                {
                    lm.AltarIcons[id].transform.position = startPos;
                    lm.AltarIcons[id].GetComponent<Altar>().index_before = -1;
                    lm.AltarIcons[id].GetComponent<Altar>().startPos = lm.AltarIcons[id].transform.position;
                }
            }
            lm.AltarIcons[id] = gameObject;
            index_before = id;
            transform.position = new Vector2(lm.AltarCorrectTrans[id].position.x, lm.AltarCorrectTrans[id].position.y);
            startPos = transform.position;
        }
        else
        {
            transform.position = startPos;
            
        }
    }

    public void UseSkill(GameObject Trigger)
    {
        Transform TriggerTf = Trigger.GetComponent<Transform>();
        SpriteRenderer TriggerSprite = Trigger.GetComponent<SpriteRenderer>();
        GameObject[] characters = GameObject.FindGameObjectsWithTag("Combat");
        Skills[SkillIndex].TakeEffect(Trigger);
/*        switch (Skill.id)
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
        }*/
        CD = Skills[0].skill.cooldown;
    }

    public void Sacrifice()
    {
        if(SkillIndex == 0)
        {
            SkillIndex++;
        }

    }
}
