using TMPro;
using UnityEngine;

public interface IColliderSetUp
{
    void SetupCollider(GameObject obj)
    {
        var collider = obj.AddComponent<BoxCollider2D>();

        if (obj.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            SetupSpriteCollider(collider, spriteRenderer);
        }
        else if (obj.TryGetComponent<TextMeshPro>(out var textMeshPro))
        {
            collider.size = textMeshPro.bounds.size;
        }
        else
        {
            collider.size = obj.transform.lossyScale;
        }
    }

    void SetupSpriteCollider(BoxCollider2D collider, SpriteRenderer spriteRenderer)
    {
        Rect spriteRect = spriteRenderer.sprite.rect;
        float pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;
        Vector2 actualSize = new(
            spriteRect.width / pixelsPerUnit,
            spriteRect.height / pixelsPerUnit
        );
        collider.size = actualSize;
    }
}