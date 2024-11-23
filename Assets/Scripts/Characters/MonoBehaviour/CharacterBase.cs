using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int maxHp;
    public IntVariable hp;
    public int CurrentHP {get => hp.currentValue; set => hp.SetValue(value);}
    public int MaxHP {get => hp.maxValue;}
    
    private bool isDead;
    protected virtual void Start(){
        hp.maxValue = maxHp;
        CurrentHP = MaxHP;
    }

    public virtual void TakeDamage(int damage)
    {
        if (CurrentHP > damage)
        {
            CurrentHP -= damage;
        }
        if (CurrentHP <= 0)
        {
            CurrentHP =  0;
            //TODO: Die
            isDead = true;
        }
    } 
}
