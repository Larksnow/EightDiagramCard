using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class DamageNumberController : MonoBehaviour
{
    public DamageNumberPool damageNumberPool;
    public float numbersDelay = 0.3f;
    private Dictionary<Vector3, int> damagePositions = new();

public void ShowDamageNumber(object obj)
{
    DamagePosition damagePos = (DamagePosition)obj;

    if (!damagePositions.TryGetValue(damagePos.position, out int count))
    {
        count = 0;
        damagePositions[damagePos.position] = count;
    }

    StartCoroutine(DelayedShowDamageNumber(damagePos, count));
    damagePositions[damagePos.position] += 1;
}

    private IEnumerator DelayedShowDamageNumber(DamagePosition damagePos, int index)
    {
        yield return new WaitForSeconds(index * numbersDelay);
        
        float diminishDuration = 0.5f;
        float descentDuration = 1f;
        int descentHeight = 5;
        GameObject damageNumber = damageNumberPool.GetObjectFromPool();
        TextMeshPro text = damageNumber.GetComponent<TextMeshPro>();

        damageNumber.transform.position = damagePos.position;
        text.text = damagePos.amount.ToString();
        damageNumber.GetComponent<SortingGroup>().sortingOrder = index;

        // 伤害数字动画
        Sequence sequence = DOTween.Sequence();
        sequence.Append(damageNumber.transform.DOScale(damageNumber.transform.localScale * 0.4f, diminishDuration))
            .Join(text.DOColor(Color.white, diminishDuration))
            .Join(damageNumber.transform.DOMove(damageNumber.transform.position + new Vector3(1, -0.5f, 0),
                diminishDuration))
            .Append(damageNumber.transform.DOMove(
                new Vector3(damageNumber.transform.position.x + 1, damageNumber.transform.position.y - descentHeight,
                    damageNumber.transform.position.z), descentDuration))
            .Join(text.DOFade(0f, descentDuration));
        yield return sequence.WaitForCompletion();

        damagePositions.Remove(damagePos.position);
        damageNumberPool.ReleaseObjectToPool(damageNumber);
    }
}