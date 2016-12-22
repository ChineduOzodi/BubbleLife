using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float damage = 10;
    protected Rigidbody2D rigid;

    void Start()
    {
        Destroy(gameObject, 5);
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Player")
        {
            col.transform.GetComponent<Player>().health -= 10;
            //print("Hit " + col.transform.tag);
        }
        else if (col.transform.tag == "Asteroid")
        {
            Rigidbody2D colRigid = col.transform.GetComponent<Rigidbody2D>();
            Vector2 finalVel = BasePlayer.InelasticCollision(rigid.mass, rigid.velocity, colRigid.mass, colRigid.velocity);

            float damage = Mathf.Abs(colRigid.velocity.magnitude - finalVel.magnitude);

            col.transform.GetComponent<AsteroidController>().health -= 10 + damage;
            //print("Hit Asteroid: " + col.transform.GetComponent<AsteroidController>().health);
        }
        else
        {
            //print("Hit " + col.transform.tag);
        }

        Destroy(gameObject);
    }
}
