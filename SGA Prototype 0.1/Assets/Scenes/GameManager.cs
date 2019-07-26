using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public enum GameState
	{
		Start, Game, End
	}
	private enum LevelState
	{
		Ready, Start, Play, End
	}
	public Level[] levels = new Level[0];
	public int currentLevel = 0;
	public GameState gameState = GameState.Game;

	private LevelState levelState;
	private static GameManager gameManager;
	private int collectedGarbage = 0;
	private Text scoreText;
	private Text timerText;
	private Text countdownText;
	private Text endText;
	private float time;
	private float countdownTime;
	private float countdownStartTime = 3;
	private int countdownMinSize = 1;
	private int countdownMaxSize = 150;
	private GameObject[] garbage;
	private GameObject startPanel;
	private GameObject uiPanel;
	private GameObject endPanel;
	private GameObject player;
	private Vector3 playerStartPosition;
	private bool won = false;
	private bool initLevel = false;
	private SoundManager soundManager;

	void Awake () {
		if (gameManager == null) {
			gameManager = this;
			DontDestroyOnLoad (gameObject);
			AudioSource audioSource = GetComponent<AudioSource> ();
			if (audioSource != null) {
				soundManager = new SoundManager (audioSource);
			}
		} else {
			Destroy (gameObject);
		}
		//just for playing the scene during debug
		if (gameState == GameState.Game) {
			currentLevel = 0;
			initLevel = true;
		}
	}

	void Update () {
		switch (gameState) {
		case GameState.Start:
			if (Input.GetButtonDown ("Jump")) {
				gameState = GameState.Game;
				currentLevel = 0;
				initLevel = true;
				PlaySFX (SoundManager.BLEURGH);
				SceneManager.LoadScene (levels[currentLevel].name);
			}
			break;
		case GameState.Game:
			UpdateLevel ();
			break;
		case GameState.End:
			if (Input.GetButtonDown ("Jump")) {
				gameState = GameState.Start;
				PlaySFX (SoundManager.GARBAGE);
				SceneManager.LoadScene ("start_screen");
			}
			break;
		}
	}

	private void InitLevel () {
		if (gameState != GameState.Game)
			return;
		GameObject scoreGameObject = GameObject.Find ("ScoreText");
		if (scoreGameObject != null)
			scoreText = scoreGameObject.GetComponent<Text> (); 
		GameObject timerGameObject = GameObject.Find ("TimerText");
		if (timerGameObject != null)
			timerText = timerGameObject.GetComponent<Text> ();  
		uiPanel = GameObject.Find ("UIPanel");
		startPanel = GameObject.Find ("StartPanel");
		GameObject countdownTextGameObject = GameObject.Find ("CountdownText");
		if (countdownTextGameObject != null)
			countdownText = countdownTextGameObject.GetComponent<Text> ();
		endPanel = GameObject.Find ("EndPanel");
		if (endPanel != null)
			endText = GameObject.Find ("EndText").GetComponent<Text> ();
		player = GameObject.Find ("Player");
		if (player != null)
			playerStartPosition = player.transform.position;
		else
			Debug.LogError ("Object with 'Player' name not found");
		garbage = GameObject.FindGameObjectsWithTag ("Garbage");
		InitLevelParameters ();
	}

	public void InitLevelParameters(){
		collectedGarbage = 0;
		if (scoreText != null) {
			scoreText.text = "" + collectedGarbage + "/" + levels[currentLevel].scoreToWin;
		}
		time = levels[currentLevel].timeInSeconds;
		levelState = LevelState.Ready;
		if (uiPanel != null)
			uiPanel.SetActive (false);
		if (startPanel != null)
			startPanel.SetActive (true);
		if (countdownText != null)
			countdownText.gameObject.SetActive (false);
		if (endPanel != null)
			endPanel.SetActive (false);
		if (player != null) {
			player.transform.position = playerStartPosition;
			player.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
		}
		for (int g = 0; g < garbage.Length; g++) {
			garbage [g].SetActive (true);
		}
	}

	public bool CanPlay(){
		return gameState == GameState.Game && levelState == LevelState.Play;
	}

	private void UpdateLevel(){
		if (initLevel) {
			InitLevel ();
			initLevel = false;
		}
		switch (levelState) {
		case LevelState.Ready:
			if (Input.GetButtonDown ("Jump")) {
				levelState = LevelState.Start;
				startPanel.SetActive (false);
				countdownText.gameObject.SetActive (true);
				countdownTime = countdownStartTime;
			}
			break;
		case LevelState.Start:
			UpdateCountDown ();
			break;
		case LevelState.Play:
			time -= Time.deltaTime;
			if (time <= 0) {
				Lose ();
			}else{
				int minutes = (int)(time / 60);
				int seconds = (int)(time - minutes * 60);
				timerText.text = minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
			}
			break;
		case LevelState.End:
			if (Input.GetButtonDown ("Jump")) {
				PlaySFX (SoundManager.BLEURGH);
				if (won) {
					initLevel = true;
					currentLevel++;
					if (currentLevel < levels.Length) {
						initLevel = true;
						SceneManager.LoadScene (levels[currentLevel].name);
					} else {
						gameState = GameState.End;
						SceneManager.LoadScene ("end_screen");
					}
				} else {
					InitLevelParameters ();
				}
			}
			break;
		}
	}

	public void IncrementAmountOfGarbage() {
		collectedGarbage++;
		scoreText.text = "" + collectedGarbage + "/" + levels[currentLevel].scoreToWin;
		if (collectedGarbage == levels[currentLevel].scoreToWin) {
			Win ();
		}
	}

	private void Win () {
		endText.text = "You won!\n\n<size=40>press jump button to continue</size>";
		endPanel.SetActive (true);
		levelState = LevelState.End;
		uiPanel.SetActive (false);
		won = true;
		PlaySFX (SoundManager.WIN);
	}
		
	private void Lose () {
		endText.text = "You lose!\n\n<size=40>press jump button to restart</size>";
		endPanel.SetActive (true);
		levelState = LevelState.End;
		uiPanel.SetActive (false);
		won = false;
		PlaySFX (SoundManager.LOSE);
	}

	private void UpdateCountDown(){
		countdownTime -= Time.deltaTime;
		float fontSize = Mathf.Lerp (countdownMinSize, countdownMaxSize, 1 - (countdownTime - (int)countdownTime));
		countdownText.fontSize = (int)Mathf.Round(fontSize);
		string previousText = countdownText.text;
		if (countdownTime < -1) {
			levelState = LevelState.Play;
			countdownText.gameObject.SetActive (false);
			uiPanel.SetActive (true);
		}else if (countdownTime < 0) {
			countdownText.text = "GO!";
			countdownText.fontSize = countdownMaxSize;
		} else {
			countdownText.text = "" + (int)(countdownTime + 1);
		}
		if (!countdownText.text.Equals (previousText)) {
			if (countdownTime < 0)
				PlaySFX (SoundManager.COUNTDOWN_GO);
			else
				PlaySFX (SoundManager.COUNTDOWN);
		}
	}

	public void PlaySFX(int soundEffect){
		if (soundManager != null)
			soundManager.PlaySFX (soundEffect);
	}

	public void StopSFX(){
		if (soundManager != null)
			soundManager.StopSFX ();
	}
}
