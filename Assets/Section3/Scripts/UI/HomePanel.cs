using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Section3
{
    public class HomePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TxtHighScore;

        private void OnEnable()
        {
            m_TxtHighScore.text = "HIGH SCORE : " + PlayerPrefs.GetInt("HighScore");
        }
        void Start()
        {
            // m_GameManager = FindObjectOfType<GameManager>();
        }

        // Update is called once per frame
        public void BtnPlay_Pressed()
        {
            GameManager.instance.Play();
        }
    }
}