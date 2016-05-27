using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BasePlayer : MonoBehaviour {

	public int food = 10;
	public float width = 1.8f;
	protected GameManager gameManager;
	protected float speedMod = 40;
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
	
    protected void Move(Vector3 target) {
        offset = target - transform.position;
        offset.Normalize();
        GameObject obj = Instantiate(gameManager.food, transform.position + offset, Quaternion.identity) as GameObject;
        obj.GetComponent<Rigidbody2D>().AddForce(offset * speedMod);
        rigid.AddForce(offset * -speedMod);
        BasePlayer objPlayer = obj.AddComponent<BasePlayer>();
        food--;
        objPlayer.food = 1;
        UpdateSize();
        objPlayer.UpdateSize();

    }

	void OnTriggerStay2D( Collider2D col){

	
		if (col.gameObject.tag != "border") {
			BasePlayer colScript = col.gameObject.GetComponent<BasePlayer> ();
			if (colScript.width < width && col.gameObject.tag != "Player") {
				food += colScript.food;
                gameManager.AddFood(1);
				Destroy (col.gameObject);
				UpdateSize ();
                if (transform.tag != "food")
                {
                    info.text = food.ToString();
                }
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
	}
}
