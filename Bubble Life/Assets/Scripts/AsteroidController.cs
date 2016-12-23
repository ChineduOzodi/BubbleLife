using UnityEngine;
using System.Collections;
using System;

public class AsteroidController : BaseObject {

    public float size;
    public float divideForce = 1;
    public float unitSize;

    public GameObject divideAsteroid;

    // Use this for initialization
    void Start () {

        health = 100;

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
        obj.transform.rotation = gameObject.transform.rotation;
        obj.transform.position += ((obj.transform.up) * 2f);
        obj.transform.localScale = gameObject.transform.localScale * .5f;
        obj.GetComponent<Rigidbody2D>().velocity = rigid.velocity + (Vector2)((obj.transform.up) * divideForce);

        GameObject obj2 = Instantiate(divideAsteroid, transform.position, Quaternion.identity) as GameObject;
        obj2.transform.localScale = gameObject.transform.localScale * .5f;
        obj2.transform.rotation = gameObject.transform.rotation;
        obj2.transform.position += ((obj.transform.up) * -2f);
        obj2.GetComponent<Rigidbody2D>().velocity = rigid.velocity + (Vector2)((obj.transform.up) * -divideForce);

        Explode();
    }
}
