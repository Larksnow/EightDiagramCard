using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Yin,
    Yang
}

public enum DiagramType
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