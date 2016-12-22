using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BasePlayer : BaseObject {

    public GameObject projectile;
    public GameObject gunLocation;
    public GameObject playerFlame;

    protected GameController gameController;
    
    public float health = 100;
    public float projectilePower = 10;


    public float speedMod = 400;
    public float turnSpeed = 200;
	protected float force = 1;
	protected Text info;
	
	protected Vector3 offset;

    private bool impact = false;


    void start(){
        

        if (!levelScript.debug)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
        
        

		info = transform.GetComponentInChildren<Text> ();
		
        playerFlame.gameObject.SetActive(false);
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        Rigidbody2D colRigid = col.transform.GetComponent<Rigidbody2D>();
        Vector2 finalVel = InelasticCollision(rigid.mass, rigid.velocity, colRigid.mass, colRigid.velocity);

        float damage = Mathf.Abs(rigid.velocity.magnitude - finalVel.magnitude);

        health -= damage;

        if (col.transform.tag == "Player")
        {
            //col.transform.GetComponent<Player>().health -= 10;
            //print("Hit " + col.transform.tag);
        }
        else if (col.transform.tag == "Asteroid")
        {
            col.transform.GetComponent<AsteroidController>().health -= Mathf.Abs(colRigid.velocity.magnitude - finalVel.magnitude);
            print("Hit Asteroid: " + health.ToString("0.00"));
        }
        else
        {
            //print("Hit " + col.transform.tag);
        }
    }

    public static Vector2 InelasticCollision (float mass1, Vector2 vel1, float mass2, Vector2 vel2){

		Vector2 finalVel = (mass1 * vel1 + mass2 * vel2) / (mass1 + mass2);
		return finalVel;
	}

	protected void Attack(){

		GameObject obj = Instantiate (projectile,gunLocation.transform.position,Quaternion.identity) as GameObject;
        obj.transform.rotation = transform.rotation;
        obj.GetComponent<Rigidbody2D>().velocity = rigid.velocity + (Vector2)(gunLocation.transform.up * projectilePower);
	}

    protected void Move()
    {
        rigid.AddRelativeForce(Vector2.up * speedMod * Time.deltaTime);
    }
}
