using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    private const string MENU = "MainMenu";
    private const string GAME = "GameScene";
    private const string LEVEL_CHOOSE = "Levels";

    public void ChangeToMenu()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeScene(MENU));
    }

    public void ChangeToGameScene()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeScene(GAME));
    }

    public void ChangeToLevels()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeScene(LEVEL_CHOOSE));
    }

    private IEnumerator ChangeScene(string sceneName)
    {
        //Optional: Add animation here

        yield return new WaitForSecondsRealtime(.4f);
        SceneManager.LoadScene(sceneName);
    }
}
