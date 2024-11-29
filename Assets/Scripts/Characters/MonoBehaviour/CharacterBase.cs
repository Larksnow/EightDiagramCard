using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class CharacterBase : MonoBehaviour
{
    public struct DamagePosition
    {
        public Vector3 position;
        public int amount;
        public DamagePosition(Vector3 pos, int amount)
        {
            this.position = pos;
            this.amount = amount;
        }
    }

    public struct HPChange
    {
        public CharacterBase target;
        public int updated;
        public HPChange(CharacterBase target, int amount) { this.target = target; this.updated = amount; }
    }

    public struct ShieldChange
    {
        public CharacterBase target;
        public int updated;
        public ShieldChange(CharacterBase target, int amount) { this.target = target; this.updated = amount; }
    }

    public struct BuffChange
    {
        public CharacterBase target;
        public BuffType buffType;
        public int updated;
        public BuffChange(CharacterBase target, BuffType buffType, int amount) { this.target = target; this.buffType = buffType; this.updated = amount; }
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

    [Header("Broadcast Events")]
    public ObjectEventSO updateHPEvent;
    public ObjectEventSO updateShieldedEvent;
    public ObjectEventSO updateBuffEvent;

    // This list store all the buff SOs which stores the number
    public bool isDead;

    protected virtual void Start()
    {
        currentHP = maxHP;
        currentMiti = 0;
        currentShield = 0;
        currentSere = 0;
        currentDodge = 0;
        currentRage = 0;
        currentThorn = 0;
        currentVuln = 0;
        currentWeak = 0;
        currentPoison = 0;
        roundsNumber = 1;
        isDead = false;
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
                UpdateBuffNumber(BuffType.Miti, -1);
                Debug.Log("Decrease Miti by 1 by taking damage");
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
        UpdateHP(-amount);

        DamagePosition damage = new(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), amount);
        takeDamageEvent.RaiseEvent(damage, amount); // 呼叫ui更新(伤害数字)
    }

    public virtual void UpdateHP(int amount)
    {
        currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
        if (currentHP == 0)
            isDead = true;
        updateHPEvent.RaiseEvent(new HPChange(this, currentHP), this);

    }

    public virtual void UpdateShield(int value)
    {
        value += currentSere;   // 宁静增加获得的化劲
        Debug.Log($"Before Update: CurrentShield = {currentShield}");
        currentShield += value;
        if (currentShield < 0) currentShield = 0;
        Debug.Log($"After Update: CurrentShield = {currentShield}");
        updateShieldedEvent.RaiseEvent(new ShieldChange(this, currentShield), this);
    }

    public virtual void Heal(int healAmount) { }

    public virtual void Die() { }

    // 监听TurnBeginEvent
    public virtual void OnTurnBegin()
    {
        roundsNumber++;
        // 重置化劲
        currentShield = 0;
        Debug.Log("Decrease Miti by 1");
        UpdateBuffNumber(BuffType.Miti, -1);
        UpdateBuffNumber(BuffType.Sere, -1);
        UpdateBuffNumber(BuffType.Dodge, -1);
        UpdateBuffNumber(BuffType.Rage, -1);
        UpdateBuffNumber(BuffType.Thorn, -1);

        // 如果不是上一回合新被敌人施加debuff，则buffs减1
        if (vulnAppliedRound != roundsNumber - 1)
        {
            Debug.Log("Decrease Vuln by 1");
            UpdateBuffNumber(BuffType.Vuln, -1);
        }
        if (weakAppliedRound != roundsNumber - 1)
        {
            Debug.Log("Decrease Weak by 1");
            UpdateBuffNumber(BuffType.Weak, -1);
        }
        if (poisonAppliedRound != roundsNumber - 1)
        {
            Debug.Log("Decrease Poison by 1");
            UpdateBuffNumber(BuffType.Poison, -1);
        }
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

    public virtual void UpdateBuffNumber(BuffType buffType, int value)
    {
        switch (buffType)
        {
            case BuffType.Miti:
                if (currentMiti == 0 && value > 0) mitiAppliedRound = roundsNumber;
                currentMiti = Mathf.Clamp(currentMiti + value, 0, 9);
                updateBuffEvent.RaiseEvent(new BuffChange(this, BuffType.Miti, currentMiti), this);
                break;
            case BuffType.Sere:
                if (currentSere == 0 && value > 0) sereAppliedRound = roundsNumber;
                currentSere = Mathf.Clamp(currentSere + value, 0, 9);
                updateBuffEvent.RaiseEvent(new BuffChange(this, BuffType.Sere, currentSere), this);
                break;
            case BuffType.Dodge:
                if (currentDodge == 0 && value > 0) dodgeAppliedRound = roundsNumber;
                currentDodge = Mathf.Clamp(currentDodge + value, 0, 9);
                updateBuffEvent.RaiseEvent(new BuffChange(this, BuffType.Dodge, currentDodge), this);
                break;
            case BuffType.Rage:
                if (currentRage == 0 && value > 0) rageAppliedRound = roundsNumber;
                currentRage = Mathf.Clamp(currentRage + value, 0, 9);
                updateBuffEvent.RaiseEvent(new BuffChange(this, BuffType.Rage, currentRage), this);
                break;
            case BuffType.Thorn:
                if (currentThorn == 0 && value > 0) thornAppliedRound = roundsNumber;
                currentThorn = Mathf.Clamp(currentThorn + value, 0, 9);
                updateBuffEvent.RaiseEvent(new BuffChange(this, BuffType.Thorn, currentThorn), this);
                break;
            case BuffType.Vuln:
                if (currentVuln == 0 && value > 0) vulnAppliedRound = roundsNumber;
                currentVuln = Mathf.Clamp(currentVuln + value, 0, 9);
                updateBuffEvent.RaiseEvent(new BuffChange(this, BuffType.Vuln, currentVuln), this);
                break;
            case BuffType.Weak:
                if (currentWeak == 0 && value > 0) weakAppliedRound = roundsNumber;
                currentWeak = Mathf.Clamp(currentWeak + value, 0, 9);
                updateBuffEvent.RaiseEvent(new BuffChange(this, BuffType.Weak, currentWeak), this);
                break;
            case BuffType.Poison:
                if (currentPoison == 0 && value > 0) poisonAppliedRound = roundsNumber;
                currentPoison = Mathf.Clamp(currentPoison + value, 0, 9);
                updateBuffEvent.RaiseEvent(new BuffChange(this, BuffType.Poison, currentPoison), this);
                break;
        }
    }
}
