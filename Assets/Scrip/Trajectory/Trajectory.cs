using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField]
    private int numberOfDots;
    [SerializeField]
    private GameObject dotPrefab;
    [SerializeField] 
    private GameObject dotContainer;
    [SerializeField]
    private float dotSpacing;
    [SerializeField]
    private float dotMinScale;
    [SerializeField]
    private float dotMaxScale;
     
    private Transform[] dotsList;
    private Vector2 pos;
    private float timeStamp;


    private void Start()
    {
        Hide();
        PrepareDots();
    }

    private void PrepareDots()
    {
        dotsList = new Transform[numberOfDots];
        dotPrefab.transform.localScale = Vector2.one * dotMaxScale;

        float scale = dotMaxScale;

        float scaleFactor = scale / numberOfDots;

        for (int i = 0; i < numberOfDots; i++)
        {
            dotsList[i] = Instantiate(dotPrefab, dotContainer.transform).transform;
            dotsList[i].localScale = Vector2.one * scale;
            if (scale > dotMinScale)
            {
                scale -= scaleFactor;
            }
        }
    }

    public void UpdateDots(Vector3 ballPos, Vector2 forceApplied)
    {
        Show();
        timeStamp = dotSpacing;
        for (int i = 0; i < numberOfDots; i++)
        {
            pos.x = (ballPos.x + forceApplied.x * timeStamp);
            pos.y = (ballPos.y + forceApplied.y * timeStamp) - (Physics2D.gravity.magnitude * timeStamp * timeStamp) / 2f;
            dotsList[i].position = pos;
            timeStamp += dotSpacing;
        }
    }

    public void Show()
    {
        dotContainer.SetActive(true);
    }

    public void Hide()
    {
        dotContainer.SetActive(false);
    }
}
