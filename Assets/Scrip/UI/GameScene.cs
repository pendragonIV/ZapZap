using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform ingameMenu; 
    [SerializeField]
    private Transform achivementContainer;
    [SerializeField]
    private Transform endgameAchivementContainer;
    [SerializeField]
    private Transform pauseWindow;
    [SerializeField]
    private Transform winPanel;
    [SerializeField]
    private Transform losePanel;
    [SerializeField]
    private Transform sceneTransition;

    [SerializeField]
    private Text arrowLeft;

    private const int MAX_ACHIVEMENT = 3;

    public void SetArrow(int arrows)
    {
        arrowLeft.text = arrows.ToString();
    }

    public void SetIngameAchivement(int achivement)
    {
        for(int i = 0; i < MAX_ACHIVEMENT; i++)
        {
            Transform star = achivementContainer.GetChild(i);
            Transform starInactive = star.GetChild(0);
            Transform starActive = star.GetChild(1);
            if (i < achivement)
            {
                starInactive.gameObject.SetActive(false);
                starActive.gameObject.SetActive(true);
            }
            else
            {
                starActive.gameObject.SetActive(false);
                starInactive.gameObject.SetActive(true);
            }
        }
    }

    public void SetEndgameAchivement(int achivement)
    {
        for (int i = 0; i < MAX_ACHIVEMENT; i++)
        {
            Transform star = endgameAchivementContainer.GetChild(i);
            Transform starInactive = star.GetChild(0);
            Transform starActive = star.GetChild(1);
            if (i < achivement)
            {
                starInactive.gameObject.SetActive(false);
                starActive.gameObject.SetActive(true);
            }
            else
            {
                starActive.gameObject.SetActive(false);
                starInactive.gameObject.SetActive(true);
            }
        }
    }

    private void ShowIngameMenu()
    {
        if (ingameMenu != null)
        {
            ingameMenu.gameObject.SetActive(true);
        }
    }

    private void HideIngameMenu()
    {
        if (ingameMenu != null)
        {
            ingameMenu.gameObject.SetActive(false);
        }
    }

    public void ShowPauseWindow()
    {
        if (pauseWindow != null)
        {
            ShowIngameMenu();
            FadeIn(ingameMenu.GetComponent<CanvasGroup>(), pauseWindow.GetComponent<RectTransform>());
            GameManager.instance.PauseGame();
            pauseWindow.gameObject.SetActive(true);
        }
    }

    public void HidePauseWindow()
    {
        if(pauseWindow != null)
        {
            StartCoroutine(FadeOut(ingameMenu.GetComponent<CanvasGroup>(), pauseWindow.GetComponent<RectTransform>()));
        }
    }

    public void ShowWinPanel()
    {
        if(winPanel != null)
        {
            ShowIngameMenu();
            ShowAchivementContainer();
            winPanel.gameObject.SetActive(true);
            FadeIn(ingameMenu.GetComponent<CanvasGroup>(), winPanel.GetComponent<RectTransform>());
        }
    }

    public void HideWinPanel()
    {
        if(winPanel != null)
        {
            winPanel.gameObject.SetActive(false);
            HideIngameMenu();
            HideAchivementContainer();
        }
    }

    public void ShowLosePanel()
    {
        if(losePanel != null)
        {
            ShowIngameMenu();
            ShowAchivementContainer();
            losePanel.gameObject.SetActive(true);
            FadeIn(ingameMenu.GetComponent<CanvasGroup>(), losePanel.GetComponent<RectTransform>());
        }
    }

    public void HideLosePanel()
    {
        if(losePanel != null)
        {
            losePanel.gameObject.SetActive(false);
            HideIngameMenu();
            HideAchivementContainer();
        }
    }

    public void ShowAchivementContainer()
    {
        if(endgameAchivementContainer != null)
        {
            endgameAchivementContainer.gameObject.SetActive(true);
        }
    }

    private void HideAchivementContainer()
    {
        if(endgameAchivementContainer != null)
        {
            endgameAchivementContainer.gameObject.SetActive(false);
        }
    }


    private void FadeIn(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        rectTransform.transform.localPosition = new Vector3(0, -1000, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 0), 0.2f, false).SetEase(Ease.OutElastic);
        canvasGroup.DOFade(1, .5f);
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 1f;
        rectTransform.transform.localPosition = new Vector3(0, 0, 0);
        rectTransform.DOAnchorPos(new Vector2(0, -1000), 0.2f, false).SetEase(Ease.InOutQuint);
        canvasGroup.DOFade(0, .5f);
        yield return new WaitForSeconds(1f);
        pauseWindow.gameObject.SetActive(false);
        HideIngameMenu();
        GameManager.instance.ResumeGame();
    }
}
