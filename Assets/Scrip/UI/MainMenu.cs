using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Transform guidePanel;
    [SerializeField]
    private Transform guideLinePanel;

    private void Awake()
    {
        guidePanel.gameObject.SetActive(false); 
    }

    public void OpenGuide()
    {
        guidePanel.gameObject.SetActive(true);
        FadeIn(guidePanel.GetComponent<CanvasGroup>(), guideLinePanel.GetComponent<RectTransform>());
    }

    public void CloseGuide()
    {
        FadeOut(guidePanel.GetComponent<CanvasGroup>(), guideLinePanel.GetComponent<RectTransform>());
        StartCoroutine(FadeOut(guidePanel.GetComponent<CanvasGroup>(), guideLinePanel.GetComponent<RectTransform>()));
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
        yield return new WaitForSeconds(0.5f);
        guidePanel.gameObject.SetActive(false);
    }
}
