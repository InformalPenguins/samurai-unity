using UnityEngine;
using System.Collections;

public class SamuraiScript : MonoBehaviour {

	private const int RIGHT = 1;
	private const int LEFT = 2;

	private int direction = SamuraiScript.RIGHT;

	public float jumpDuration = 0.3f;
	public float jumpDistance = 6;

	private bool jumping = false;
	private bool attacking = false;

	private float jumpStartVelocityY;

	private Animator animator;
	private SpriteRenderer spriteRenderer; 
	private Sprite spriteStand;
	private Sprite spriteJump;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();

		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteStand = spriteRenderer.sprite;
		spriteJump = Resources.Load<Sprite>("Samurai_jump1");

		spriteRenderer.sprite = spriteStand;

		jumpStartVelocityY = -jumpDuration * Physics.gravity.y / 2;
	}
	
	// Update is called once per frame
	void Update () {
		if (jumping)
		{
			return;
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if (direction != SamuraiScript.LEFT) {
				direction = SamuraiScript.LEFT;
				Vector2 forwardAndLeft = (transform.forward - transform.right) * jumpDistance;
				StartCoroutine(Jump(forwardAndLeft));
			} else {
				StartCoroutine (Attack ((float)0.2));
			}
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (direction != SamuraiScript.RIGHT) {
				direction = SamuraiScript.RIGHT;
				Vector2 forwardAndRight = (transform.forward + transform.right) * jumpDistance;
				StartCoroutine (Jump (forwardAndRight));
			} else {
				StartCoroutine (Attack ((float)0.2));
			}
		}
		else if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine (Attack ((float)0.2));
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(attacking && other.gameObject.tag == "Enemy") {
			NinjaScript ninja = (NinjaScript)other.gameObject.GetComponent<NinjaScript>();
			ninja.die ();
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if(attacking && other.gameObject.tag == "Enemy") {
			NinjaScript ninja = (NinjaScript)other.gameObject.GetComponent<NinjaScript>();
			ninja.die ();
		}
	}

	private IEnumerator Jump(Vector2 direction)
	{
		jumping = true;
		attacking = true;
		Vector2 startPoint = transform.position;
		Vector2 targetPoint = startPoint + direction;
		float time = 0;
		float angle = 0;
		float jumpProgress = 0;
		float velocityY = jumpStartVelocityY;
		float height = startPoint.y;

		animator.SetBool ("samuraiJumping", true);

		while (jumping)
		{
			jumpProgress = time / jumpDuration;
			angle = jumpProgress * Mathf.Rad2Deg * 10;

			if (jumpProgress > 1)
			{
				jumping = false;
				attacking = false;
				jumpProgress = 1;
				angle = 0;

				this.Stand ();
			}

			Vector2 currentPos = Vector2.Lerp(startPoint, targetPoint, jumpProgress);
			currentPos.y = height * 30;
			transform.position = currentPos;

			//Wait until next frame.
			yield return null;

			height += velocityY * Time.deltaTime;
			velocityY += Time.deltaTime * Physics.gravity.y;
			time += Time.deltaTime;
		}

		transform.position = targetPoint;
		yield break;
	}

	private IEnumerator Attack(float duration)
	{
		attacking = true;
		animator.SetTrigger ("samuraiChop");
		yield return new WaitForSeconds(duration);
		attacking = false;
	}

	private void Stand() {
		animator.SetBool ("samuraiJumping", false);
		Vector3 localScale = transform.localScale;
		localScale.x *= -1;
		transform.localScale = localScale;
	}
}
