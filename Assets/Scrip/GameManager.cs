using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if(instance != this && instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public GameObject player;
    public GameObject enemiesContainer;
    public GameScene gameScene;
    public List<GameObject> leftEnemies = new List<GameObject>();
    public List<GameObject> rightEnemies = new List<GameObject>();

    private bool isGameEnded = false;
    [SerializeField]
    private bool isGamePaused = false;
    [SerializeField]
    private GameObject lastArrow;
    private int arrows;

    private void Start()
    {
        Level currentLevelData = LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex);
        GameObject mapInit = Instantiate(currentLevelData.map);
        enemiesContainer = mapInit.transform.GetChild(0).gameObject;
        arrows = enemiesContainer.transform.childCount + 4;
        SetGameStatus();

        foreach (Transform enemy in enemiesContainer.transform)
        {
            if(enemy.position.x < LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex).leftPosX)
            {
                leftEnemies.Add(enemy.gameObject);
            }
            else
            {
                rightEnemies.Add(enemy.gameObject);
            }
        }

        Time.timeScale = 1;
    }

    private void Update()
    {


        if (leftEnemies.Count <= 0 && rightEnemies.Count <= 0)
        {
            if (!isGameEnded)
            {
                Win();
            }
        }

    }

    public void SetLastArrow(GameObject arrow)
    {
        lastArrow = arrow;
    }

    public GameObject GetLastArrow()
    {
        return lastArrow;
    }

    public void PauseGame()
    {
        isGamePaused = true;
    }

    public void ResumeGame()
    {
        isGamePaused = false;
    }

    public bool IsGamePause()
    {
        return isGamePaused;
    }

    public void DecreseArrows()
    {
        arrows--;
        SetGameStatus();
    }

    public int GetArrows()
    {
        return arrows;
    }   

    private void SetGameStatus()
    {
        if (!isGameEnded)
        {
            gameScene.SetArrow(arrows);
            gameScene.SetEndgameAchivement(arrows);
            gameScene.SetIngameAchivement(arrows);
        }
    }

    public void Lose()
    {
        gameScene.ShowLosePanel();
        isGameEnded = true;
    }
    
    public void CompleteLevel()
    {
        isGameEnded = true;
    }

    public void Win()
    {
        gameScene.ShowWinPanel();
        SetCompleteLevel();
        SetUpNextLevel();
        LevelManager.instance.levelData.SaveDataJSON();
        isGameEnded = true;
    }

    private void SetUpNextLevel()
    {
        if (LevelManager.instance.currentLevelIndex < LevelManager.instance.levelData.GetLevels().Count - 1)
        {
            if(!LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex + 1).isPlayable)
            {
                LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex + 1, true, false, 0);
            }
        }
    }

    private void SetCompleteLevel()
    {
        LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex, true, true, arrows >= 3 ? 3 : arrows);
    }
}
