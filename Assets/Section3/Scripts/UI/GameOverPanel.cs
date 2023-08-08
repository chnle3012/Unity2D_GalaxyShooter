using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Section3
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TxtResult;
        [SerializeField] private TextMeshProUGUI m_TxtHighScore;
        private GameManager m_GameManager;
        // Start is called before the first frame update
        void Start()
        {
            m_GameManager = FindObjectOfType<GameManager>();
        }

        public void BtnHome_Pressed()
        {
            m_GameManager.Home();
        }

        public void DisplayHighScore(int score)
        {
            m_TxtHighScore.text = "HIGHSCORE : " + score;
        }

        public void DisplayResult(bool iswin)
        {
            if (iswin)
                m_TxtResult.text = "YOU WIN";
            else
            {
                m_TxtResult.text = "YOU LOSE";
            }
        }

    }
}