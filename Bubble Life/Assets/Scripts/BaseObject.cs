using UnityEngine;
using System.Collections;

public class BaseObject : MonoBehaviour {

    public float health = 100;
    protected LevelScript levelScript;
    protected Rigidbody2D rigid;

    // Use this for initialization
    void Awake () {

        levelScript = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelScript>();
        rigid = gameObject.GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {

        if (transform.position.x < levelScript.minWorldPosX)
        {
            transform.position = new Vector3(levelScript.maxWorldPosX, transform.position.y, transform.position.z);
        }
        else if (transform.position.y < levelScript.minWorldPosY)
        {
            transform.position = new Vector3(transform.position.x, levelScript.maxWorldPosY, transform.position.z);
        }
        else if (transform.position.x > levelScript.maxWorldPosX)
        {
            transform.position = new Vector3(levelScript.minWorldPosX, transform.position.y, transform.position.z);
        }
        else if (transform.position.y > levelScript.maxWorldPosY)
        {
            transform.position = new Vector3(transform.position.x, levelScript.minWorldPosY, transform.position.z);
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Rigidbody2D colRigid = col.transform.GetComponent<Rigidbody2D>();
        Vector2 finalVel = InelasticCollision(rigid.mass, rigid.velocity, colRigid.mass, colRigid.velocity);

        float damage = Mathf.Abs(rigid.velocity.magnitude - finalVel.magnitude);

        health -= damage;

    }

    public static Vector2 InelasticCollision(float mass1, Vector2 vel1, float mass2, Vector2 vel2)
    {

        Vector2 finalVel = (mass1 * vel1 + mass2 * vel2) / (mass1 + mass2);
        return finalVel;
    }
}
