using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelSelectUI : FadablePanel
{
    
    // Hardcode for now, option1  is card option2 is blessing
    public GameObject option1;
    public GameObject option2;
    public GameAwardSO gameAwardData;
    public GameSceneSO sceneToLoad;

    [Header("Broadcast events")]
    public ObjectEventSO LoadNextLevelEvent;

    public override void FadeOutAfterClick(GameObject selected)
    {
        base.FadeOutAfterClick(selected);
        if (selected == option1)
        {
            gameAwardData.awardType = AwardType.Card;
            LoadNextLevelEvent.RaiseEvent(null, this);
        }else if (selected == option2)
        {
            //TODO: Change this into blessing after we made blessing
            gameAwardData.awardType = AwardType.Card;
            LoadNextLevelEvent.RaiseEvent(null, this);
        }
    }
}
