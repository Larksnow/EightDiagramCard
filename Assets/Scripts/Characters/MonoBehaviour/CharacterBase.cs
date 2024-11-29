using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class CharacterBase : MonoBehaviour
{
    public struct Damage
    {
        public Vector3 position;
        public int amount;
        public Damage(Vector3 pos, int amount)
        {
            this.position = pos;
            this.amount = amount;
        }
    }

    [Header("Broadcast Events")]
    public ObjectEventSO takeDamageEvent;

    [Header("Attributes")]
    public int maxHP;
    public int currentHP;
    public int currentShield;   // 化劲(抵消伤害)

    [Header("Buffs")]
    public int currentMiti;     // 坚硬(减伤25%，受伤时减少1层) 
    public int currentSere;     // 宁静(增加获得的化劲，每层增加1点)
    public int currentDodge;    // 逍遥(增加闪避，每层加6%)
    public int currentRage;     // 暴怒(增加攻击力和受到的伤害，每层增加1点)
    public int currentThorn;    // 天罚(反伤，被攻击时对攻击者造成天罚层数的伤害)

    [Header("Debuffs")]
    public int currentVuln;     // 虚损(受到伤害提高, 50%)
    public int currentWeak;     // 力竭(减少攻击力, 25%)
    public int currentPoison;   // 中毒(回合结束减少HP, 减少等同于层数的生命)

    public int roundsNumber;    // 一场战斗进行的回合数
    [Header("Last Turn to Apply Buffs/Debuffs")]
    public int mitiAppliedRound = -1;
    public int sereAppliedRound = -1;
    public int dodgeAppliedRound = -1;
    public int rageAppliedRound = -1;
    public int thornAppliedRound = -1;
    public int vulnAppliedRound = -1;
    public int weakAppliedRound = -1;
    public int poisonAppliedRound = -1;

    // This list store all the buff SOs which stores the number
    public bool isDead;
    protected virtual void Start()
    {
        currentHP = maxHP;
        currentMiti = 0;
        currentShield = 0;
        //TODO: Load all buff SO using addressable asset (virtual, player and enemy have their own SOs)
    }

    // 所有伤害向上取整，所有受伤向下取整
    public virtual void TakeDamage(int amount, CharacterBase attacker, bool isAbsolute = false)
    {
        // 如果时绝对伤害（中毒），则不受buff增减影响
        if (!isAbsolute)
        {
            // 闪避
            int dodge = currentDodge * 6;
            if (Random.Range(0, 100) < dodge)
            {
                // TODO: 闪避成功
                return;
            }

            // 计算伤害(考虑减伤25%和易伤50%力竭25%)
            float damageRate = 1.0f;
            if (currentMiti > 0)
            {
                damageRate -= 0.25f;
                // 受伤后减伤减少一层
                UpdateBuffNumber(ref currentMiti, ref mitiAppliedRound, -1);
            }
            if (currentVuln > 0)
            {
                damageRate += 0.5f; // 易伤
            }
            if (attacker.currentWeak > 0)
            {
                damageRate -= 0.25f; // 力竭
            }
            amount = Mathf.FloorToInt(amount * damageRate);
            if (currentRage > 0)
            {
                amount += currentRage;// 暴怒
            }
        }

        // 天罚反伤
        if (attacker != this && currentThorn > 0)
        {
            attacker.TakeDamage(currentThorn, this);
        }

        // 施加伤害
        if (currentHP > amount)
        {
            currentHP -= amount;
        }
        else
        {
            currentHP = 0;
            //TODO: Die
            isDead = true;
        }

        Damage damage = new(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), amount);
        takeDamageEvent.RaiseEvent(damage, amount); // 呼叫ui更新
    }

    public virtual void UpdateShield(int value)
    {
        value += currentSere;   // 宁静增加获得的化劲
        Debug.Log($"Before Update: CurrentShield = {currentShield}");
        currentShield += value;
        if (currentShield < 0) currentShield = 0;
        Debug.Log($"After Update: CurrentShield = {currentShield}");
    }

    public virtual void Heal(int healAmount) { }

    public virtual void Die() { }

    // 监听TurnBeginEvent
    public virtual void OnTurnBegin()
    {
        roundsNumber++;
        // 重置化劲
        currentShield = 0;
        // 如果上一回合新被施加buff，则buffs减1
        if (mitiAppliedRound != roundsNumber - 1)
            UpdateBuffNumber(ref currentMiti, ref mitiAppliedRound, -1);
        if (sereAppliedRound != roundsNumber - 1)
            UpdateBuffNumber(ref currentSere, ref sereAppliedRound, -1);
        if (dodgeAppliedRound != roundsNumber - 1)
            UpdateBuffNumber(ref currentDodge, ref dodgeAppliedRound, -1);
        if (rageAppliedRound != roundsNumber - 1)
            UpdateBuffNumber(ref currentRage, ref rageAppliedRound, -1);
        if (thornAppliedRound != roundsNumber - 1)
            UpdateBuffNumber(ref currentThorn, ref thornAppliedRound, -1);
        if (vulnAppliedRound != roundsNumber - 1)
            UpdateBuffNumber(ref currentVuln, ref vulnAppliedRound, -1);
        if (weakAppliedRound != roundsNumber - 1)
            UpdateBuffNumber(ref currentWeak, ref weakAppliedRound, -1);
        if (poisonAppliedRound != roundsNumber - 1)
            UpdateBuffNumber(ref currentPoison, ref poisonAppliedRound, -1);
    }

    // 监听TurnEndEvent
    public virtual void OnTurnEnd()
    {
        // 中毒伤害
        if (currentPoison > 0)
        {
            this.TakeDamage(currentPoison, this, true);
        }
    }

    // TODO: 监听新的战斗开始
    public virtual void OnNewBattle()
    {
        roundsNumber = 0;
    }

    public virtual void UpdateBuffNumber(ref int currentBuff, ref int appliedRound, int value)
    {
        if (currentBuff == 0 && value > 0) appliedRound = roundsNumber;
        currentBuff = Mathf.Clamp(currentBuff + value, 0, 9);
    }
}
