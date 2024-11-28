using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Diagram Effects/Damage Effect")]

public class DamageEffect : Effect
{
    public DiagramDataSO zhenData;
    public float zhenMultiplier = 0.7f;
    public float zhenBuffMultiplier = 0.1f;

    public override void Execute(CharacterBase target, DiagramDataSO triggered, CardType cardType = 0)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        int damage = value;
        switch (triggered.diagramType)
        {
            case DiagramType.Li:// 随机选择单体作为目标
                target = enemies[Random.Range(0, enemies.Length)].GetComponent<CharacterBase>();
                target.TakeDamage(damage);
                break;
            case DiagramType.Zhen:
                damage = Mathf.RoundToInt(value * zhenMultiplier);
                foreach (var enemy in enemies)
                {
                    if (zhenData.yangBuff || zhenData.yinBuff)
                    {
                        // 如果震卦有buff，则伤害加10%
                        damage += Mathf.RoundToInt(value * 0.1f);
                    }
                    enemy.GetComponent<CharacterBase>().TakeDamage(damage);
                }
                break;
        }
    }
}