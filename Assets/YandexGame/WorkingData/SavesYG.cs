
using System.Collections.Generic;
using _Project.Scripts.Saves;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        public int highScore = 0;
        public int currentScore = 0;
        public List<GameObjectData> gameObjectDataList = new();
    }
}
