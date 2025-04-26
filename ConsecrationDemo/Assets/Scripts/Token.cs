using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Token : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        collider2D = GetComponent<Collider2D>();
        InfoCanvas = transform.GetChild(0).gameObject;
        lm = GameObject.FindWithTag("System").GetComponent<LevelManager>();
        CD = 0;
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
                if (near(lm.TokenCorrectTrans[i]))
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
            lm.TokenIcons[index_before] = null;
        }
        if (id >= 0)
        {
            if (lm.TokenIcons[id] != null)
            {
                if (index_before >= 0)
                {
                    lm.TokenIcons[index_before] = lm.TokenIcons[id];
                    lm.TokenIcons[id].transform.position = new Vector2(lm.TokenCorrectTrans[index_before].position.x, lm.TokenCorrectTrans[index_before].position.y);
                    lm.TokenIcons[id].GetComponent<Token>().index_before = index_before;
                }
                else
                {
                    lm.TokenIcons[id].transform.position = lm.TokenIcons[id].GetComponent<Token>().startPos;
                    lm.TokenIcons[id].GetComponent<Token>().index_before = -1;
                }
            }
            lm.TokenIcons[id] = gameObject;
            index_before = id;
            transform.position = new Vector2(lm.TokenCorrectTrans[id].position.x, lm.TokenCorrectTrans[id].position.y);
        }
        else
        {
            transform.position = startPos;
            index_before = -1;
        }
    }
}
