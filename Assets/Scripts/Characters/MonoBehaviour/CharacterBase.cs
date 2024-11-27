using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public int maxHP;
    public int currentHP;
    public int currentMiti;
    public int currentShield;
    // public int maxHp;
    // public IntVariable hp;
    // public IntVariable mitiNumber;
    // Property to manage the character's current health and trigger events when modified
    // These are stored in a SO called CharacterHP (IntVariable type)
    // public int MaxHP { get => hp.maxValue; }
    // public int CurrentHP { get => hp.currentValue; set => hp.SetValue(value); }
    // public int CurrentMiti { get => mitiNumber.currentValue; set => mitiNumber.SetValue(value); }
    // public int CurrentShield { get; set; }

    // This list store all the buff SOs which stores the number
    public List<IntVariable> buffList;
    public bool isDead;
    protected virtual void Start()
    {
        currentHP = maxHP;
        currentMiti = 0;
        currentShield = 0;
        //TODO: Load all buff SO using addressable asset (virtual, player and enemy have their own SOs)
    }

    // 所有伤害向上取整，所有受伤向下取整
    public virtual void TakeDamage(int amount)
    {
        Damage damage = new(new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), amount);
        if (currentMiti > 1)
        {
            amount = Mathf.FloorToInt(0.75f * amount);
            UpdateMitiNumber(-1);
        }
        if (currentHP > amount)
        {
            currentHP -= amount;
        }
        if (currentHP <= 0)
        {
            currentHP = 0;
            //TODO: Die
            isDead = true;
        }
    }

    public virtual void Die() { }

    public virtual void Heal(int healAmount) { }

    public virtual void UpdateMitiNumber(int value) // decrease one miti
    {
        Debug.Log($"Before Update: CurrentMiti = {currentMiti}");
        currentMiti += value;
        Debug.Log($"After Update: CurrentMiti = {currentMiti}");
        if (currentMiti < 0)
        {
            currentMiti = 0;
        }
    }

    public virtual void UpdateShield(int value)
    {
        Debug.Log($"Before Update: CurrentShield = {currentShield}");
        currentShield += value;
        Debug.Log($"After Update: CurrentShield = {currentShield}");
        if (currentShield < 0) currentShield = 0;
    }
}
