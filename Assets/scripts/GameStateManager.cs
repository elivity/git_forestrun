using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour {

	// Game States
	// for now we are only using these two
	public enum GameState { INTRO, MAIN_MENU }
	public delegate void OnStateChangeHandler();

	private static GameStateManager instance;
	public event OnStateChangeHandler OnStateChange;
	public  GameState gameState { get; private set; }

	private static string activeLevel;
	private static int highScore = 0;
    private static int lifes = 3;
	private static float speed = 0;
	private static bool cankick = true;
	private static GameObject teddyToKick;
	private static int coinsCollected = 0;
	private static int distanceRun = 0000;
	/*private GameStateManager() {
		// initialize your game manager here. Do not reference to GameObjects here (i.e. GameObject.Find etc.)
		// because the game manager will be created before the objects
	}  */  

	public static GameStateManager Instance {
		get {
			if(instance==null) {
				DontDestroyOnLoad(GameStateManager.instance);
				instance = new GameStateManager();
			}

			return instance;
		}
	}

	public void SetGameState(GameState state){
		this.gameState = state;
		OnStateChange();
	}


	public void restartLevel() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		speed = 2;
	}

	// Add your game mananger members here
	public void Pause(bool paused) {
		
	}

	public int getDistance() {
		return distanceRun;
	}

	public void setDistance(int dist) {
		distanceRun = dist;
	}

	public void collectCoin() {
		coinsCollected++;
	}

    public int getCollectedCoins()
    {
        return coinsCollected;
    }

	public float getSpeed() {
		return speed;
	}
	public void setSpeed(float gotSpeed) {
		speed = gotSpeed;
	}

	public string getLevel() { return activeLevel; }

	public void addScore(int points) {
		highScore = points;
	}

	public int getPoints() {
		return highScore;
	}

    public void deleteOneLife()
    {
            lifes--;
		Debug.Log ("hit");
    }

    public int getLifes()
    {
        return lifes;
    }


}
