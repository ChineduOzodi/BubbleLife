using UnityEngine;
using System.Collections;

public class Player : BasePlayer {

	private Vector3 mousePosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (!gameManager.setup) {
			mousePosition = Input.mousePosition;
			mousePosition = Camera.main.ScreenToWorldPoint (mousePosition);
			mousePosition.z = transform.position.z;

			transform.position = Vector3.MoveTowards (transform.position, mousePosition, speedMod/ width * Time.deltaTime);
			//transform.position = Vector2.Lerp (transform.position, mousePosition, speedMod);
			//float transX = Input.GetAxis ("Horizontal") * speedMod * Time.deltaTime;
			//float transY = Input.GetAxis ("Vertical") * speedMod * Time.deltaTime;
			//print (transX.ToString () + transY.ToString ());
			//transform.rotation = new Quaternion();
			//transform.Translate (new Vector3 (transX, transY));
		}
	
	}
}
