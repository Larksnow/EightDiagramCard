using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : EnemyBase
{
    public GameObject grassPrefab;
    [SerializeField] GameObject enemyArrangement;
    // 敌人被动效果写在自己的Class里
    protected override void Awake()
    {
        base.Awake();
        enemyArrangement = GameObject.Find("EnemyArrangement");
    }
    
    public void DuplicateSelf()
    {
        GameObject copygrass = Instantiate(grassPrefab, transform.position, Quaternion.identity, enemyArrangement.transform);
        copygrass.GetComponent<Grass>().maxHP = Mathf.CeilToInt(currentHP / 2f);
        copygrass.GetComponent<Grass>().AddHP(0);
        enemyArrangement.GetComponent<EnemyArrangement>().enemyList.Add(copygrass);
        enemyArrangement.GetComponent<EnemyArrangement>().UpdateEnemyPosition();
    }
}
