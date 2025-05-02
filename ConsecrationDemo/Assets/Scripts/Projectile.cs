using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    public int type; //1-飞行型 2-天降型
    public int startPos;
    public int endPos;
    private SpriteRenderer projectileSprite;
    // Start is called before the first frame update
    void Start()
    {
        projectileSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public IEnumerator Shoot()
    {
        if (type == 1)
        {
            if (endPos > startPos)
            {
                projectileSprite.flipX = true;
                yield return transform.DOMove(SwitchPos.IntToVector2(endPos), 0.2f, false).WaitForCompletion();
            }
        }
        else
        {
            yield return transform.transform.DOMove(SwitchPos.IntToVector2(endPos), 0.2f, false).WaitForCompletion();
        }
    }
}
