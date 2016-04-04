using UnityEngine;
using System.Collections;

public class NinjaScript : MonoBehaviour {

	public int velocity;

	private Animator animator;
	private GameManagerScript game;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		game = (GameManagerScript)GameObject.Find("GameManager").GetComponent<GameManagerScript>();

		int level = game.level;
		velocity = Random.Range (level, level + 5);

		float currentX = transform.position.x;
		if (currentX < 0) {
			Vector3 localScale = transform.localScale;
			localScale.x *= -1;
			transform.localScale = localScale;
		}
	}
	
	// Update is called once per frame
	void Update () {
		float currentX = transform.position.x;
		if (currentX > 0) {
			transform.Translate(Vector3.left * Time.deltaTime * velocity);
		}
		else if (currentX < 0) {
			transform.Translate(Vector3.right * Time.deltaTime * velocity);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.name == "Home") {
			game.removeLive ();
			Destroy (gameObject);
		}
	}

	public void die() {
		velocity = 0;
		StartCoroutine (KillOnAnimationEnd());
	}

	private IEnumerator KillOnAnimationEnd() {
		animator.SetTrigger ("enemyDies");
		yield return new WaitForSeconds (0.167f);
		Destroy (gameObject);
	}
}
