using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelHolder : MonoBehaviour
{
    private const string GAME = "GameScene";

    public void ChooseLevel()
    {
        int levelIndex = int.Parse(this.name);
        LevelManager.instance.currentLevelIndex = levelIndex;
        StopAllCoroutines();
        StartCoroutine(ChangeScene(GAME));
    }

    private IEnumerator ChangeScene(string sceneName)
    {
        //Optional: Add animation here
        LevelsScene.instance.sceneTransition.GetComponent<Animator>().Play("SceneTransitionReverse");
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(sceneName);
    }
}
