using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    private const string MENU = "MainMenu";
    private const string GAME = "GameScene";
    private const string LEVEL_CHOOSE = "Levels";

    [SerializeField]
    private Transform sceneTransition;

    private void Start()
    {
        sceneTransition.GetComponent<Animator>().Play("SceneTransition");
    }

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
        sceneTransition.GetComponent<Animator>().Play("SceneTransitionReverse");
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(sceneName);
    }
}
