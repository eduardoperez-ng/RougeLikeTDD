using System.Collections;
using Completed;
using Completed.Interfaces;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
    public class PlayerTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void PlayerTestsSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator PlayerTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
        
        
        [UnityTest]
        public IEnumerator AttemptMoveRemovesOneFoodUnit()
        {
            var gameManager = GivenAGameManager();
            
            var player = GivenAPlayer();
            player.Init(100);
            
            player.AttemptMove<Wall>(1, 1);
            
            Assert.AreEqual(99, player.Food);

            yield return null;
        }

        private static IGameManager GivenAGameManager()
        {
            var gameManager = Substitute.For<IGameManager>();
            return gameManager;
        }

        private static Player GivenAPlayer()
        {
            var playerGameObject = CreatePlayerGameObject();

            var player = playerGameObject.AddComponent<Player>();
            return player;
        }

        private static GameObject CreatePlayerGameObject()
        {
            var playerGameObject = new GameObject("PlayerGameObject");
            playerGameObject.AddComponent<BoxCollider2D>();
            playerGameObject.AddComponent<Text>();
            playerGameObject.AddComponent<Rigidbody2D>();
            return playerGameObject;
        }
    }
}
