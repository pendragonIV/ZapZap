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
    public List<GameObject> leftEnemies = new List<GameObject>();
    public List<GameObject> rightEnemies = new List<GameObject>();

    private void Start()
    {
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
    }
}
