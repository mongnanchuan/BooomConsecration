using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorConfig : MonoBehaviour
{
    public GameObject altarObject;
    public GameObject godObject;
    public GameObject crackMaskObject;
    // Start is called before the first frame update
    void Start()
    {
        altarObject = transform.Find("altar").gameObject;
        godObject = transform.Find("god").gameObject;
        crackMaskObject = transform.Find("crack").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowHalfGod(Altar targetAltar)
    {
        altarObject.SetActive(false);
        targetAltar.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        crackMaskObject.SetActive(true);
        godObject.GetComponent<SpriteRenderer>().sprite = targetAltar.GodImage;
        godObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        godObject.SetActive(true);
    }

    public void ShowCompleteGod(Altar targetAltar)
    {
        crackMaskObject.SetActive(false);
        godObject.GetComponent<SpriteRenderer>().sprite = targetAltar.GodImage;
        godObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
    }

    public void BackToIcon(Altar targetAltar)
    {
        godObject.SetActive(false);
        altarObject.SetActive(true);
        crackMaskObject.SetActive(false);
        targetAltar.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        targetAltar.SkillIndex = 0;
    }
}
