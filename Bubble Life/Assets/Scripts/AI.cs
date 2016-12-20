using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : BasePlayer {

	float repeatFreq = .1f;
	bool nofood;
	float enemyMinDist = 10f;
	Vector3 spawnSpot;

	Vector3 closestEnemy;
	Vector3 closestFood;
	Vector3 closestPrey;
	Vector3 closestBorder;

	// Use this for initialization
	void Start () {

		//InvokeRepeating ("ScanSurroundings", 0, repeatFreq);
		spawnSpot = transform.position;

	
	}
	
	// Update is called once per frame
	void Update(){
		
		//if (Vector3.Distance(closestEnemy,transform.position) < enemyMinDist) {
		//	offset = closestEnemy - transform.position;
		//	offset.Scale (new Vector3 (-1f, -1f));
		//	Move (transform.position + offset);
		//} else {
		//	Move (closestFood);
		//	//offset = closestFood - transform.position;
		//	//offset.Scale (new Vector3 (-1f, -1f));
		//	//offset.Normalize ();
		//	//offset.Scale (new Vector3 (speedMod / width, speedMod / width));
		//	//rigid.AddForce(offset);
		//}
//		if (enemyQ.Count > 0) {
//			Collider2D col = enemyQ.Dequeue ();
//			Vector3 offset = col.transform.position - transform.position;
//			offset.Scale (new Vector3 (-1f, -1f));
//			Vector3 moveDirection = offset + transform.position;
//			transform.position = Vector3.MoveTowards (transform.position, moveDirection, speedMod / width * Time.deltaTime);
//		} else {
//			int foodSum = 0;
//			Vector3 moveDirection = Vector3.zero;
//			//moveDirection = moveDirection - transform.position;
//			//moveDirection.Normalize ();
//			//print (moveDirection.ToString ());
//			if ((foodQ.Count == 0 || preyQ.Count == 0) && nofood == false) {
//				foodFindpos = GameObject.FindGameObjectWithTag ("food").transform.position;
//				nofood = true;
//			}
//			if (foodQ.Count > 0 || preyQ.Count > 0){
//				nofood = false;
//				while (foodQ.Count > 0) {
//					Collider2D col = foodQ.Dequeue ();
//					float scale = col.transform.GetComponent<Food>().amount / Vector3.Distance (col.transform.position, transform.position);
//					Vector3 offset = col.transform.position - transform.position;
//					offset.Normalize ();
//					offset.Scale (new Vector3 (scale, scale));
//					moveDirection = moveDirection + offset;
//
//				}
//				while (preyQ.Count > 0) {
//					Collider2D col = preyQ.Dequeue ();
//					float scale = col.transform.GetComponent<BasePlayer>().food / Vector3.Distance (col.transform.position, transform.position);
//					Vector3 offset = col.transform.position - transform.position;
//					offset.Normalize ();
//					offset.Scale (new Vector3 (scale, scale));
//					moveDirection = moveDirection + offset;
//
//				}
//
//				moveDirection = moveDirection + transform.position;
//				//print (moveDirection.ToString ());
//				transform.position = Vector3.MoveTowards (transform.position, moveDirection, speedMod / width * Time.deltaTime);
//			} else {
//				transform.position = Vector3.MoveTowards (transform.position, foodFindpos, speedMod / width * Time.deltaTime);
//			}
//		}

	}
}
