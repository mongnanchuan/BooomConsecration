using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WarningFlash : MonoBehaviour
{
    public Color EndColor;
    // Start is called before the first frame update
    void Start()
    {
        DOVirtual.Color(new Color(1f, 1f, 1f, 0.8f), EndColor, 0.4f, c =>
        {
            GetComponent<SpriteRenderer>().material.color = c;
        }).SetLoops(-1, LoopType.Yoyo);
        /*        Sequence s = DOTween.Sequence();
                s.SetDelay(0.2f);
                s.Append(DOVirtual.Color(new Color(1f, 1f, 1f, 0.75f), new Color(1f, 0f, 0f, 0.75f), 0.8f, c => {GetComponent<SpriteRenderer>().material.color = c;}));
                s.AppendInterval(0.2f);
                s.SetLoops(-1, LoopType.Yoyo);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
