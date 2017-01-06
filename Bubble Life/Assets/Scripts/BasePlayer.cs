using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BasePlayer : BaseObject {

    public GameObject projectile;
    public GameObject gunLocation;
    public GameObject playerFlame;
    public AudioClip fireSound;

    protected GameController gameController;
    
    public float projectilePower = 10;
    public float speedMod = 400;
    public float turnSpeed = 200;
    public bool destroy = false;

    protected float force = 1;
	protected Text info;
	protected Vector3 offset;
    
    void Start(){

        if (!levelScript.debug)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }

		info = transform.GetComponentInChildren<Text>();

        playerFlame.gameObject.SetActive(false);

    }

    protected void CheckHealth()
    {
        if (health < 0)
        {
            animator.SetBool("isDead", true);
        }
        if (destroy)
        {
            //Reset player;
            //animator.SetBool("isDead", false);
            destroy = false;
            if (gameObject.CompareTag("Player"))
                levelScript.GameOver();
            Dead();
        }
    }
	protected void Attack(){
        source.pitch = 1f;
        source.PlayOneShot(fireSound, Random.Range(volLow, volHigh));

		GameObject obj = Instantiate (projectile,gunLocation.transform.position,Quaternion.identity, levelScript.transform) as GameObject;
        obj.transform.rotation = transform.rotation;
        obj.GetComponent<Rigidbody2D>().velocity = rigid.velocity + (Vector2)(gunLocation.transform.up * projectilePower);
        obj.GetComponent<Projectile>().origin = gameObject;
	}

    protected void Move()
    {
        rigid.AddRelativeForce(Vector2.up * speedMod * Time.deltaTime);
    }

    protected void Dead()
    {
        gameObject.SetActive(false);
    }
}
