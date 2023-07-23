using UnityEngine;

public class GameManager : HappyTools.SingletonBehaviour<GameManager>
{
        public bool IsGamePause = false;
        private int _gold;
        public int MaxGold = 20;
        private void Start()
        {
                Init();
        }
        public void Init()
        {
                Debug.Log("GameManager - Init Game");
                _gold = 0;
        }
        public void PauseGame()
        {
                IsGamePause = true;

        SoundManager.Instance.PlayCommonSFXPitched("Click");
        Time.timeScale = 0f;
        }

        public void ResumeGame()
    {
        SoundManager.Instance.PlayCommonSFXPitched("Click");
        IsGamePause = false;
        Time.timeScale = 1f;
    }

        public void AddGold(int amount)
        {
                _gold += amount;
        SoundManager.Instance.PlayCommonSFXPitched("CoinGain");
        }

        public int GetGold()
        {
                return _gold;
        }
}