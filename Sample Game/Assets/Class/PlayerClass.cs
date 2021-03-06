using UnityEngine;
using System.Collections;

public class PlayerClass : MonoBehaviour {
	//just after class declaration line
	public float speed;
	public GameObject bombPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//inside the Update method
		/*if (transform.position.x > 18) {
			//get new speed
			speed = Random.Range(8f,12f);
			transform.position = new Vector3( -18f, transform.position.y, transform.position.z );
		}*/
		if (transform.position.x < -19 || transform.position.x > 20) {
			//turn around
			transform.Rotate(new Vector3(0,180,0));
			//transform.Translate( new Vector3(-10, transform.position.y,8) );
			transform.position = new Vector3( transform.position.x, transform.position.y, transform.position.z );
			//get new speed
			speed = Random.Range(8f,12f);
		}
		transform.Translate(0, 0, speed * Time.deltaTime);	
		
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			//Check if user has bombs remaining, do not fire if bombs are not remaining
			if (UserDisplayClass.bombsLeft > 0) {
				GameObject bombObject = (GameObject)Instantiate(bombPrefab);
				bombObject.transform.position = this.gameObject.transform.position;
				UserDisplayClass.bombsLeft--;
			} else {
				//go to game over screen
				Application.LoadLevel(3);
			}
		}

		if (Input.GetKeyDown (KeyCode.Mouse1)) {
			transform.Rotate(new Vector3(0,180,0));

				}
	}
}
