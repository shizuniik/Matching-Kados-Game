using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUI : MonoBehaviour
{
    private void Awake()
    {
    }

    public void StartGame()
    {
        FindObjectOfType<AudioManager>().Play("button_click");
        SceneManager.LoadScene(1); 
    }

    public void QuitGame()
    {
        FindObjectOfType<AudioManager>().Play("button_click");

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); 
#endif
    }

}
