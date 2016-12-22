using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LevelScript : MonoBehaviour {

    //GameController Script
    private GameController gameController;

    //Game Setup
    public GameObject border;
    public GameObject[] asteroids;
    public GameObject aiOpponent;
    public GameObject playerInstance;
    private GameObject player;

    public int asteroidCount;
    public int aiCount;
    public int minAsteroidSize = 1;
    public int maxAsteroidSize = 10;
    public float minAsteroidVel = 0;
    public float maxAsteroidVel = 10;

    //Organizational Components
    GameObject asteroidEmpty;

    //To help instantiation of objects work better
    Grid grid;

    //Game UI
    public Text gameInfo;
    public Text gameTimer;

    private GameObject gameOverPanel;
    private Text gameOverText;
    private Button gameExitButton;
    private Button gameRestartButton;

    //Game Settings
    public Vector2 gridWorldSize;

    internal float minWorldPosX;
    internal float minWorldPosY;
    internal float maxWorldPosX;
    internal float maxWorldPosY;
    internal bool setup;
    public bool debug;

    //Game timer
    float timer;

    // Use this for initialization
    void Start () {
        if (!debug)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
        
        RunGameSetup();

    }
	
	// Update is called once per frame
	void Update () {

        if (!setup)
        {
            Timer();
            gameInfo.text = "Time: " + timer.ToString("0.0");
        }

    }

    private void RunGameSetup()
    {
        
        //Setup Game UI
        gameRestartButton = GameObject.FindGameObjectWithTag("restart_button").GetComponent<Button>();
        gameExitButton = GameObject.FindGameObjectWithTag("exit_button").GetComponent<Button>();

        gameInfo = GameObject.FindGameObjectWithTag("info").GetComponent<Text>();
        gameTimer = GameObject.FindGameObjectWithTag("timer").GetComponent<Text>();

        gameOverPanel = GameObject.FindGameObjectWithTag("game_over_panel");
        gameOverText = GameObject.FindGameObjectWithTag("game_over_text").GetComponent<Text>();

        gameRestartButton.onClick.AddListener(() => { RestartGame(); });
        gameExitButton.onClick.AddListener(() => { gameController.ExitGame(); });

        //Hide Game Over Panel
        gameOverPanel.SetActive(false);

        //Create Collision check grid
        grid = new GameObject("Grid").AddComponent<Grid>();
        grid.levelScript = this;

        asteroidEmpty = new GameObject("food");

        minWorldPosX = grid.grid[0, 0].worldPosition.x;
        minWorldPosY = grid.grid[0, 0].worldPosition.y;
        maxWorldPosX = grid.grid[grid.gridSizeX - 1, grid.gridSizeY - 1].worldPosition.x;
        maxWorldPosY = grid.grid[grid.gridSizeX - 1, grid.gridSizeY - 1].worldPosition.y;

        GameObject borderBlock = Instantiate(border, Vector3.zero, Quaternion.identity) as GameObject;
        borderBlock.transform.localScale = new Vector3(gridWorldSize.x, gridWorldSize.y, 1);

        grid.CreateGrid();

        // spawn Food

        for (int i = 0; i < asteroidCount; i++)
        {
            AddAsteroid();
        }

        grid.CreateGrid();

        //Add Player
        int index = UnityEngine.Random.Range(0, grid.walkableNodes.Count);
        Node node = grid.walkableNodes[index];
        player = Instantiate(playerInstance, node.worldPosition, Quaternion.identity) as GameObject;
        //player.GetComponent<SpriteRenderer>().color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        //player.transform.localScale = new Vector3(1.8f, 1.8f);
        //BasePlayer playerScript = player.AddComponent<Player>();
        player.name = "player1";
        player.tag = "Player";

        ////Spawn AI
        //for (int i = 0; i < aiCount; i++) {
        //	AddAI ();
        //}

    }

    //public void AddAI (){
    //	int index = UnityEngine.Random.Range (0, spawnLocations.Count);
    //	Coord location = spawnLocations [index];
    //	GameObject ai = Instantiate (bubble, new Vector3 (location.x, location.y), Quaternion.identity) as GameObject;
    //	ai.GetComponent<SpriteRenderer> ().color = new Color (UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
    //	ai.transform.localScale = new Vector3(1.8f,1.8f);
    //	ai.name = "AI";
    //	ai.tag = "ai";
    //	//ai.AddComponent<Rigidbody2D> ();
    //	AI aiScript = ai.AddComponent<AI>();
    //	//ai.transform.SetParent (foodEmpty.transform);
    //}
    private void AddAsteroid()
    {
        int index = UnityEngine.Random.Range(0, grid.walkableNodes.Count);
        int asteroidSize = UnityEngine.Random.Range(minAsteroidSize, maxAsteroidSize);
        float asteroidVel = UnityEngine.Random.Range(minAsteroidVel, maxAsteroidVel);
        Vector2 velDirection = Random.insideUnitCircle.normalized;

        GameObject asteroid = asteroids[UnityEngine.Random.Range(0, asteroids.Length)];
        Node node = grid.walkableNodes[index];
        GameObject asteroidObj = Instantiate(asteroid, node.worldPosition, Quaternion.identity) as GameObject;
        asteroidObj.transform.localScale = new Vector3(asteroidSize, asteroidSize);
        asteroidObj.GetComponent<Rigidbody2D>().velocity = velDirection * asteroidVel;
        asteroidObj.transform.SetParent(asteroidEmpty.transform);
    }

    internal void GameOver()
    {
        setup = true;
        gameOverPanel.SetActive(true);
        //if (player.GetComponent<Player>().food > highestFoodCount && timer < lowestHighScoreTime)
        //{
        //    // New High Score
        //    gameOverText.text = "You made it on the Leaderboard!!\n" + timer.ToString() + " s";
        //    for (int i = 0; i < highScores.Count; i++)
        //    {
        //        if (timer < highScores[i].time)
        //        {
        //            highScores.Insert(i, new Scorer(playerName, Mathf.RoundToInt(timer)));
        //            break;
        //        }
        //    }
        //    highScores.RemoveAt(10);
        //    SaveHighScores();
        //}
        //else if (player.GetComponent<Player>().food > highestFoodCount)
        //{
        //    //Won Game, no new high score
        //    gameOverText.text = "You Won!!\n" + timer.ToString() + " s";
        //}
        //else
        //{
        //    //Lost Game
        //    gameOverText.text = "Game Over";
        //}
    }

    //public void SaveName()
    //{
    //    playerName = menuEnterNameField.text;

    //    BinaryFormatter bf = new BinaryFormatter();

    //    FileStream file = File.Create(savePathPlayerName);

    //    bf.Serialize(file, playerName);
    //    file.Close();
    //}

    //public void SaveHighScores()
    //{
    //    BinaryFormatter bf = new BinaryFormatter();

    //    FileStream file = File.Create(savePathHighScores);

    //    bf.Serialize(file, highScores);
    //    file.Close();


    //}

    private void Timer()
    {
        timer += Time.deltaTime;
        gameTimer.text = "Health: " + player.GetComponent<Player>().health;
    }
    private void RestartGame()
    {
        SceneManager.LoadScene("level");
    }
}
