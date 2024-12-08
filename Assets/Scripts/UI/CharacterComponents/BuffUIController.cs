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
    private readonly List<GameObject> buffsInDisplay = new();
    private readonly Dictionary<BuffType, GameObject> buffUIs = new();

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
        buffUIs.Add(BuffType.Miti, mitiUI);
        buffUIs.Add(BuffType.Sere, sereUI);
        buffUIs.Add(BuffType.Dodge, dodgeUI);
        buffUIs.Add(BuffType.Rage, rageUI);
        buffUIs.Add(BuffType.Thorn, thornUI);
        buffUIs.Add(BuffType.Vuln, vulnUI);
        buffUIs.Add(BuffType.Weak, weakUI);
        buffUIs.Add(BuffType.Poison, poisonUI);
    }

    #region Event Listening

    public void UpdateBuffUI(object obj)
    {
        BuffChange buffChange = (BuffChange)obj;
        if (buffChange.target != currentCharacter) return;

        int updatedValue = buffChange.updated;
        GameObject target = buffUIs[buffChange.buffType];

        if (updatedValue != 0)
        {
            if (!target.activeSelf)
            {
                target.SetActive(true);
                buffsInDisplay.Add(target);
            }
        }
        else
        {
            if (target.activeSelf)
            {
                target.SetActive(false);
                buffsInDisplay.Remove(target);
            }
        }

        target.GetComponentInChildren<TextMeshPro>().text = updatedValue.ToString();
        UpdateList();
    }

    #endregion

    private void UpdateList()
    {
        var count = buffsInDisplay.Count;
        for (var i = 0; i < count; i++)
        {
            buffsInDisplay[i].transform.position = new Vector3(
                leftToRight ? beginPos.x + i * uiInterval : beginPos.x - i * uiInterval, beginPos.y, beginPos.z);
        }
    }
}