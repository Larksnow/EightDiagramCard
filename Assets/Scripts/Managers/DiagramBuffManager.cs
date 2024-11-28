using System.Collections.Generic;
using UnityEngine;

public class DiagramBuffManager : MonoBehaviour
{
    public DiagramDataSO qianData, duiData, liData, zhenData, xunData, kanData, genData, kunData;

    private List<DiagramDataSO> dataList = new();

    private void Awake()
    {
        dataList.Add(qianData);
        dataList.Add(duiData);
        dataList.Add(liData);
        dataList.Add(zhenData);
        dataList.Add(xunData);
        dataList.Add(kanData);
        dataList.Add(genData);
        dataList.Add(kunData);
        foreach (var data in dataList)
        {
            data.yinBuff = false;
            data.yangBuff = false;
        }
    }
}