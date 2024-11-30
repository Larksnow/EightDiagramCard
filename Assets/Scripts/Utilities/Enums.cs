using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Yin,
    Yang,
    Any,
    ShaoYin,
    ShaoYang,
    LaoYin,
    LaoYang
}

public enum DiagramSO
{
    // 乾
    Qian,
    // 坤
    Kun,
    // 震
    Zhen,
    // 巽
    Xun,
    // 坎
    Kan,
    // 离
    Li,
    // 兑
    Dui,
    // 艮
    Gen
}

public enum EffectTargetType
{
    Self,
    All,
    Single
}

public enum BuffType
{
    Miti,
    Sere,
    Dodge,
    Rage,
    Thorn,
    Vuln,
    Weak,
    Poison
}