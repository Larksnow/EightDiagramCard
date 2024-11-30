using TMPro;
using UnityEngine;

public class ShieldBarController : MonoBehaviour
{
    public CharacterBase currentCharacter;
    public SpriteRenderer bar;
    public TextMeshPro amountText;


    private void Awake()
    {
        amountText.enabled = false;
        bar.enabled = false;
        currentCharacter = GetComponentInParent<CharacterBase>();
    }

    #region Event Listening
    public void UpdateShield(object obj)
    {
        CharacterBase.ShieldChange shieldChange = (CharacterBase.ShieldChange)obj;
        if (shieldChange.target != currentCharacter) return;

        int currentShield = shieldChange.updated;
        if (currentCharacter.isDead || currentShield == 0)
        {
            // TODO: 护盾淡化/碎裂动画
            bar.enabled = false;
            amountText.enabled = false;
        }
        else
        {
            bar.enabled = true;
            amountText.enabled = true;
            amountText.text = currentShield.ToString();
        }
    }
    #endregion
}
