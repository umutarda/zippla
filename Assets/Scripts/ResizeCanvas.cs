using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeCanvas : MonoBehaviour
{
    CameraViewportAdjuster cwa;

    void Awake () => cwa = FindObjectOfType<CameraViewportAdjuster>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnRectTransformDimensionsChange()
    {
        if(cwa) cwa.ResizeCamera();
    }
}
