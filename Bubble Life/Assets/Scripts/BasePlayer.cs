using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BasePlayer : BaseObject {

    public GameObject projectile;
    public GameObject gunLocation;
    public GameObject playerFlame;

    protected GameController gameController;
    protected Animator animator;
    
    
    public float projectilePower = 10;


    public float speedMod = 400;
    public float turnSpeed = 200;
	protected float force = 1;
	protected Text info;
	
	protected Vector3 offset;

    public bool destroy = false;


    public void Start(){
        animator = GetComponent<Animator>();

        if (!levelScript.debug)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
        
        

		info = transform.GetComponentInChildren<Text> ();
		
        playerFlame.gameObject.SetActive(false);
	}

    void Update()
    {
        if (health < 0)
        {
            animator.SetBool("isDead", true);
        }
        if (destroy)
        {
            levelScript.GameOver();
            Dead();
        }
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

    protected void Dead()
    {
        Destroy(gameObject);
    }
}
