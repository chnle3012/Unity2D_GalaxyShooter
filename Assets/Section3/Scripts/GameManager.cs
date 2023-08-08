using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Section3
{
    public enum GameState
    {
        Home,
        GamePlay,
        Pause,
        GameOver
    }
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_Instance;
        public static GameManager instance
        {
            get
            {
                if(m_Instance == null)
                    m_Instance = FindObjectOfType<GameManager>();
                return m_Instance;
            }
        }

        public Action<int> onScoreChanged;

        [SerializeField] private HomePanel m_HomePanel;
        [SerializeField] private GamePlayPanel m_GamePlayPanel;
        [SerializeField] private PausePanel m_PausePanel;
        [SerializeField] private GameOverPanel m_GameOverPanel;
        [SerializeField] private WaveData[] m_Wave;

        // private AudioManager m_AudioManager;
        // private SpawnManager m_SpawnManager;
        private GameState m_GameState;
        private bool m_Win;
        private int m_score;
        private int m_curWaveIndex;
        // Start is called before the first frame update

        private void Awake()
        {
            if(m_Instance == null)
                m_Instance = this;
            else if(m_Instance != this)
                Destroy(gameObject);
        }

        void Start()
        {
            // m_AudioManager = FindObjectOfType<AudioManager>();
            // m_SpawnManager = FindObjectOfType<SpawnManager>();
            m_HomePanel.gameObject.SetActive(false);
            m_GamePlayPanel.gameObject.SetActive(false);
            m_PausePanel.gameObject.SetActive(false);
            m_GameOverPanel.gameObject.SetActive(false);
            SetState(GameState.Home);
        }

        // Update is called once per frame
        

        private void SetState(GameState state)
        {
            m_GameState = state;
            m_HomePanel.gameObject.SetActive(m_GameState == GameState.Home);
            m_GamePlayPanel.gameObject.SetActive(m_GameState == GameState.GamePlay);
            m_PausePanel.gameObject.SetActive(m_GameState == GameState.Pause);
            m_GameOverPanel.gameObject.SetActive(m_GameState == GameState.GameOver);

            if(m_GameState == GameState.Pause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }

            if (m_GameState == GameState.Home)
                AudioManager.Instance.PlayHomeMusic();
            else
                AudioManager.Instance.PlayBattleMusic();
        }

        public bool inactive()
        {
            return m_GameState == GameState.GamePlay;
        }

        public void Play()
        {
            m_curWaveIndex = 0;
            WaveData wave = m_Wave[m_curWaveIndex];
            SpawnManager.instance.StartBattle(wave,true); 
            SetState(GameState.GamePlay);
            m_score = 0;
            if (onScoreChanged != null)
                onScoreChanged(m_score);
        }

        public void Pause()
        {
            SetState(GameState.Pause);
        }

        public void Home()
        {
            SetState(GameState.Home);
            SpawnManager.instance.Clear();
        }

        public void Continue()
        {
            SetState(GameState.GamePlay);
        }

        public void GameOver(bool win)
        {
            int curHighScore = PlayerPrefs.GetInt("HighScore");
            if (curHighScore < m_score)
            {
                PlayerPrefs.SetInt("HighScore", m_score);
                curHighScore = m_score;
            }
            m_Win = win;
            SetState(GameState.GameOver);
            m_GameOverPanel.DisplayResult(m_Win);
            m_GameOverPanel.DisplayHighScore(curHighScore);
        }
        
        public void Addscore(int value)
        {
            m_score += value;

            if (onScoreChanged != null)
                onScoreChanged(m_score);

            if (SpawnManager.instance.isclear())
            {
                m_curWaveIndex++;
                if (m_curWaveIndex >= m_Wave.Length)
                    GameOver(true);
                else
                {
                    WaveData wave = m_Wave[m_curWaveIndex];
                    SpawnManager.instance.StartBattle(wave,false);
                }
            }
                
        }
    }
}
