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
        
        // TODO: add test cases
        // TODO: test 2 when player moves movement list is updated.
        // TODO: add shadow.
        
        [UnityTest]
        public IEnumerator AttemptMoveRemovesOneFoodUnit()
        {
            var playerGameObject = new GameObject("PlayerGameObject");
            playerGameObject.AddComponent<BoxCollider2D>();
            playerGameObject.AddComponent<Text>();
            playerGameObject.AddComponent<Rigidbody2D>();
            
            var player = playerGameObject.AddComponent<Player>();

            var gameManager = Substitute.For<IGameManager>();
            
            player.Init(gameManager, 100);
            
            player.AttemptMove<Wall>(1, 1);
            
            Assert.AreEqual(99, player.Food);

            yield return null;
        }
    }
}
