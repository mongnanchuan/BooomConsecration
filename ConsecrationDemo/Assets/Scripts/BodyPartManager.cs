using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartManager : MonoBehaviour
{
    public GameObject[] SpecialPart1;
    public GameObject[] SpecialPart2;
    public GameObject[] SpecialPart3;
    public GameObject[] HitPart;
    public int PhaseIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var part in SpecialPart2)
        {
            part.SetActive(false);
        }
        foreach (var part in SpecialPart3)
        {
            part.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnToSecond()
    {
        SpecialPart1[0].SetActive(false);
        SpecialPart1[1].SetActive(false);
        SpecialPart1[4].SetActive(false);
        foreach (var part in SpecialPart2)
        {
            part.SetActive(true);
        }
    }

    public void TurnToThird()
    {
        SpecialPart1[2].SetActive(false);
        SpecialPart1[3].SetActive(false);
        SpecialPart1[5].SetActive(false);
        SpecialPart2[0].SetActive(false);
        SpecialPart2[2].SetActive(false);
        SpecialPart2[3].SetActive(false);
        SpecialPart2[4].SetActive(false);
        foreach (var part in SpecialPart3)
        {
            part.SetActive(true);
        }
    }

    public void BackToOrigin()
    {
        foreach (var part in SpecialPart2)
        {
            part.SetActive(false);
        }
        foreach (var part in SpecialPart3)
        {
            part.SetActive(false);
        }
        foreach (var part in SpecialPart1)
        {
            part.SetActive(true);
        }
    }

    public void ShowHitFace()
    {
        if(PhaseIndex == 3)
        {
            HitPart[1].SetActive(true);
        }
        else
        {
            HitPart[0].SetActive(true);
        }
        //Invoke("HideHitFace", 0.5f);
    }

    public void HideHitFace()
    {
        foreach (var part in HitPart)
        {
            part.SetActive(false);
        }
    }

    public void ShowDie()
    {
        if(PhaseIndex != 3)
        {
            HitPart[2].SetActive(true);
            SpecialPart1[5].SetActive(false);
        }
        else
        {
            SpecialPart3[6].SetActive(false);
        }
    }
}
