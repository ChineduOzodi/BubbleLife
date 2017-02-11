using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BasePlayer : BaseObject {

    public Projectile projectile;
    public Projectile explosionParticle;
    public GameObject gunLocation;
    public GameObject playerFlame;
    public AudioClip fireSound;

    protected GameController gameController;
    protected Text info;
    protected Vector3 offset;
    protected float turnSpeed = 200;

    private float projectilePower = 10;
    private float projectileResetTime = 3;
    private int projectileMaxAmount = 5;
    private int explosionParticleCount = 20;
    private int explosionPower = 15;
    private float speedMod = 400;
    private float maxSpeed = 10;

    private int projectileAmount;
    private float projectileTime = 0;
	
    
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
            if (gameObject.CompareTag("Player"))
                levelScript.GameOver();
            Dead();
        }
        
    }
	protected virtual void Attack(){
        if (projectileTime < Time.time)
        {
            projectileTime = Time.time + projectileResetTime;
            projectileAmount = projectileMaxAmount;
        }
        if (projectileAmount > 0)
        {
            projectileAmount--;
            
            //Sound
            source.pitch = 1f;
            source.PlayOneShot(fireSound, Random.Range(volLow, volHigh));

            //Projectile Instance
            GameObject obj = Instantiate(projectile.gameObject, gunLocation.transform.position, Quaternion.identity, levelScript.transform) as GameObject;
            obj.transform.rotation = transform.rotation;
            obj.GetComponent<Rigidbody2D>().velocity = rigid.velocity + (Vector2)(gunLocation.transform.up * projectilePower);
            obj.GetComponent<Projectile>().origin = gameObject;
        }
	}

    protected void Move()
    {
        rigid.AddRelativeForce(Vector2.up * speedMod * Time.deltaTime);
    }

    protected void Dead()
    {
        Explosion(explosionParticle, explosionParticleCount);

        gameObject.SetActive(false);
    }

    private void Explosion(Projectile explosionParticle, int explosionParticleCount)
    {
        float explosionAngle = 360 / explosionParticleCount; 

        for ( int i = 1; i < explosionParticleCount + 1; i++)
        {
            Polar2 explosionPolar = new Polar2(1, explosionAngle * Mathf.Deg2Rad * i);

            //Projectile Instance
            GameObject obj = Instantiate(explosionParticle.gameObject, transform.position + (Vector3) explosionPolar.cartesian, Quaternion.identity, levelScript.transform) as GameObject;
            obj.transform.rotation = transform.rotation;
            obj.GetComponent<Rigidbody2D>().velocity = rigid.velocity + (explosionPolar.cartesian * explosionPower);
            obj.GetComponent<Projectile>().origin = gameObject;
            obj.GetComponent<Projectile>().lifeTime = 3;
        }

    }
}
