using UnityEngine;

public struct DamagePosition
{
    public Vector3 position;
    public int amount;
    public DamagePosition(Vector3 pos, int amount)
    {
        this.position = pos;
        this.amount = amount;
    }
}