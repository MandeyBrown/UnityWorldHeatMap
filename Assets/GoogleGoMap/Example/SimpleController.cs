using UnityEngine;

public class SimpleController : MonoBehaviour
{
	public float speed = 50.0F;
	public float gravity = 20.0F;
	private Vector3 rotateValue;

	private Vector3 moveDirection = Vector3.zero;
	public CharacterController controller;
	public GameObject cam;

	private float curSpeed = 0.0f;
	private float acc = 2f; 
	private bool up = true;


	void Start(){
		// Store reference to attached component
		controller = GetComponent<CharacterController>();
	}

	void Update() 
	{
		
		// Use input up and down for direction, multiplied by speed
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= speed;
		

		if(moveDirection.magnitude > 0.001)
			controller.Move(moveDirection * Time.deltaTime);

		/*if (Input.GetKey (KeyCode.U)) {
			//Debug.Log ("HERE");
			Vector3 moveDirection1 = new Vector3(0, 300, 0);
			controller.Move(moveDirection1 * Time.deltaTime);

		}*/

		// UP
		//if (Input.GetKey (KeyCode.O)) {
		if (Input.GetButton ("Jump")) {
			//Debug.Log ("HERE");

			float jumpVal = Input.GetAxis ("Jump");
			bool currUp = true;
			if (jumpVal < 0) {
				currUp = false;
			}

			if (currUp != up) {
				curSpeed = 0f;
				up = currUp;
			}

			curSpeed += acc * jumpVal;
			Vector3 moveDirection1 = new Vector3 (0, curSpeed, 0);
			controller.Move (moveDirection1 * Time.deltaTime);

		}
		// DOWN
		/*if (Input.GetKey (KeyCode.P)) {
			//Debug.Log ("HERE");
			Vector3 moveDirection1 = new Vector3(0, -100, 0);
			controller.Move(moveDirection1 * Time.deltaTime);

		}


		if (Input.GetKey (KeyCode.R)) {
			cam.transform.Rotate(5, 0, 0, Space.Self);

		}

		if (Input.GetKey (KeyCode.T)) {
			cam.transform.Rotate(-5, 0, 0, Space.Self);

		}*/



	}
}