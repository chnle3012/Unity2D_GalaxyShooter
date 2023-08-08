using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Section3
{
    public class GamePlayPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TxtScore;
        [SerializeField] private Image m_ImgBar;

        private void OnEnable()
        {
            GameManager.instance.onScoreChanged += OnScoreChanged;
            SpawnManager.instance.Player.onHPChanged += OnHPChanged;
        }

        private void OnDisable()
        {
            GameManager.instance.onScoreChanged -= OnScoreChanged;
            SpawnManager.instance.Player.onHPChanged -= OnHPChanged;
        }
        public void BtnPause_Pressed()
        {
            GameManager.instance.Pause();
        }
 
        private void OnScoreChanged(int score)
        {
            m_TxtScore.text = "SCORE : " + score;
        }

        private void OnHPChanged(int currentHp, int maxHp)
        {
            m_ImgBar.fillAmount = currentHp * 1f / maxHp;
        }
    }
}