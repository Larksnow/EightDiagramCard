using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnemyBase : CharacterBase
{
    public EnemyDataSO enemyData;
    [Header("Buffs")]
    public bool isTaunting;    // 嘲讽(使敌方只能攻击此单位)
    CharacterBase player;
    Indicator indicator;
    [SerializeField] ObjectEventSO enemyEndActionEvent; // 敌人行动结束事件
    [SerializeField] Sprite daySprite;
    [SerializeField] Sprite nightSprite;
    public SpriteRenderer dayImage;
    public SpriteRenderer nightImage;
    public float fadeDuration = 0.5f;
    protected override void Start()
    {
        base.Start();
        indicator = FindObjectOfType<Indicator>();
        player = GameObject.FindGameObjectWithTag("player").GetComponent<Player>();
        dayImage.sprite = daySprite;
        nightImage.sprite = nightSprite;
        if (indicator.isDay)
        {
            SetSpriteRendererAlpha(dayImage, 1f);
            SetSpriteRendererAlpha(nightImage, 0f);
        }
        else
        {
            SetSpriteRendererAlpha(dayImage, 0f);
            SetSpriteRendererAlpha(nightImage, 1f);
        }
    }
    private void SetSpriteRendererAlpha(SpriteRenderer spriteRenderer, float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
    public override void OnTurnBegin()
    {
        base.OnTurnBegin();
        // TODO: 嘲讽只生效一回合
        if (isTaunting) isTaunting = false;
    }

    public virtual void TakeActions()
    {
        if (indicator.isDay) // If is day, execute Day intends
        {
            int index = (roundsNumber - 1) % enemyData.dayIntends.Count;
            foreach (var item in enemyData.dayIntends[index].actionList)
            {
                Debug.Log($"Enmey {enemyData.name} Action: " + item.name);
                item.Execute(this, player);
            }
        }
        else // If is night, execute night intends
        {
            int index = (roundsNumber - 1) % enemyData.nightIntends.Count;
            foreach (var item in enemyData.nightIntends[index].actionList)
            {
                Debug.Log($"Enmey {enemyData.name} Action: " + item.name);
                item.Execute(this, player);
            }
        }
        enemyEndActionEvent.RaiseEvent(null, this);
    }

    [ContextMenu("Die")]
    public override void Die()
    {
        // Fade out all the renderers of the dead enemy first
        SpriteRenderer[] deadEnemyRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var renderer in deadEnemyRenderers)
        {
            renderer.DOFade(0f, fadeDuration).OnComplete(() => {
                base.Die();
            });
        }
    }
    
    public void SwitchSprite()
    {
        if (!indicator.isDay)
        {
            dayImage.DOFade(0, fadeDuration).SetEase(Ease.InOutQuart);
            nightImage.DOFade(1, fadeDuration).SetEase(Ease.InOutQuart);

        }
        else if (indicator.isDay)
        {
            dayImage.DOFade(1, fadeDuration).SetEase(Ease.InOutQuart);
            nightImage.DOFade(0, fadeDuration).SetEase(Ease.InOutQuart);
        }
    }
    // 敌人被动效果写在自己的Class里
    public List<EnemyEffect> GetNextTurnEffects()
    {
        return indicator.isDay ? enemyData.dayIntends[roundsNumber % enemyData.dayIntends.Count].actionList
        : enemyData.nightIntends[roundsNumber % enemyData.nightIntends.Count].actionList;
    }
}
