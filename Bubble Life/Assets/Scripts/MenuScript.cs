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

        //Load saved info
        savePathHighScores = Application.persistentDataPath + "highscores.sav";
        savePathPlayerName = Application.persistentDataPath + "playername.sav";
        Load();
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
        menuEnterNameField = GameObject.FindGameObjectWithTag("name_input_field").GetComponent<InputField>();
        highScoreText = GameObject.FindGameObjectWithTag("info").GetComponent<Text>();

        menuStartButton.onClick.AddListener(() => { StartGame(); });
        menuExitButton.onClick.AddListener(() => { gameController.ExitGame(); });
        //menuEnterNameField.onEndEdit.AddListener(delegate { gameController.SaveName(); });

        //Name Setup
        menuEnterNameField.text = playerName;

        //HighScores Setup
        HighScores();
    }

    public void Load()
    {
        if (File.Exists(savePathHighScores))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePathHighScores, FileMode.Open);
            highScores = (List<Scorer>)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            highScores = new List<Scorer>()
            {
                new Scorer(), new Scorer(), new Scorer(), new Scorer(), new Scorer(),
                new Scorer(), new Scorer(), new Scorer(), new Scorer(), new Scorer()};
        }
        if (File.Exists(savePathPlayerName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePathPlayerName, FileMode.Open);
            playerName = (string)bf.Deserialize(file);
            file.Close();
        }
        else { playerName = "Player Name"; }

        lowestHighScoreTime = highScores[9].time;
    }

    private void HighScores()
    {
        string highScoresString = "";

        for (int i = 0; i < highScores.Count; i++)
        {
            int m = i + 1;
            string playerString = "";
            if (m.ToString().Length == 2)
            {
                playerString = String.Format("{0}. {1} s - {2}\n", m.ToString(), highScores[i].time.ToString(), highScores[i].name);
            }
            else
            {
                playerString = String.Format("{0}.  {1} s - {2}\n", m.ToString(), highScores[i].time.ToString(), highScores[i].name);
            }

            if (playerString.Length < 40)
            {
                string dashes = new string('-', 40 - playerString.Length);
                playerString = playerString.Insert(playerString.IndexOf('-'), dashes);
            }

            highScoresString += playerString;

        }
        highScoreText.text = highScoresString;
    }

    private void StartGame()
    {
        SceneManager.LoadScene("level");
    }
}
