using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace _Project.Scripts.Saves
{
    [Serializable]
    public class SimpleVector3
    {
        public float x;
        public float y;
        public float z;

        public SimpleVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
    [Serializable]
    public class GameObjectData
    {
        public SimpleVector3 position;
        public string suit;
        public int rank;
    }
    public class DataSaver : MonoBehaviour
    {
        // Coroutine for update leaderboard
        // private void Start()
        // {
        //     StartCoroutine(SaveLeaderBoard());
        // }

        // private void OnEnable()
        // {
        //     YandexGame.GetDataEvent += GetLeaderBoard;
        // }
        //
        // private void OnDisable()
        // {
        //     YandexGame.GetDataEvent -= GetLeaderBoard;
        // }

        private void GetLeaderBoard()
        {
            YandexGame.NewLeaderboardScores("ScoreLeaderBoard", YandexGame.savesData.highScore);
        }

        public static void SaveGameObjects(SavesYG saves)
        {
            YandexGame.savesData.gameObjectDataList = saves.gameObjectDataList;
            YandexGame.SaveProgress();
            // Debug.Log("Game objects saved: " + saves.gameObjectDataList.Count);
        }

        public static SavesYG LoadGameObjects()
        {
            var saves = new SavesYG();
            saves.gameObjectDataList = YandexGame.savesData.gameObjectDataList ?? new List<GameObjectData>();
            // Debug.Log("Game objects loaded: " + saves.gameObjectDataList.Count);
            return saves;
        }

        public static void ResetData()
        {
            YandexGame.savesData.gameObjectDataList.Clear();
            YandexGame.savesData.currentScore = 0;
            YandexGame.SaveProgress();
        }

        private IEnumerator SaveLeaderBoard()
        {
            yield return new WaitForSeconds(5f);
            YandexGame.NewLeaderboardScores("ScoreLeaderBoard", YandexGame.savesData.highScore);
        }
    }
    
    [Serializable]
    public class Serialization<T>
    {
        public List<T> target;
        public Serialization(List<T> target)
        {
            this.target = target;
        }
    }
}