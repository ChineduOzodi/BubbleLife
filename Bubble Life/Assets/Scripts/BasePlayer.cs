using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BasePlayer : MonoBehaviour {

	public int food = 10;
	public float width = 1.8f;
	protected GameManager gameManager;
	protected float speedMod = 40;
	protected float maxSpeed = 10;
	protected Text info;
	protected float diffScale = .1f;
	protected Rigidbody2D rigid;
	protected Vector3 offset;
	protected Queue<BasePlayer> bubbleGroup;
	public bool isLeader = false;
	public BasePlayer leader;
	public List<BasePlayer> children;


	protected void Eat(){
		
	}

	protected void UpdateSize(){
		width = Mathf.Sqrt (food / Mathf.PI);
		transform.localScale = new Vector3 (width, width);
	}

	void Awake(){
		gameManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
		info = transform.GetComponentInChildren<Text> ();
		rigid = gameObject.GetComponent<Rigidbody2D> ();
	}
	protected void Move(Vector3 target){
		offset = target - transform.position;
		if (transform.position.x < 0) {
			offset = new Vector3 (10, 9);
		} else if (transform.position.y < 0) {
			offset = new Vector3 (0, 10);
		} else if (transform.position.x > gameManager.borderSize) {
			offset = new Vector3 (-10, 9);
		} else if (transform.position.y > gameManager.borderSize) {
			offset = new Vector3 (0, -10);
		} else {
			maxSpeed = speedMod / width;
			float scale = (maxSpeed * maxSpeed - rigid.velocity.sqrMagnitude);
			offset.Normalize ();
			offset.Scale (new Vector3 (scale, scale));
		}
		rigid.AddForce (offset);
		if (isLeader) {
			for (int a = 0; a < children.Count; a++) {
				children [a].transform.GetComponent<Rigidbody2D> ().AddForce (offset);
			}
		}

//		if (rigid.velocity.sqrMagnitude > maxSpeed * maxSpeed){
//			rigid.velocity.Normalize();
//			rigid.velocity.Scale (new Vector2 (maxSpeed, maxSpeed));
//		}
	}

	void OnTriggerStay2D( Collider2D col){

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

	protected void Attack(){

		GameObject obj = Instantiate (gameManager.bubble,transform.position,Quaternion.identity) as GameObject;
		offset = offset * width;
		obj.GetComponent<Rigidbody2D> ().velocity = rigid.velocity * width * .75f;
		BasePlayer objPlayer = obj.AddComponent<BasePlayer> ();
		obj.tag = transform.tag;
		food = food / 2;
		objPlayer.food = food;
		UpdateSize ();
		objPlayer.UpdateSize ();

		if (isLeader) {
			children.Add (objPlayer);
			objPlayer.leader = transform.GetComponent<BasePlayer> ();

		} else {
			leader.AddChildren (objPlayer);
			objPlayer.leader = leader;

		}
	}

	public void AddChildren(BasePlayer child){
		
		children.Add (child);
	}
}
