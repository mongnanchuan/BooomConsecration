using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class IntroductionHelper : MonoBehaviour, IPointerClickHandler
{
    public List<Sprite> Sprites;

    private int index = 0;

    private void OnEnable()
    {
        index = 0;
        GetComponent<Image>().sprite = Sprites[0];
    }

    private void OnDisable()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        index++;
        if (index < Sprites.Count)
        {
            GetComponent<Image>().sprite = Sprites[index];
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
