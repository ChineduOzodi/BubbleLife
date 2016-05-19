using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour
{

	public int borderSize = 200;
	public int foodLimit = 500;
	public int aiCount = 20;

	public static GameManager instance = null;
	string savePath;
	public bool setup = false;

	public GameObject border;
	public GameObject food;
	public GameObject bubble;
	public GameObject infoCanvas;

	private GameObject player;

	GameObject borderEmpty;
	GameObject foodEmpty;

	List<Coord> spawnLocations;


	// Use this for initialization
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		savePath = Application.persistentDataPath + "gamesave.sav";

		spawnLocations = new List<Coord> ();

		setup = false;

		borderEmpty = new GameObject ("border");
		foodEmpty = new GameObject ("food");

		RunSetup ();
	}
		

	public void Save()
	{
		try
		{
			//savePath = Application.persistentDataPath + "/World/" + worldGen.world.worldName + "_Auto Save.sav";//"worldGen.world.saveNum"
		}
		catch (DirectoryNotFoundException)
		{
			// Directory.CreateDirectory(Application.persistentDataPath + "/World/");
			//savePath = Application.persistentDataPath + "/World/" + worldGen.world.worldName + "_Auto Save.sav";//"worldGen.world.saveNum"
		}
//		BinaryFormatter bf = new BinaryFormatter();
//
//		FileStream file = File.Create(savePath);
//
//		World world = worldGen.world;
//		bf.Serialize(file, world);
//		file.Close();
//		worldGen.world.saveNum++;
//		if (worldGen.world.saveNum > numAutoSave)
//			worldGen.world.saveNum = 1;
	}
	public void Load()
	{
//		if (File.Exists(savePath))
//		{
//			BinaryFormatter bf = new BinaryFormatter();
//			FileStream file = File.Open(savePath, FileMode.Open);
//			worldGen.world = (World)bf.Deserialize(file);
//			file.Close();
//
//			worldGen.loadWorld();
//
//		}
	}

	private void RunSetup(){
		setup = true;

		for (int x = 0; x <= borderSize; x++) {
			for (int y = 0; y <= borderSize; y++) {
				if (x == 0 || y == 0 || x == borderSize || y == borderSize) {
					GameObject borderBlock = Instantiate (border, new Vector3 (x, y), Quaternion.identity) as GameObject;
					borderBlock.transform.SetParent (borderEmpty.transform);
				} else {
					spawnLocations.Add (new Coord (x, y));
				}
				//spawnLocations.Add (new Coord (x, y));
			}
					
		}

		//spawn Food
		for (int i = 0; i < foodLimit; i++) {
			AddFood ();
		}

		//Add Player
		int index = UnityEngine.Random.Range (0, spawnLocations.Count);
		Coord location = spawnLocations [index];
		player = Instantiate (bubble, new Vector3 (location.x, location.y), Quaternion.identity) as GameObject;
		player.GetComponent<SpriteRenderer> ().color = new Color (UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
		player.transform.localScale = new Vector3(1.8f,1.8f);
		player.AddComponent<Player>();
		//player.AddComponent<Rigidbody2D> ();
		player.name = "player1";
		player.tag = "Player";

		//Spawn AI
		for (int i = 0; i < aiCount; i++) {
			AddAI ();
		}


		setup = false;



	}
	public void AddAI (){
		int index = UnityEngine.Random.Range (0, spawnLocations.Count);
		Coord location = spawnLocations [index];
		GameObject ai = Instantiate (bubble, new Vector3 (location.x, location.y), Quaternion.identity) as GameObject;
		ai.GetComponent<SpriteRenderer> ().color = new Color (UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
		ai.transform.localScale = new Vector3(1.8f,1.8f);
		ai.name = "AI";
		ai.tag = "ai";
		//ai.AddComponent<Rigidbody2D> ();
		AI aiSCript = ai.AddComponent<AI>();

		//ai.transform.SetParent (foodEmpty.transform);
	}
	private void AddFood (){
		int index = UnityEngine.Random.Range (0, spawnLocations.Count);
		Coord location = spawnLocations [index];
		spawnLocations.RemoveAt (index);
		GameObject bubbleFood = Instantiate (food, new Vector3 (location.x, location.y), Quaternion.identity) as GameObject;
		bubbleFood.GetComponent<SpriteRenderer> ().color = new Color (UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
		bubbleFood.transform.localScale = new Vector3(.56f,.56f);
		Food bubbleFoodFood = bubbleFood.AddComponent<Food>();
		bubbleFoodFood.coord = location;
		bubbleFood.transform.SetParent (foodEmpty.transform);
	}

	public void AddFood (Coord coord){
		AddFood ();
		spawnLocations.Add (coord);
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
