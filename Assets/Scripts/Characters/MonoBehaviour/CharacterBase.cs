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

    public int maxHp;
    public IntVariable hp;
    // Property to manage the character's current health and trigger events when modified
    // These are stored in a SO called CharacterHP (IntVariable type)
    public int CurrentHP { get => hp.currentValue; set => hp.SetValue(value); }
    public int MaxHP { get => hp.maxValue; }

    [Header("Broadcast Events")]
    public ObjectEventSO takeDamageEvent;

    public bool isDead;
    protected virtual void Start()
    {
        hp.maxValue = maxHp;
        CurrentHP = MaxHP;
    }

    public virtual void TakeDamage(int amount)
    {
        Damage damage = new(new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), amount);
        takeDamageEvent.RaiseEvent(damage, this);

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
}
