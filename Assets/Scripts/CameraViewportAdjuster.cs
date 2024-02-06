using UnityEngine;

// Change the width of the viewport each time space key is pressed

public class CameraViewportAdjuster : MonoBehaviour
{
    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        ResizeCamera();
    }

    public void ResizeCamera() 
    {
        
        float defaultAR = 16f/9;
        float aspectRatio = (1.0f*Screen.width)/Screen.height;

        if(aspectRatio < 1) Debug.Log("aspect ratio < 1");
        
        if(aspectRatio > defaultAR) 
        {
            float margin = (1-defaultAR/aspectRatio)/2;
            cam.rect = new Rect(margin, 0.0f, 1.0f - margin * 2.0f, 1.0f);

        }

        else if (aspectRatio < defaultAR) 
        {
            float margin = (1-aspectRatio/defaultAR)/2;
            cam.rect = new Rect(0.0f,margin,1.0f, 1.0f - margin * 2.0f);
        }

    }
}