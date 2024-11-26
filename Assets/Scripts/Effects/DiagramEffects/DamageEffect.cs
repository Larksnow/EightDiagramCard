using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Diagram Effects/Damage Effect")]

public class DamageEffect : Effect
{
    public override void Execute(CharacterBase target)
    {
        switch (targetType){
            case EffectTargetType.Self:
            case EffectTargetType.Single:
                target.TakeDamage(value);
                break;
            case EffectTargetType.All:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("enemy"))
                {
                    enemy.GetComponent<CharacterBase>().TakeDamage(value);
                }
                break;
        }
    }
}