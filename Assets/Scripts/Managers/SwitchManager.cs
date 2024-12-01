using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    public static SwitchManager main;
    public bool isDay;
    public GameObject SwitchCounter;
    void Awake()
    {
        if (main) Destroy(gameObject);
        else main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
