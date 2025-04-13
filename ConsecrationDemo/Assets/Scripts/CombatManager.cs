using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public bool isInPlayerTurn = false;
    public GameObject Player;
    private Transform PlayerTf;
    // Start is called before the first frame update
    void Start()
    {
        PlayerTf = Player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //¥¶¿Ì ‰»Î
        if(isInPlayerTurn)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                PlayerMove(-1);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                PlayerMove(+1);
            }
        }


    }

    public void PlayerTurnEnd()
    {

    }

    public void PlayerMove(int d)
    {
        float NewPosX = PlayerTf.position.x + d * 1.5f;
        if(NewPosX <= 4.5f && NewPosX >= -4.5f)
        {
            PlayerTf.position = new Vector3(NewPosX, PlayerTf.position.y, PlayerTf.position.z);
        }
    }
}
