using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Effect_Type
{
    ForceMove,//强制移动
    MakeDamage,//伤害
    Healing,//治疗
    AddBuff,//加Buff/Debuff
    RemoveBuff,//消Buff/Debuff
}


public class Effect
{
    public Effect_Type type;//效果类型
    public int userPos;//使用者的位置

    public int demage;//伤害值
    public int heal;//治疗值
    public List<int> addBuffID;//BUFF列表
    public List<int> RemoveBuffID;//BUFF列表
    public Vector2 forceMovePos;//（方向（1=向右，-1=向左，0=传送），目标位置编号）
}


