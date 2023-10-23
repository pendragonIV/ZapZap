using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private List<Level> levels = new List<Level>();

    public Level GetLevelAt(int index)
    {
        return levels[index];
    }
}

[System.Serializable]
public class Level
{
    public GameObject map;
    public float leftPosX;
    public float rightPosX;
    public bool isCompleted;
    public bool isPlayable;
    public int achivement;
}
