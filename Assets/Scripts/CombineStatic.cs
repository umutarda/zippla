using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineStatic : MonoBehaviour
{
    void Awake() 
    {
        StaticBatchingUtility.Combine(gameObject);
    }
    
}
