using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BasePlayer : MonoBehaviour {

	public int food = 10;
	public float width = 1.8f;
	protected GameManager gameManager;
	protected float speedMod = 40;
	protected Text info;
	protected float diffScale = .1f;

	protected void Eat(){
		
	}

	protected void UpdateSize(){
		width = Mathf.Sqrt (food / Mathf.PI);
		transform.localScale = new Vector3 (width, width);
	}

	void Awake(){
		gameManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
		info = transform.GetComponentInChildren<Text> ();
	}
	void update(){
	}

	void OnCollisionEnter2D( Collision2D col){

		if (col.gameObject.tag == "food") {
			Food colFood = col.gameObject.GetComponent<Food> ();
			food += colFood.amount;
			gameManager.SendMessage ("AddFood", colFood.coord);
			Destroy (col.gameObject);
			info.text = food.ToString ();
			UpdateSize ();
		} else if (col.gameObject.tag != "border") {
			BasePlayer colScript = col.gameObject.GetComponent<BasePlayer> ();
			if (colScript.width < width * (1f - diffScale) && col.gameObject.tag != "Player") {
				food += colScript.food;
				gameManager.AddAI ();
				Destroy (col.gameObject);
				info.text = food.ToString ();
				UpdateSize ();
			}
		}

		
	}

}
