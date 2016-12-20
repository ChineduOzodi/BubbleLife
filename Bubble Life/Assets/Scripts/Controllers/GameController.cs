using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour
{
    #region "Declarations"
    //Setup
    public static GameController instance = null;
    public bool menu = true;



    #endregion
    #region "Monodevelop functions"
    // Use this for initialization
    void Awake()
	{
        //Initial Setup and making sure only one gamemanager exists
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
        
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            ExitGame();
        }
    }
    void OnLevelWasLoaded(int levelInt)
    {
        if(levelInt == 0) //Main Menu
        {
            menu = true;
            
        }
        else //Game Level
        {
            menu = false;
        }
    }

    #endregion

    internal void ExitGame()
    {
        if (menu)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene("main_menu");
        }
    }
    
}
