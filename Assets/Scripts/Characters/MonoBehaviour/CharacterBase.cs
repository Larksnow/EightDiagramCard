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

    public int maxHp;
    public IntVariable hp;
    public IntVariable mitiNumber;
    // Property to manage the character's current health and trigger events when modified
    // These are stored in a SO called CharacterHP (IntVariable type)
    public int CurrentHP { get => hp.currentValue; set => hp.SetValue(value); }
    public int MaxHP { get => hp.maxValue; }
    public int CurrentMiti { get => mitiNumber.currentValue; set => mitiNumber.SetValue(value); }

    // This list store all the buff SOs which stores the number
    public List<IntVariable> buffList;
    public bool isDead;
    protected virtual void Start()
    {
        hp.maxValue = maxHp;
        CurrentHP = MaxHP;
        //TODO: Load all buff SO using addressable asset (virtual, player and enemy have their own SOs)
    }

    // 所有伤害向上取整，所有受伤向下取整
    public virtual void TakeDamage(int amount)
    {
        Damage damage = new(new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), amount);
        // TODO:: Apply miti before taking damage -> （damage * 0.75）向下取整 
        if (CurrentMiti > 1)
        {
            amount = Mathf.FloorToInt(0.75f * amount);
            UpdateMitiNumber(-1);
        }
        if (CurrentHP > amount)
        {
            CurrentHP -= amount;
        }
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            //TODO: Die
            isDead = true;
        }
    }

    public virtual void Die() { }

    public virtual void Heal(int healAmount) { }

    public virtual void UpdateMitiNumber(int value) // decrease one miti
    {
        Debug.Log($"Before Update: CurrentMiti = {CurrentMiti}");
        CurrentMiti += value;
        Debug.Log($"After Update: CurrentMiti = {CurrentMiti}");
        if (CurrentMiti < 0)
        {
            CurrentMiti = 0;
        }
    }
}
