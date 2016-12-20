using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BasePlayer : MonoBehaviour {

    public GameObject projectile;
    public GameObject gunLocation;

    protected GameController gameController;
    protected LevelScript levelScript;
    public int health = 100;
    public float projectilePower;


    public float speedMod = 400;
	protected float force = 1;
	protected Text info;
	protected Rigidbody2D rigid;
	protected Vector3 offset;
    

    void Awake(){
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
        levelScript = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelScript>();

		info = transform.GetComponentInChildren<Text> ();
		rigid = gameObject.GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate(){

        if (transform.position.x < levelScript.minWorldPosX)
        {
            offset = new Vector3(force, 0);
            rigid.AddForce(offset);
        }
        else if (transform.position.y < levelScript.minWorldPosY)
        {
            offset = new Vector3(0, force);
            rigid.AddForce(offset);
        }
        else if (transform.position.x > levelScript.maxWorldPosX)
        {
            offset = new Vector3(-force, 0);
            rigid.AddForce(offset);
        }
        else if (transform.position.y > levelScript.maxWorldPosY)
        {
            offset = new Vector3(0, -force);
            rigid.AddForce(offset);
        }
    }

	protected Vector2 InelasticCollision (float mass1, Vector2 vel1, float mass2, Vector2 vel2){

		Vector2 finalVel = (mass1 * vel1 + mass2 * vel2) / (mass1 + mass2);
		return finalVel;
	}

	protected void Attack(){

		GameObject obj = Instantiate (projectile,transform.position,Quaternion.identity) as GameObject;
        obj.GetComponent<Rigidbody2D>().velocity = rigid.velocity + (Vector2)(gunLocation.transform.up * projectilePower);
	}

    protected void Move()
    {
        rigid.AddRelativeForce(Vector2.up * speedMod * Time.deltaTime);
    }
}
