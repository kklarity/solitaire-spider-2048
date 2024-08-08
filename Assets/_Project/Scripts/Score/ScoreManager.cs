using System;
using TMPro;
using UnityEngine;
using YG;

namespace _Project.Scripts.Score
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance;

        private int comboCount = 0;
        private float comboTimer = 0f;
        private float comboResetTime = 2f; // Время в секундах для сброса комбо
        private int _score = 0;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _highScoreText;
        
        [SerializeField] private AudioSource _boop;
        [SerializeField] private ParticleSystem _particleSystem;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _highScoreText.text = YandexGame.savesData.highScore.ToString();
            _score = YandexGame.savesData.currentScore;
            _scoreText.text = YandexGame.savesData.currentScore.ToString();
        }

        private void Update()
        {
            if (comboTimer > 0)
            {
                comboTimer -= Time.deltaTime;
                if (comboTimer <= 0)
                {
                    comboCount = 0;
                }
            }
        }

        public void AddCombo()
        {
            comboCount++;
            comboTimer = comboResetTime;
        }

        public void AddPoints(int basePoints)
        {
            _boop.Play();
            var points = basePoints * comboCount;
            
            _score += points;
            _scoreText.text = _score.ToString();
            YandexGame.savesData.currentScore = _score;
    
            if (_score > YandexGame.savesData.highScore)
            {
                YandexGame.savesData.highScore = _score;
                _highScoreText.text = _score.ToString();
            }
            
            YandexGame.SaveProgress();
        }

        public void Add10K(int basePoints)
        {
            _boop.Play();
            var points = basePoints * comboCount;
            
            _score += points;
            _scoreText.text = _score.ToString();
            YandexGame.savesData.currentScore = _score;
            _particleSystem.Play();
    
            if (_score > YandexGame.savesData.highScore)
            {
                YandexGame.savesData.highScore = _score;
                _highScoreText.text = _score.ToString();
            }
            
            YandexGame.SaveProgress();
        }


    }
}