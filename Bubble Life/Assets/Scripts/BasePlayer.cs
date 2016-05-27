using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BasePlayer : MonoBehaviour {

	public int food = 10;
	public float width = 1.8f;
	protected GameManager gameManager;
	protected float speedMod = 40;
	protected float wallForce = 1;
	protected Text info;
	protected Rigidbody2D rigid;
	protected Vector3 offset;


	protected void Eat(){
		
	}

	public void UpdateSize(){
		width = Mathf.Sqrt (food / Mathf.PI);
		transform.localScale = new Vector3 (width, width);
	}

	void Awake(){
		gameManager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
		info = transform.GetComponentInChildren<Text> ();
		rigid = gameObject.GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate(){
		
		if (transform.position.x < 0) {
			offset = new Vector3 (wallForce, 0);
			rigid.AddForce (offset);
		} else if (transform.position.y < 0) {
			offset = new Vector3 (0, wallForce);
			rigid.AddForce (offset);
		} else if (transform.position.x > gameManager.borderSize) {
			offset = new Vector3 (-wallForce, 0);
			rigid.AddForce (offset);
		} else if (transform.position.y > gameManager.borderSize) {
			offset = new Vector3 (0, -wallForce);
			rigid.AddForce (offset);
		}
	}
	
    protected void Move(Vector3 target) {
        offset = target - transform.position;
        offset.Normalize();
        GameObject obj = Instantiate(gameManager.food, transform.position + offset * width/1.7f, Quaternion.identity) as GameObject;
		obj.GetComponent<Rigidbody2D> ().velocity = rigid.velocity;
        obj.GetComponent<Rigidbody2D> ().AddForce(offset * speedMod);
        rigid.AddForce(offset * -speedMod);
        BasePlayer objPlayer = obj.AddComponent<BasePlayer>();
        food--;
        objPlayer.food = 1;
        UpdateSize();
        objPlayer.UpdateSize();

    }

	void OnTriggerStay2D( Collider2D col){

	
		if (col.gameObject.tag != "border") {
			BasePlayer colScript = col.GetComponent<BasePlayer> ();
			if (colScript.width < width && col.gameObject.tag != "Player") {
				food += colScript.food;

				Rigidbody2D colRigid = col.GetComponent<Rigidbody2D> ();
				Vector2 newVelocity = InelasticCollision (rigid.mass, rigid.velocity, colRigid.mass, colRigid.velocity);
                gameManager.AddFood(1);
				Destroy (col.gameObject);
				UpdateSize ();
				rigid.velocity = newVelocity;
                if (transform.tag != "food")
                {
                    info.text = food.ToString();
                }
            }
            
        }

		
	}

	protected Vector2 InelasticCollision (float mass1, Vector2 vel1, float mass2, Vector2 vel2){

		Vector2 finalVel = (mass1 * vel1 + mass2 * vel2) / (mass1 + mass2);
		return finalVel;
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
	}
}
