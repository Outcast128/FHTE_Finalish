using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    // Screen Object Variables
    public GameObject loginUI;
    public GameObject registerUI;
    public GameObject userDataUI;
    public GameObject scoreboardUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    // Functions to change the login screen UI

    public void ClearScreen()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        userDataUI.SetActive(false);
        scoreboardUI.SetActive(false);
    }

    public void LoginScreen() // Back Button
    {
        ClearScreen();
        loginUI.SetActive(true);
    }

    public void RegisterScreen() // Register Button
    {
        ClearScreen();
        registerUI.SetActive(true);
    }

    public void UserDataScreen() // UserData Button
    {
        ClearScreen();
        userDataUI.SetActive(true);
    }

    public void ScoreboardScreen() // Scoreboard Button
    {
        ClearScreen();
        scoreboardUI.SetActive(true);
    }
}
