using UnityEngine;
using System.Collections;

public class AIVisionBlock : MonoBehaviour {

    internal float input = 0;
    internal Vector2 vel;
    internal float distance;

    private SpriteRenderer rend;

    private Collider2D curColl;

    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (curColl != null)
        {
            distance = 1 - Vector3.Distance(transform.position, curColl.transform.position) / 100;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (curColl == null || Vector3.Distance(transform.position, col.transform.position) < Vector3.Distance(transform.position, curColl.transform.position))
        {
            curColl = col;

            Rigidbody2D colRigid = col.transform.GetComponent<Rigidbody2D>();

            if (col.transform.CompareTag("Projectile"))
            {
                if (col.GetComponent<Projectile>().origin.name != GetComponentInParent<AI>().name 
                    && Vector3.Distance(transform.position, col.transform.position) < 20)
                {
                    input = -1f;
                    vel = colRigid.velocity;
                    rend.color = new Color(1, 0, 0, .1f);
                }


            }
            else if (col.transform.CompareTag("Asteroid"))
            {
                input = -1f;
                vel = colRigid.velocity;
                rend.color = new Color(0, 0, 1, .1f);
            }
            else if (col.transform.CompareTag("Player") || col.transform.CompareTag("AI"))
            {
                input = 1f;
                vel = colRigid.velocity;
                rend.color = new Color(0, 1, 0, .1f);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        Rigidbody2D colRigid = col.transform.GetComponent<Rigidbody2D>();

        input = 0;
        vel = Vector2.zero;
        rend.color = new Color(1, 1, 1, .01f);
        curColl = null;

    }
}
