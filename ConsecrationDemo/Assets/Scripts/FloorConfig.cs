using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorConfig : MonoBehaviour
{
    public GameObject altarObject;
    public GameObject godObject;
    public GameObject crackObject;
    // Start is called before the first frame update
    void Start()
    {
        altarObject = transform.Find("altar").gameObject;
        godObject = transform.Find("god").gameObject;
        crackObject = transform.Find("crack").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowHalfGod(Altar targetAltar)
    {
        crackObject.SetActive(true);
        godObject.GetComponent<SpriteRenderer>().sprite = targetAltar.GodImage;
        godObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        godObject.SetActive(true);
    }

    public void ShowCompleteGod(Altar targetAltar)
    {
        crackObject.SetActive(false);
        godObject.GetComponent<SpriteRenderer>().sprite = targetAltar.GodImage;
        godObject.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
    }
}
