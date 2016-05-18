using UnityEngine;
using System.Collections;

public class BasePlayer : MonoBehaviour {

	int food = 10;
	public float width = 1.8f;
	protected GameManager gameManager;
	protected float speedMod = 40;

	protected void Eat(){
		
	}

	protected void UpdateSize(){
		width = Mathf.Sqrt (food / Mathf.PI);
		transform.localScale = new Vector3 (width, width);
	}

	void Awake(){
		gameManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
	}
	void update(){
	}

	void OnCollisionEnter2D( Collision2D col){

		if (col.gameObject.tag == "food") {
			Food colFood = col.gameObject.GetComponent<Food> ();
			food += colFood.amount;
			gameManager.SendMessage ("AddFood", colFood.coord);
			Destroy (col.gameObject);

			UpdateSize ();
		}

		
	}

}
