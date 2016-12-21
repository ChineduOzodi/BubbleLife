using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float damage = 10;

    void Start()
    {
        Destroy(gameObject, 5);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Player")
        {
            col.transform.GetComponent<Player>().health -= 10;
            print("Hit " + col.transform.tag);
        }
        else if (col.transform.tag == "Asteroid")
        {
            col.transform.GetComponent<AsteroidController>().health -= 10;
            print("Hit Asteroid: " + col.transform.GetComponent<AsteroidController>().health);
        }
        else
        {
            print("Hit " + col.transform.tag);
        }

        Destroy(gameObject);
    }
}
