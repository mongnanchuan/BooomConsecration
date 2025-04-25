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
    public Attribute Taker;
    public Attribute Ganker;

    public int damage;//伤害值
    public int heal;//治疗值
    public List<int> addBuffID;//BUFF列表
    public List<int> RemoveBuffID;//BUFF列表
    public int forceMoveDis;//强制位移的距离（方向用正负号表示）
    public int portalMovePos;//传送移动的位置
    public int collisionDamage; //碰撞伤害
}


