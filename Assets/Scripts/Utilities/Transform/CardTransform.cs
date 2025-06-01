using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CardTransform
{
    public Vector3 position;
    public Quaternion rotation;
    public CardTransform(Vector3 vector3, Quaternion quaternion)
    {
        position = vector3;
        rotation = quaternion;
    }
}
