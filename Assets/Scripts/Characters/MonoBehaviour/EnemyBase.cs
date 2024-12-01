using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : CharacterBase
{
    public EnemyDataSO enemyData;
    [Header("Buffs")]
    public bool isTaunting;    // 嘲讽(使敌方只能攻击此单位)
    CharacterBase player;
    SwitchManager switchManager;
    [SerializeField] ObjectEventSO enemyTrunEndEvent; // 敌人回合结束事件
    [SerializeField] Sprite daySprite;
    [SerializeField] Sprite nightSprite;
    public Transform image;
    protected virtual void Awake()
    {
        switchManager = FindObjectOfType<SwitchManager>();
        player = GameObject.FindGameObjectWithTag("player").GetComponent<Player>();
        image.GetComponent<SpriteRenderer>().sprite = switchManager.isDay ? daySprite : nightSprite;
    }

    public override void OnTurnBegin()
    {
        base.OnTurnBegin();
        // TODO: 嘲讽只生效一回合
        if (isTaunting) isTaunting = false;
    }

    public virtual void TakeActions()
    {
        if(SwitchManager.main.isDay) // If is day, execute Day intends
        {
            int index = (roundsNumber - 1) % enemyData.dayIntends.Count;
            foreach (var item in enemyData.dayIntends[index].actionList)
            {
                Debug.Log($"Enmey {enemyData.name} Action: " + item.name);
                item.Execute(this, player);
            }
        }else // If is night, execute night intends
        {
            int index = (roundsNumber - 1) % enemyData.nightIntends.Count;
            foreach (var item in enemyData.nightIntends[index - 1].actionList)
            {
                Debug.Log($"Enmey {enemyData.name} Action: " + item.name);
                item.Execute(this, player);
            }
        }
        enemyTrunEndEvent.RaiseEvent(null, this);
    }

    // 敌人被动效果写在自己的Class里
}
