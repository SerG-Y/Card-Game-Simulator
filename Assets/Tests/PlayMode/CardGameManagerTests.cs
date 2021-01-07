﻿using System.Collections;
using System.IO;
using CardGameDef.Unity;
using Cgs;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class CardGameManagerTests
    {
        private CardGameManager _manager;

        [SetUp]
        public void Setup()
        {
            var manager = new GameObject();
            manager.AddComponent<EventSystem>();
            _manager = manager.AddComponent<CardGameManager>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(_manager.gameObject);
        }

        [UnityTest]
        public IEnumerator CanLoadDefaultGames()
        {
            if (Directory.Exists(UnityCardGame.GamesDirectoryPath))
                Directory.Delete(UnityCardGame.GamesDirectoryPath, true);
            PlayerPrefs.SetString(CardGameManager.PlayerPrefDefaultGame, Tags.StandardPlayingCardsDirectoryName);

            _manager.LookupCardGames();
            _manager.ResetCurrentToDefault();
            _manager.ResetGameScene();
            yield return new WaitUntil(() => !CardGameManager.Current.IsDownloading);

            Assert.IsTrue(CardGameManager.Current.HasLoaded);
            Assert.IsTrue(string.IsNullOrEmpty(CardGameManager.Current.Error));
            Assert.AreEqual("Standard Playing Cards", CardGameManager.Current.Name);

            // TODO: DOMINOES AND MAHJONG
        }

        [UnityTest]
        public IEnumerator CanGetArcmage()
        {
            // Download
            yield return _manager.GetCardGame("https://www.cardgamesimulator.com/games/Arcmage/Arcmage.json");
            Assert.IsTrue(CardGameManager.Current.HasLoaded);
            Assert.IsTrue(string.IsNullOrEmpty(CardGameManager.Current.Error));
            Assert.AreEqual("Arcmage", CardGameManager.Current.Name);

            // Update
            // TODO
        }
    }
}
