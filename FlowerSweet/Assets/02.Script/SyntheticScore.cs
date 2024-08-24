using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SyntheticScore : MonoBehaviour
{
    public static SyntheticScore Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] RectTransform star1,star2,star3,star1AT,star2AT,star3AT;
    [SerializeField] private TMP_Text scoreText, levelText, coinsText;

    public void Synthetic()
    {
        scoreText.text = UserData.data.ScoreInCache.ToString();
        levelText.text = UserData.data.LevelInCache.ToString();
        coinsText.text = UserData.data.CoinsInCache.ToString();
    }
}
