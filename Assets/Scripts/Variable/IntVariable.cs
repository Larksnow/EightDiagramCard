using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "Variable/IntVariable")]
public class IntVariable : ScriptableObject
{
    public int maxValue;
    public int currentValue;
    public IntEventSO valueChangedEvent;
    [SerializeField] private string description;
    public void SetValue(int value)
    {
        currentValue = value;
        // When this int variable changes, it will raise the event tells every subscriber that it has changed
        valueChangedEvent.RaiseEvent(value,this);
    }
}