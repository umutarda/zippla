using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject HTPImage;
    public void Play() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    public void ActiveHTPImage() => HTPImage.SetActive(true);
    public void DeactiveHTPImage() => HTPImage.SetActive(false);
    
}
