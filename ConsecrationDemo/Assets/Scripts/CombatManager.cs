using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public bool isInPlayerTurn = false;
    public GameObject Player;
    private Transform PlayerTf;
    private SpriteRenderer PlayerSprite;
    // Start is called before the first frame update
    void Start()
    {
        PlayerTf = Player.GetComponent<Transform>();
        PlayerSprite = Player.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //¥¶¿Ì ‰»Î
        if(isInPlayerTurn)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if(PlayerSprite.flipX)
                {
                    PlayerSprite.flipX = false;
                }
                else
                {
                    PlayerMove(-1);
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if(!PlayerSprite.flipX)
                {
                    PlayerSprite.flipX = true;
                }
                else
                {
                    PlayerMove(+1);
                }
            }
        }


    }

    public void TurnEnd()
    {
        if(isInPlayerTurn)
        {
            isInPlayerTurn = false;
        }
        else
        {
            isInPlayerTurn = true;
        }
    }

    public void PlayerMove(int d)
    {
        float NewPosX = PlayerTf.position.x + d * 1.5f;
        if(NewPosX <= 6f && NewPosX >= -6f)
        {
            PlayerTf.position = new Vector3(NewPosX, PlayerTf.position.y, PlayerTf.position.z);
            TurnEnd();
        }
    }
}
