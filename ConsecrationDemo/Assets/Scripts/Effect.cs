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
    public int userPos;//ʹ���ߵ�λ��

    public int demage;//�˺�ֵ
    public int heal;//����ֵ
    public List<int> addBuffID;//BUFF�б�
    public List<int> RemoveBuffID;//BUFF�б�
    public Vector2 forceMovePos;//������1=���ң�-1=����0=���ͣ���Ŀ��λ�ñ�ţ�
}


