using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SwitchPos
{
    public static Vector2 IntToVector2(int number)
    {
        float Xpos = (number - 4) * 1.5f;
        Vector2 position = new Vector2(Xpos,0f);
        return position;
    }
}
