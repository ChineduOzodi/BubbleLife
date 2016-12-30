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

    private float volLow = .5f;
    private float volHigh = 1f;
    private float freqLow = .75f;
    private float freqHigh = 1.25f;
    
    void Start(){

        if (!levelScript.debug)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }

		info = transform.GetComponentInChildren<Text> ();

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
            levelScript.GameOver();
            Dead();
        }
    }
	protected void Attack(){
        source.pitch = 1f;
        source.PlayOneShot(fireSound, Random.Range(volLow, volHigh));

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
