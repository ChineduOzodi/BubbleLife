using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    //GameController Script
    private GameController gameController;

    //Main Menu UI Setup
    private Button menuStartButton;
    private Button menuExitButton;
    private Text highScoreText;
    private InputField menuEnterNameField;

    //Save
    List<Scorer> highScores;
    private float lowestHighScoreTime;
    string playerName;
    string savePathPlayerName;
    string savePathHighScores;

    // Use this for initialization
    void Start () {

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        RunMainMenuSetup();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void RunMainMenuSetup()
    {
        //Button Setup
        menuStartButton = GameObject.FindGameObjectWithTag("start_button").GetComponent<Button>();
        menuExitButton = GameObject.FindGameObjectWithTag("exit_button").GetComponent<Button>();

        menuStartButton.onClick.AddListener(() => { StartGame(); });
        menuExitButton.onClick.AddListener(() => { gameController.ExitGame(); });

    }
    

    private void StartGame()
    {
        SceneManager.LoadScene("level_1");
    }
}
