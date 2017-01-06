using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using NeuralNetwork;

public class LevelScript : MonoBehaviour {

    protected GameController gameController;

    //Game Setup
    public GameObject border;
    public GameObject starInstance;
    public GameObject[] asteroids;
    public GameObject aiOpponent;
    public GameObject playerInstance;

    protected GameObject player;
    protected AudioSource source;

    public int asteroidCount;
    public int aiCount;
    public int minAsteroidSize = 1;
    public int maxAsteroidSize = 10;
    public float minAsteroidVel = 0;
    public float maxAsteroidVel = 10;

    protected int starCount = 1000;
    protected float m_minStarSize = .15f;
    protected float m_maxStarSize = .3f;

    //Organizational Components
    protected GameObject asteroidEmpty;

    //To help instantiation of objects work better
    protected Grid grid;

    //Game UI
    public Text gameInfo;
    public Text gameTimer;

    protected GameObject gameOverPanel;
    protected Text gameOverText;
    protected Button gameExitButton;
    protected Button gameRestartButton;

    //Game Settings
    public Vector2 gridWorldSize;

    internal float minWorldPosX;
    internal float minWorldPosY;
    internal float maxWorldPosX;
    internal float maxWorldPosY;
    internal bool setup;
    public bool debug;

    //Game timer
    protected float timer;

    //AI
    public int population = 10;

    protected int numInputs = 12 * 4 + 1;
    protected int numOutputs = 4;
    protected int numHiddenLayers = 1;
    protected int numNodeHiddenLayers = 15;

    internal NeuralNet[] nNetwork;
    internal GeneticAlgorithm genAlg;
    internal NeuralNetworkView nView;

    

    // Use this for initialization
    void Start () {
        numNodeHiddenLayers = (numInputs + numOutputs) / 2;

        //-------------Basic Visual Representation of Neural Network---------------------------//
        nView = GetComponent<NeuralNetworkView>();
        nView.CreateNetwork(numInputs, numOutputs, numHiddenLayers, numNodeHiddenLayers);

        //Intiation of neural network array for the population
        nNetwork = new NeuralNet[population];

        source = GetComponent<AudioSource>();

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

            //Used to track the best performing spawn
            double bestFitness = 0;
            int bestSpawn = 0;

            //TimeScale
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                Time.timeScale *= .5f;
            }
            if (Input.GetKeyDown(KeyCode.Period))
            {
                Time.timeScale *= 2;
            }
            //----------------Use the first spawn as the selected object in visual representation--------------//

            //nView.NodeUpdate(input, nNetwork[0]);

            //More Stats
            genAlg.CalculateBestWorstAvTot();
            gameInfo.text += "\nBest Fitness: " + genAlg.bestFitness + "\nAverage Fitness: " + genAlg.averageFitness + "\nLowest Fitness: " + genAlg.worstFitness;
        }



    }

    protected void RunGameSetup()
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

        minWorldPosX = grid.grid[0, 0].worldPosition.x;
        minWorldPosY = grid.grid[0, 0].worldPosition.y;
        maxWorldPosX = grid.grid[grid.gridSizeX - 1, grid.gridSizeY - 1].worldPosition.x;
        maxWorldPosY = grid.grid[grid.gridSizeX - 1, grid.gridSizeY - 1].worldPosition.y;

        grid.CreateGrid();

        // spawn Asteroid and Stars and Background
        AddStars(starCount);
        AddAsteroids(asteroidCount);

        GameObject borderBlock = Instantiate(border, Vector3.zero, Quaternion.identity) as GameObject;
        borderBlock.transform.localScale = new Vector3(gridWorldSize.x, gridWorldSize.y, 1);

        //Add Player
        int index = UnityEngine.Random.Range(0, grid.walkableNodes.Count);
        Node node = grid.walkableNodes[index];
        player = Instantiate(playerInstance, node.worldPosition, Quaternion.identity) as GameObject;
        //player.GetComponent<SpriteRenderer>().color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        //player.transform.localScale = new Vector3(1.8f, 1.8f);
        //BasePlayer playerScript = player.AddComponent<Player>();
        player.name = "player1";
        player.tag = "Player";

        //Spawn AI
        AddAI(aiCount);
        

    }

    protected void AddAI(int aiCount)
    {
        GameObject aiEmpty = new GameObject("AIOpponents");
        for (int i = 0; i < aiCount; i++)
        {
            int index = UnityEngine.Random.Range(0, grid.walkableNodes.Count);
            Node node = grid.walkableNodes[index];
            GameObject ai = Instantiate(aiOpponent, node.worldPosition, Quaternion.identity, aiEmpty.transform) as GameObject;
            ai.GetComponent<SpriteRenderer>().color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            ai.name = "AI " + i;

            //-------Create neural network with defined inputs and ouputs for each spawn----------//
            nNetwork[i] = new NeuralNet(numInputs, numOutputs, numHiddenLayers, numNodeHiddenLayers);
            

            //-------Creates on Genetic algorithm to handle the whole popluation-----------------//
            if (i == 0)
            {
                genAlg = new GeneticAlgorithm(population, .2, .7, nNetwork[0].GetNumWeights());
            }
            //--------Set the random weights generated in genAlg into--------------------------//
            nNetwork[i].PutWeights(genAlg.population[i].weights);
            ai.GetComponent<AI>().neuralNet = nNetwork[i];
        }

    }
    protected void AddAsteroids(int astroidCount)
    {
        asteroidEmpty = new GameObject("Astroids");

        for (int i = 0; i < asteroidCount; i++)
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
        
    }

    protected void AddStars(float num)
    {
        GameObject starEmpty = new GameObject("Stars");
        for (int i = 0; i < num; i++)
        {
            float starSize = Random.Range(m_minStarSize, m_maxStarSize);
            int index = UnityEngine.Random.Range(0, grid.walkableNodes.Count);
            Node node = grid.walkableNodes[index];
            GameObject star = Instantiate(starInstance, node.worldPosition, Quaternion.identity, starEmpty.transform) as GameObject;
            star.transform.localScale = new Vector3(starSize, starSize);
        }
        
    }

    internal void GameOver()
    {
        setup = true;
        Time.timeScale = .001f;
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

    protected void Timer()
    {
        timer += Time.deltaTime;
        gameTimer.text = "Health: " + player.GetComponent<Player>().health;
    }
    protected void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
