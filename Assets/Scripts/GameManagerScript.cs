using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	public int level = 1;
	public int lives = 3;

	private int counter = 0;
	private Text textPoints;
	private Text textLevel;
	private int totalEnemies = 0;
	private int points = 0;
	private float spawnTime = 0.5f;

	public GameObject enemyPrefab;

	// Use this for initialization
	void Start () {
		textPoints = GameObject.Find("TextPoints").GetComponent<Text>();
		textLevel = GameObject.Find("TextLevel").GetComponent<Text>();

		InvokeRepeating ("createEnemy", spawnTime, spawnTime);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void createEnemy() {
		counter = (counter + 1) % 10;
		if (counter > level) {
			return;
		}

		int startSide = Random.Range (0, 2);

		int startX = 10;
		if (startSide == 1) {
			startX = -10;
		}

		Instantiate(enemyPrefab, new Vector3 (startX, 0, 0), Quaternion.identity);

		points++;
		textPoints.text = "Ninjas: " + points;

		totalEnemies++;
		if (totalEnemies == level * 10) {
			totalEnemies = 0;
			level++;
			textLevel.text = "Level " + level;
		}
	}

	public void removeLive() {
		if(lives == 3) {
			Destroy(GameObject.Find("Hearth_3"));
		}
		if(lives == 2) {
			Destroy(GameObject.Find("Hearth_2"));
		}
		if(lives == 1) {
			Destroy(GameObject.Find("Hearth_1"));
		}
		lives--;

		if (lives <= 0) {
			SceneManager.LoadScene ("GameOver");
		}
	}
}
