using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    public int type; //1-飞行型 2-天降型
    public int startPos;
    public int endPos;
    public SpriteRenderer projectileSprite;
    // Start is called before the first frame update
    void Start()
    {
        //projectileSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public IEnumerator Shoot(Vector2 offect)
    {
        if (type == 1)
        {
            if (endPos > startPos)
            {
                //Debug.Log(endPos);
                projectileSprite.flipX = true;
                yield return transform.DOMove(SwitchPos.IntToVector2(endPos) + offect, 0.2f, false).WaitForCompletion();
                Destroy(this.gameObject);
            }
            else
            {
                yield return transform.DOMove(SwitchPos.IntToVector2(endPos) + offect, 0.2f, false).WaitForCompletion();
                Destroy(this.gameObject);
            }
        }
        else
        {
            Vector2 startUnder = SwitchPos.IntToVector2(startPos);
            Vector2 startUp = startUnder + new Vector2(0, 6);
            Vector2 endUnder = SwitchPos.IntToVector2(endPos);
            Vector2 endUp = endUnder + new Vector2(0, 6);
            transform.position = startUnder;
            yield return transform.DOMove(startUp, 0.2f, false).WaitForCompletion();
            transform.position = endUp;
            transform.localScale = new Vector3(1, -1, 1);
            yield return transform.DOMove(endUnder, 0.2f, false).WaitForCompletion();
            Destroy(this.gameObject);
        }
    }
}
