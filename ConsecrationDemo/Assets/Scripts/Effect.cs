using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Effect_Type
{
    ForceMove,//ǿ���ƶ�
    MakeDamage,//�˺�
    Healing,//����
    AddBuff,//��Buff/Debuff
    RemoveBuff,//��Buff/Debuff
}


public class Effect
{
    public Effect_Type type;//Ч������
    public GameObject Taker;

    public int damage;//�˺�ֵ
    public int heal;//����ֵ
    public List<int> addBuffID;//BUFF�б�
    public List<int> RemoveBuffID;//BUFF�б�
    public int forceMoveDis;
    public int portalMovePos;
}


