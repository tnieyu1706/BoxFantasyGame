using System;
using System.IO;
// change follow Charon setting - module.
using Systems.Charon;
//
using EditorAttributes;
using TnieYuPackage.CustomAttributes;
using TnieYuPackage.DesignPatterns.Patterns.Singleton;
using UnityEngine;
using Void = EditorAttributes.Void;

namespace TnieYuPackage.CharonSupport
{
    /// <summary>
    /// CharonGlobalData can use in Editor & Runtime
    /// *Note: typeof(GameData) & using Charon can be change depend on Charon setting.
    /// </summary>
    [CreateAssetMenu(fileName = "CharonGlobalGameData", menuName = "TnieYuPackage/CharonSupport/CharonGlobalGameData")]
    public class CharonGlobalGameData : SingletonScriptable<CharonGlobalGameData>
    {
        #region Properties

        [TniePath(typeof(TextAsset))] public string gameDataPath;

        public Formatters.GameDataFormat gameDataFormat = Formatters.GameDataFormat.Json;

        [Space(20)] public Void spacing;

        #endregion

        private GameData gameData;

        public GameData GameData
        {
            get
            {
                if (gameData == null)
                {
                    LoadGameData();
                }

                return gameData;
            }
        }

        private Formatters.GameDataLoadOptions gameDataLoadOptions;

        private Formatters.GameDataLoadOptions GetGameDataLoadOptions()
        {
            if (gameDataLoadOptions == null)
            {
                gameDataLoadOptions = new Formatters.GameDataLoadOptions
                {
                    Format = gameDataFormat
                };
            }

            return gameDataLoadOptions;
        }

        [Button("Load Game Data Manual")]
        public void LoadGameData()
        {
            if (string.IsNullOrEmpty(gameDataPath))
            {
                Debug.LogWarning("No game data path provided.");
                return;
            }

            try
            {
                using (var fileStream = File.OpenRead(gameDataPath))
                {
                    gameData = new GameData(fileStream, GetGameDataLoadOptions());
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
        }
    }
}