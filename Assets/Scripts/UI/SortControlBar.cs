using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SortControlBar : MonoBehaviour
{
    public enum SortType
    {
        AcquisitionOrder,
        ManaCost
    }

    public UnityAction<SortType, bool> OnSortChanged; // 通知订阅者（面板）
    public SortType lastSortType = SortType.AcquisitionOrder; // 默认按获取顺序排序

    [Header("Buttons")] public UnityEngine.UI.Button acquisitionOrderBtn;
    public UnityEngine.UI.Button manaCostBtn;

    [Header("Icons")] public Sprite ascendingIcon;
    public Sprite descendingIcon;

    [Header("Highlight Colors")]
    public Color highlightColor;
    public Color normalTextColor;
    public Color normalFlagColor;
    
    private Dictionary<SortType, UnityEngine.UI.Button> sortButtons = new();
    private Dictionary<SortType, Image> orderFlags = new();
    private Dictionary<SortType, bool> isAscendingBools = new();

    private void Awake()
    {
        sortButtons.Add(SortType.AcquisitionOrder, acquisitionOrderBtn);
        sortButtons.Add(SortType.ManaCost, manaCostBtn);
        orderFlags.Add(SortType.AcquisitionOrder,
            acquisitionOrderBtn.transform.Find("OrderFlag").GetComponent<Image>());
        orderFlags.Add(SortType.ManaCost, manaCostBtn.transform.Find("OrderFlag").GetComponent<Image>());
        isAscendingBools.Add(SortType.AcquisitionOrder, true);
        isAscendingBools.Add(SortType.ManaCost, true);
    }

    private void Start()
    {
        acquisitionOrderBtn.onClick.AddListener(() => OnClickSortButton(SortType.AcquisitionOrder));
        manaCostBtn.onClick.AddListener(() => OnClickSortButton(SortType.ManaCost));
        HighlightButton(lastSortType);
    }

    private void OnClickSortButton(SortType sortType)
    {
        if (lastSortType != sortType)
        {
            HighlightButton(sortType);
            CancelHighlightButton(lastSortType);
        }
        else
        {
            isAscendingBools[sortType] = !isAscendingBools[sortType]; // 切换升序/降序
            orderFlags[sortType].sprite = isAscendingBools[sortType] ? ascendingIcon : descendingIcon; // 更新升序降序图标
        }

        OnSortChanged?.Invoke(sortType, isAscendingBools[sortType]); // 通知订阅者（面板）

        lastSortType = sortType;
    }

    private void HighlightButton(SortType sortType)
    {
        sortButtons[sortType].transform.Find("Text").GetComponent<TextMeshProUGUI>().color = highlightColor;
        sortButtons[sortType].transform.Find("OrderFlag").GetComponent<Image>().color = highlightColor;
    }

    private void CancelHighlightButton(SortType sortType)
    {
        sortButtons[sortType].transform.Find("Text").GetComponent<TextMeshProUGUI>().color = normalTextColor;
        sortButtons[sortType].transform.Find("OrderFlag").GetComponent<Image>().color = normalFlagColor;
    }
}