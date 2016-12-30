using UnityEngine;
using System.Collections;
using System;

public class Player : BasePlayer {

	private Vector3 mousePosition;
	private int camSize = 5;
    private int zoomSpeed = 5;
	
	// Update is called once per frame
	void Update() {

		if (!levelScript.setup) {
			mousePosition = Input.mousePosition;
			mousePosition = Camera.main.ScreenToWorldPoint (mousePosition);
			mousePosition.z = transform.position.z;

			//Camera.main.orthographicSize = camSize + width * 2;
			Camera.main.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * -zoomSpeed;
			Camera.main.transform.position = new Vector3(transform.position.x,transform.position.y,-1f);

			float transX = Input.GetAxis ("Horizontal") * turnSpeed * Time.deltaTime;
			float transY = Input.GetAxis ("Vertical") * turnSpeed * Time.deltaTime;

			//print (transX.ToString () + transY.ToString ());
			transform.Rotate(new Vector3(0,0,-transX));
            //transform.Translate (new Vector3 (trans90=-p0oiuyg=]-[098juhygtr=76-0987656tre8765trewsaerwq32Q X, transY));

            if (Input.GetButton("Jump") && playerFlame.activeSelf == false)
            {
                playerFlame.gameObject.SetActive(true);
                source.Play();
                Move();
            }
            else if (Input.GetButton("Jump")){
                Move();
            }
            else if (playerFlame.activeSelf == true)
            {
                playerFlame.gameObject.SetActive(false);
                source.Stop();
            }

            if (Input.GetButtonDown("Fire1"))
            {
                Attack();
            }

            CheckBorder();
            CheckHealth();
		}

	}

    
}
