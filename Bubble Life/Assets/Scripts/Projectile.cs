using UnityEngine;
using System.Collections;
using System;

public class Projectile : MonoBehaviour {

    public float damage = 10;
    protected Rigidbody2D rigid;
    internal GameObject origin;
    internal float lifeTime = 5;

    void Start()
    {
        Destroy(gameObject, lifeTime);
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            col.transform.GetComponent<Player>().health -= 10;
            CheckOrigin();
        }
        else if (col.transform.CompareTag("AI"))
        {
            col.transform.GetComponent<AI>().health -= 10;
            col.transform.GetComponent<AI>().fitness -= 10;
            CheckOrigin();
        }
        else if (col.transform.tag == "Asteroid")
        {
            Rigidbody2D colRigid = col.transform.GetComponent<Rigidbody2D>();
            Vector2 finalVel = BasePlayer.InelasticCollision(rigid.mass, rigid.velocity, colRigid.mass, colRigid.velocity);

            float damage = Mathf.Abs(colRigid.velocity.magnitude - finalVel.magnitude);

            col.transform.GetComponent<AsteroidController>().health -= 1 + damage;
            //print("Hit Asteroid: " + col.transform.GetComponent<AsteroidController>().health);
        }
        else
        {
            //print("Hit " + col.transform.tag);
        }

        Destroy(gameObject);
    }

    private void CheckOrigin()
    {
        if (origin.CompareTag("AI"))
        {
            origin.GetComponent<AI>().fitness += 10;
        }
    }
}
