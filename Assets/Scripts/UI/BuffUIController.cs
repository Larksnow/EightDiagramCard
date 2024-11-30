using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffUIController : MonoBehaviour
{
    public CharacterBase currentCharacter;
    public int uiInterval = 1;
    public bool leftToRight = true;

    private Vector3 beginPos;
    private GameObject mitiUI, sereUI, dodgeUI, rageUI, thornUI, vulnUI, weakUI, poisonUI;
    // TODO: 敌人和玩家独特buffUI
    private List<GameObject> buffUIs = new();

    private void Awake()
    {
        beginPos = transform.position;
        mitiUI = transform.Find("Miti").gameObject;
        sereUI = transform.Find("Sere").gameObject;
        dodgeUI = transform.Find("Dodge").gameObject;
        rageUI = transform.Find("Rage").gameObject;
        thornUI = transform.Find("Thorn").gameObject;
        vulnUI = transform.Find("Vuln").gameObject;
        weakUI = transform.Find("Weak").gameObject;
        poisonUI = transform.Find("Poison").gameObject;
    }

    #region Event Listening
    public void UpdateBuffUI(object obj)
    {
        CharacterBase.BuffChange buffChange = (CharacterBase.BuffChange)obj;
        if (buffChange.target != currentCharacter) return;

        int updatedValue = buffChange.updated;
        GameObject target;
        switch (buffChange.buffType)
        {
            case BuffType.Miti:
                target = mitiUI;
                break;
            case BuffType.Sere:
                target = sereUI;
                break;
            case BuffType.Dodge:
                target = dodgeUI;
                break;
            case BuffType.Rage:
                target = rageUI;
                break;
            case BuffType.Thorn:
                target = thornUI;
                break;
            case BuffType.Vuln:
                target = vulnUI;
                break;
            case BuffType.Weak:
                target = weakUI;
                break;
            case BuffType.Poison:
                target = poisonUI;
                break;
            default:
                target = null;
                Debug.LogError("BuffType not found");
                break;
        }

        if (updatedValue != 0)
        {
            if (!target.activeSelf)
            {
                target.SetActive(true);
                buffUIs.Add(target);
            }
        }
        else
        {
            if (target.activeSelf)
            {
                target.SetActive(false);
                buffUIs.Remove(target);
            }
        }
        target.GetComponentInChildren<TextMeshPro>().text = updatedValue.ToString();
        UpdateList();
    }
    #endregion

    private void UpdateList()
    {
        int count = buffUIs.Count;
        for (int i = 0; i < count; i++)
        {
            buffUIs[i].transform.position = new Vector3(leftToRight ? beginPos.x + i * uiInterval : beginPos.x - i * uiInterval, beginPos.y, beginPos.z);
        }
    }
}