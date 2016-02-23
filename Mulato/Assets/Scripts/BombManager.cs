using UnityEngine;
using System.Collections;

public class BombManager : MonoBehaviour {
	public float explusionTime = 3;
	public int power = 1;
	void Start() {
		SetTheBomb (explusionTime, power);
	}

	public void SetTheBomb(float explusionTime, int power) {
		//GetComponent <BoxCollider2D> ().enabled = false;
		RaycastHit2D hitPositiveX = Physics2D.Raycast (transform.position, 
			new Vector2 (transform.position.x + power, transform.position.y), (1 << 10));

		RaycastHit2D hitNegativeX = Physics2D.Raycast (transform.position, 
			new Vector2 (transform.position.x - power, transform.position.y), (1 << 10));

		RaycastHit2D hitPositiveY = Physics2D.Raycast (transform.position, 
			new Vector2 (transform.position.x, transform.position.y + power), (1 << 10));

		RaycastHit2D hitNegativeY = Physics2D.Raycast (transform.position, 
			new Vector2 (transform.position.x, transform.position.y - power), (1 << 10));
		//GetComponent <BoxCollider2D> ().enabled = true;

		if (hitPositiveX != null) {
			Destroy (hitPositiveX.transform.gameObject);
		}
		if (hitNegativeX != null) {
			Destroy (hitNegativeX.transform.gameObject);
		}
		if (hitPositiveY != null) {
			Destroy (hitPositiveY.transform.gameObject);
		}
		if (hitNegativeY != null) {
			Destroy (hitNegativeY.transform.gameObject);
		}

		Debug.DrawLine (transform.position, new Vector3 (transform.position.x + power, transform.position.y), Color.green);
		Debug.DrawLine (transform.position, new Vector3 (transform.position.x - power, transform.position.y), Color.green);
		Debug.DrawLine (transform.position, new Vector3 (transform.position.x, transform.position.y + power), Color.green);
		Debug.DrawLine (transform.position, new Vector3 (transform.position.x, transform.position.y - power), Color.green);
/*
		Debug.Log ("The positive x hit is: " + hitPositiveX.transform.tag);
		Debug.Log ("The neagtive x hit is: " + hitNegativeX.transform.tag);
		Debug.Log ("The positive y hit is: " + hitPositiveY.transform.tag);
		Debug.Log ("The negative y hit is: " + hitNegativeY.transform.tag);
*/
		Destroy (gameObject, 2);
	}
}
