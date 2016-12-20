using UnityEngine;
using System.Collections;
using System;

public class AsteroidController : MonoBehaviour {

    public float health = 100;
    public float size;
    public float divideForce = 1;
    public float unitSize;

    public GameObject divideAsteroid;

    protected Rigidbody2D rigid;

    // Use this for initialization
    void Start () {

        rigid = gameObject.GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {

        if (health < 1)
        {
            if (size < 1)
            {
                Explode();
            }
            else
                Divide();
        }
	
	}

    private void Explode()
    {
        //TODO: play explosion

        Destroy(gameObject);


    }

    protected void Divide()
    {
        GameObject obj = Instantiate(divideAsteroid, transform.position, Quaternion.identity) as GameObject;
        obj.GetComponent<Rigidbody2D>().velocity = rigid.velocity;
        obj.GetComponent<Rigidbody2D>().AddForce((obj.transform.up) * divideForce);

        GameObject obj2 = Instantiate(divideAsteroid, transform.position, Quaternion.identity) as GameObject;
        obj2.GetComponent<Rigidbody2D>().velocity = rigid.velocity;
        obj2.GetComponent<Rigidbody2D>().AddForce((obj.transform.up) * -divideForce);

        Explode();
    }
}
