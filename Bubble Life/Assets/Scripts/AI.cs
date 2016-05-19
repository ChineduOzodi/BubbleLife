using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : BasePlayer {

	Vector3 moveDirection = Vector3.zero;
	bool nofood = false;
	Vector3 foodFindpos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Collider2D[] hitColliders = Physics2D.OverlapCircleAll (transform.position, width * 1.5f + 10f);
		Queue<Collider2D> enemyQ = new Queue<Collider2D> ();
		Queue<Collider2D> preyQ = new Queue<Collider2D> ();
		Queue<Collider2D> foodQ = new Queue<Collider2D> ();
		for (int i = 0; i < hitColliders.Length; i++) {
			Collider2D col = hitColliders [i];
			if (col.tag == "food") {
				foodQ.Enqueue (col);
			} else if (col.tag == "border") {
			} else {
				BasePlayer colPlayer = col.gameObject.GetComponent<BasePlayer> ();
				if (colPlayer.width > width * (1f + diffScale)) {
					enemyQ.Enqueue (col);
				} else if (colPlayer.width < width * (1f - diffScale)) {
					foodQ.Enqueue (col);
				}
			}
		}
		if (enemyQ.Count > 0) {
			Collider2D col = enemyQ.Dequeue ();
			Vector3 offset = col.transform.position - transform.position;
			offset.Scale (new Vector3 (-1f, -1f));
			moveDirection = offset + transform.position;
			transform.position = Vector3.MoveTowards (transform.position, moveDirection, speedMod / width * Time.deltaTime);
		} else {
			int foodSum = 0;
			float scaleSum = 0f;
			moveDirection = moveDirection - transform.position;
			moveDirection.Normalize ();
			//print (moveDirection.ToString ());
			if (foodQ.Count == 0 && nofood == false) {
				foodFindpos = GameObject.FindGameObjectWithTag ("food").transform.position;
				nofood = true;
			}
			if (foodQ.Count > 0 || preyQ.Count > 0){
				nofood = false;
				while (foodQ.Count > 0) {
					Collider2D col = foodQ.Dequeue ();
					float scale = col.transform.GetComponent<Food>().amount / Vector3.Distance (col.transform.position, transform.position);
					Vector3 offset = col.transform.position - transform.position;
					offset.Normalize ();
					offset.Scale (new Vector3 (scale, scale));
					scaleSum += scale;
					moveDirection = moveDirection + offset;

				}
				while (preyQ.Count > 0) {
					Collider2D col = preyQ.Dequeue ();
					float scale = col.transform.GetComponent<BasePlayer>().food / Vector3.Distance (col.transform.position, transform.position);
					Vector3 offset = col.transform.position - transform.position;
					offset.Normalize ();
					offset.Scale (new Vector3 (scale, scale));
					scaleSum += scale;
					moveDirection = moveDirection + offset;

				}

				moveDirection.Scale (new Vector3 (1f / scaleSum, 1f / scaleSum));
				moveDirection = moveDirection + transform.position;
				//print (moveDirection.ToString ());
				transform.position = Vector3.MoveTowards (transform.position, moveDirection, speedMod / width * Time.deltaTime);
			} else {
				transform.position = Vector3.MoveTowards (transform.position, foodFindpos, speedMod / width * Time.deltaTime);
			}
		}
			
	
	}
}
