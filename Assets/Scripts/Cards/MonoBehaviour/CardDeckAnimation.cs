
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class CardDeckAnimation : MonoBehaviour
{
    // 测试用
    public float aboveDeckPosY = 1f;
    public float animationTime = 0.5f;

    // 卡牌进入/离开牌堆的动画
    public IEnumerator CardTransferAnimation(Transform deckPos, bool moveIn, Card card, int layer = 0)
    {
        layer = (layer + 1) * 10;
        card.isAnimating = true;
        SpriteRenderer[] spriteRenderers = card.GetComponentsInChildren<SpriteRenderer>();
        card.GetComponent<SortingGroup>().sortingOrder = layer;

        if (moveIn)
        {
            card.transform.position = new Vector3(deckPos.position.x, deckPos.position.y + aboveDeckPosY, deckPos.position.z);

            Tween moveTween = card.transform.DOMove(deckPos.position, animationTime);
            Tween scaleTween = card.transform.DOScale(Vector3.one * 0.5f, animationTime).SetEase(Ease.OutQuad);
            Sequence fadeTween = DOTween.Sequence();
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                fadeTween.Join(sr.DOFade(0, animationTime));
            }

            yield return DOTween.Sequence().Append(moveTween).Join(scaleTween).Join(fadeTween).WaitForCompletion();
        }
        else
        {
            card.transform.position = deckPos.position;
            card.transform.localScale = Vector3.one * 0.5f;
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a * 0.5f);
            }

            Tween moveTween = card.transform.DOMove(new Vector3(deckPos.position.x, deckPos.position.y + aboveDeckPosY, deckPos.position.z), animationTime);
            Tween scaleTween = card.transform.DOScale(Vector3.one, animationTime).SetEase(Ease.InOutQuad);
            Sequence fadeTween = DOTween.Sequence();
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                fadeTween.Join(sr.DOFade(1, animationTime));
            }

            yield return DOTween.Sequence().Append(moveTween).Join(scaleTween).Join(fadeTween).WaitForCompletion();
        }

        card.isAnimating = false;
    }
}