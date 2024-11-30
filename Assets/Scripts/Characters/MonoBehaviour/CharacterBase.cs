using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[System.Serializable]
public class CharacterBase : MonoBehaviour
{
    [Header("Broadcast Events")]
    public ObjectEventSO takeDamageEvent;

    [Header("Attributes")]
    public int maxHP;
    public int currentHP;
    public int currentShield;   // 化劲(抵消伤害)

    public Dictionary<BuffType, int> buffNumbers = new();
    public Dictionary<BuffType, int> newlyAppliedRounds = new();
    public int roundsNumber;    // 一场战斗进行的回合数
    public bool isDead;

    [Header("Broadcast Events")]
    public ObjectEventSO updateHPEvent;
    public ObjectEventSO updateShieldedEvent;
    public ObjectEventSO updateBuffEvent;

    protected virtual void Start()
    {
        currentHP = maxHP;
        currentShield = 0;

        // 初始化buffs
        buffNumbers.Add(BuffType.Miti, 0);
        buffNumbers.Add(BuffType.Sere, 0);
        buffNumbers.Add(BuffType.Dodge, 0);
        buffNumbers.Add(BuffType.Rage, 0);
        buffNumbers.Add(BuffType.Thorn, 0);
        buffNumbers.Add(BuffType.Vuln, 0);
        buffNumbers.Add(BuffType.Weak, 0);
        buffNumbers.Add(BuffType.Poison, 0);

        newlyAppliedRounds.Add(BuffType.Miti, -1);
        newlyAppliedRounds.Add(BuffType.Sere, -1);
        newlyAppliedRounds.Add(BuffType.Dodge, -1);
        newlyAppliedRounds.Add(BuffType.Rage, -1);
        newlyAppliedRounds.Add(BuffType.Thorn, -1);
        newlyAppliedRounds.Add(BuffType.Vuln, -1);
        newlyAppliedRounds.Add(BuffType.Weak, -1);
        newlyAppliedRounds.Add(BuffType.Poison, -1);

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
            int dodge = buffNumbers[BuffType.Dodge] * 6;
            if (Random.Range(0, 100) < dodge)
            {
                // TODO: 闪避成功
                return;
            }

            // 计算伤害(考虑减伤25%和易伤50%力竭25%)
            float damageRate = 1.0f;
            if (buffNumbers[BuffType.Miti] > 0)
            {
                damageRate -= 0.25f;
                // 受伤后减伤减少一层
                AddBuffNumber(BuffType.Miti, -1);
                Debug.Log("Decrease Miti by 1 by taking damage");
            }
            if (buffNumbers[BuffType.Vuln] > 0)
            {
                damageRate += 0.5f; // 易伤
            }
            if (attacker.buffNumbers[BuffType.Weak] > 0)
            {
                damageRate -= 0.25f; // 力竭
            }
            amount = Mathf.FloorToInt(amount * damageRate);
            if (buffNumbers[BuffType.Rage] > 0)
            {
                amount += buffNumbers[BuffType.Rage];// 暴怒
            }
        }

        // 天罚反伤
        if (attacker != this && buffNumbers[BuffType.Thorn] > 0)
        {
            attacker.TakeDamage(buffNumbers[BuffType.Thorn], this);
        }

        // 施加伤害
        AddHP(-amount);

        DamagePosition damage = new(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), amount);
        takeDamageEvent.RaiseEvent(damage, amount); // 呼叫ui更新(伤害数字)
    }

    public virtual void AddHP(int amount)
    {
        currentHP = Mathf.Clamp(currentHP + amount, 0, maxHP);
        if (currentHP == 0)
            isDead = true;
        updateHPEvent.RaiseEvent(new HPChange(this, currentHP), this);

    }

    public virtual void AddShield(int value)
    {
        value += buffNumbers[BuffType.Sere];   // 宁静增加获得的化劲
        Debug.Log($"Before Update: CurrentShield = {currentShield}");
        currentShield += value;
        if (currentShield < 0) currentShield = 0;
        Debug.Log($"After Update: CurrentShield = {currentShield}");
        updateShieldedEvent.RaiseEvent(new ShieldChange(this, currentShield), this);
    }

    public virtual void Heal(int healAmount) { }

    public virtual void Die() { }

    #region Event Listening
    public virtual void OnTurnBegin()
    {
        roundsNumber++;
        // 重置化劲
        currentShield = 0;
        Debug.Log("Decrease Miti by 1");
        AddBuffNumber(BuffType.Miti, -1);
        AddBuffNumber(BuffType.Sere, -1);
        AddBuffNumber(BuffType.Dodge, -1);
        AddBuffNumber(BuffType.Rage, -1);
        AddBuffNumber(BuffType.Thorn, -1);

        // 如果不是上一回合新被敌人施加debuff，则buffs减1
        if (newlyAppliedRounds[BuffType.Vuln] != roundsNumber - 1)
        {
            Debug.Log("Decrease Vuln by 1");
            AddBuffNumber(BuffType.Vuln, -1);
        }
        if (newlyAppliedRounds[BuffType.Weak] != roundsNumber - 1)
        {
            Debug.Log("Decrease Weak by 1");
            AddBuffNumber(BuffType.Weak, -1);
        }
        if (newlyAppliedRounds[BuffType.Poison] != roundsNumber - 1)
        {
            Debug.Log("Decrease Poison by 1");
            AddBuffNumber(BuffType.Poison, -1);
        }
    }

    public virtual void OnTurnEnd()
    {
        // 中毒伤害
        if (buffNumbers[BuffType.Poison] > 0)
        {
            this.TakeDamage(buffNumbers[BuffType.Poison], this, true);
        }
    }

    public virtual void OnNewBattle()
    {
        roundsNumber = 0;
    }
    #endregion

    public virtual void AddBuffNumber(BuffType buffType, int value)
    {
        if (buffNumbers[buffType] == 0 && value > 0) newlyAppliedRounds[buffType] = roundsNumber;
        buffNumbers[buffType] = Mathf.Clamp(buffNumbers[buffType] + value, 0, 9);
        updateBuffEvent.RaiseEvent(new BuffChange(this, buffType, buffNumbers[buffType]), this);
    }
}
