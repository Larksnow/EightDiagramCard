
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageNumberController : MonoBehaviour
{
    public DamageNumberPool damageNumberPool;
    private List<Vector3> damagePositions;

    public void ShowDamageNumber(object obj)
    {
        DamagePosition damagePos = (DamagePosition)obj;

        if (!damagePositions.Contains(damagePos.position))
        {
            StartShowDamageNumber(damagePos);
        }
        else
        {
            // 如果此处有伤害数字播放, 延迟执行
            StartCoroutine(DelayedShowDamageNumber(damagePos));
            return;
        }
    }

    private IEnumerator DelayedShowDamageNumber(DamagePosition damagePos)
    {
        yield return new WaitForSeconds(0.5f);
        StartShowDamageNumber(damagePos);
    }

    private void StartShowDamageNumber(DamagePosition damagePos)
    {
        damagePositions.Add(damagePos.position);

        float diminishDuration = 0.5f;
        float descentDuration = 1f;
        int descentHeight = 5;
        GameObject damageNumber = damageNumberPool.GetObjectFromPool();
        TextMeshPro text = damageNumber.GetComponent<TextMeshPro>();
        Vector3 originPosition = damageNumber.transform.position;
        Vector3 originScale = damageNumber.transform.localScale;

        damageNumber.transform.position = damagePos.position;
        text.text = damagePos.amount.ToString();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(damageNumber.transform.DOScale(damageNumber.transform.localScale * 0.4f, diminishDuration))
            .Join(text.DOColor(Color.white, diminishDuration))
            .Join(damageNumber.transform.DOMove(damageNumber.transform.position + new Vector3(1, -0.5f, 0), diminishDuration))
            .Append(damageNumber.transform.DOMove(new Vector3(damageNumber.transform.position.x + 1, damageNumber.transform.position.y - descentHeight, damageNumber.transform.position.z), descentDuration))
            .Join(text.DOFade(0f, descentDuration))
            .OnComplete(() =>
            {
                damagePositions.Remove(damagePos.position);
                damageNumberPool.ReleaseObjectToPool(damageNumber);
            });
    }
}