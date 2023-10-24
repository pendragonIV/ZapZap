using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsScene : MonoBehaviour
{
    [SerializeField]
    private Transform levelHolderPrefab;
    [SerializeField]
    private Transform levelsContainer;
    
    private const int MAX_STARS = 3;    
    void Start()
    {
        PrepareLevels();    
    }

    private void PrepareLevels()
    {
        for(int i = 0; i < LevelManager.instance.levelData.GetLevels().Count; i ++)
        {
            Transform holder = Instantiate(levelHolderPrefab, levelsContainer);
            holder.name = i.ToString();
            SetUpLevelIn4(holder, LevelManager.instance.levelData.GetLevelAt(i));
        }
    }

    private void SetUpLevelIn4(Transform levelHolder, Level levelData)
    {
        Transform statusPanel = levelHolder.GetChild(0);
        Transform playBtn = statusPanel.GetChild(0);
        Transform lockPanel = statusPanel.GetChild(1);
        Transform achivementPanel = statusPanel.GetChild(2);

        SetHolderImg(levelHolder, lockPanel, levelData);

        SetLevelStatus(lockPanel, playBtn, levelData);

        SetLevelAchivement(achivementPanel, levelData);

    }

    private void SetLevelAchivement(Transform achivementPanel, Level levelData)
    {
        for (int i = 0; i < MAX_STARS; i++)
        {
            Transform star = achivementPanel.GetChild(i);
            Transform starInactive = star.GetChild(0);
            Transform starActive = star.GetChild(1);
            if (i < levelData.achivement)
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

    private void SetHolderImg(Transform levelHolder, Transform lockPanel,Level levelData)
    {
        levelHolder.GetComponent<Image>().sprite = levelData.mapPreviewImg;
        lockPanel.GetComponent<Image>().sprite = levelData.mapPreviewImg;
        
    }

    private void SetLevelStatus(Transform lockPanel, Transform playBtn, Level levelData)
    {
        if (levelData.isPlayable)
        {
            lockPanel.gameObject.SetActive(false);
            playBtn.gameObject.SetActive(true);
        }
        else
        {
            playBtn.gameObject.SetActive(false);
            lockPanel.gameObject.SetActive(true);
        }
    }
}
