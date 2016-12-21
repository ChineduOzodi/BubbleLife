using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float damage = 10;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Player")
        {
            col.transform.GetComponent<Player>().health -= 10;
        }
        else if (col.transform.tag == "Asteroid")
        {
            col.transform.GetComponent<AsteroidController>().health -= 10;
        }

        Destroy(gameObject);
    }
}
