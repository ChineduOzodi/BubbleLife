using UnityEngine;
using System.Collections;

public class BaseObject : MonoBehaviour {

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
}
