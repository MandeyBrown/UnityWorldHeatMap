using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
	//public GameObject cam;
	private Vector3 rotateValue;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Move Character Controller



		// Rotate Camera +
		if (Input.GetKey (KeyCode.R)) {
			rotateValue = new Vector3 (10,0,0);
			transform.Rotate (Vector3.right * Time.deltaTime);

		}

		// Rotate Camera -
		if (Input.GetKey (KeyCode.T)) {
			//Debug.Log ("HERE");
			transform.Rotate(10, 0, 0, Space.Self);

		}
	}
}
