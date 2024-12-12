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
    Random,
    Lowest
}

public enum BuffType
{
    Miti,   // 坚硬(减伤25%，受伤时减少1层) 
    Sere,   // 宁静(增加获得的化劲，每层增加1点)
    Dodge,  // 逍遥(增加闪避，每层加6%)
    Rage,   // 暴怒(增加攻击力和受到的伤害，每层增加1点)
    Thorn,  // 天罚(反伤，被攻击时对攻击者造成天罚层数的伤害)
    Vuln,   // 虚损(受到伤害提高, 50%)
    Weak,   // 力竭(减少攻击力, 25%)
    Poison  // 中毒(回合结束减少HP, 减少等同于层数的生命)
}

public enum CardListType
{
    PlayerHold,
    DrawDeck,
    DiscardDeck
}

public enum SceneType
{
    Test,
    Menu,
    Battle,
    Shop,
    Rest
}

public enum AwardType
{
    Card,
    Money,
    Blessing,
}

public enum EnemyEffectType
{
    Damage,
    Shield,
    Spawn,
    Taunt,
    Enhance
}

public enum EnhenceDataType
{
    Buffed,
    Temp,
    TriggerTime,
    Basic,
    
}