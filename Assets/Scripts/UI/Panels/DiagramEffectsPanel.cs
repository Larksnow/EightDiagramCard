using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DiagramEffectsPanel : MonoBehaviour
{
    public Transform effectsContent;
    public GameObject effectEntryPrefab;
    public DiagramDataSO diagramData;
    public int lineCharacterLimit = 14;

    private void OnEnable()
    {
        UpdateEffects();
    }

    public void UpdateEffects()
    {
        for (int i = 1; i < effectsContent.childCount - 1; i++)
        {
            Destroy(effectsContent.GetChild(i).gameObject);
        }

        foreach (var (effect, i) in diagramData.effects.Select((effect, i) => (effect, i)))
        {
            string descriptionText = effect.GetFormattedDescription();
            GameObject effectEntry = Instantiate(effectEntryPrefab, effectsContent);
            var currentText = effectEntry.GetComponentInChildren<TextMeshPro>();
            effectEntry.transform.SetSiblingIndex(effectsContent.childCount - 2);
            descriptionText = $"【{i + 1}】" + descriptionText;
            currentText.text = descriptionText;

            // 如果单行放不下，再额外创建新的prefab来容纳剩余文本
            if (descriptionText.Length > lineCharacterLimit)
                SplitTextIntoLines(descriptionText, currentText);
        }
    }

    private void SplitTextIntoLines(string description, TextMeshPro currentText)
    {
        int startIndex = 0;

        while (startIndex < description.Length)
        {
            // Determine the length of the current line based on the limit
            int lengthForLine = Mathf.Min(lineCharacterLimit, description.Length - startIndex);
            currentText.text = description.Substring(startIndex, lengthForLine);

            startIndex += lengthForLine;

            // If there's remaining text, create a new entry for the next line
            if (startIndex < description.Length)
            {
                GameObject newEffectEntry = Instantiate(effectEntryPrefab, effectsContent);
                newEffectEntry.transform.SetSiblingIndex(effectsContent.childCount - 2);
                currentText = newEffectEntry.GetComponentInChildren<TextMeshPro>();
            }
        }
    }
}