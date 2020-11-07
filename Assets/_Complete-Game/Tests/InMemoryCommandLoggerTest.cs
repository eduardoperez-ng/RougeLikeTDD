using System.Collections;
using System.Collections.Generic;
using Completed.Commands;
using Completed.Commands.Logger;
using Completed.Interfaces;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class InMemoryCommandLoggerTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestSimplePasses()
        {
            // Use the Assert class to test conditions
        }
        
        [UnityTest]
        public IEnumerator ConstructorPass()
        {
            var levelManager = Substitute.For<ILevelManager>();
            var commandLogger = new InMemoryCommandLogger(levelManager);
            Assert.NotNull(commandLogger);
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator LogCommandPass()
        {
            var levelManager = Substitute.For<ILevelManager>();
            levelManager.CurrentDay.Returns(1);
            
            var commandLogger = new InMemoryCommandLogger(levelManager);
            
            commandLogger.LogCommand(new Command());
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator LogCommand_Adds_Command_To_CommandList()
        {
            var levelManager = Substitute.For<ILevelManager>();
            levelManager.CurrentDay.Returns(1);
            
            var commandLogger = new InMemoryCommandLogger(levelManager);
            
            commandLogger.LogCommand(new Command());

            var commandsLogged = commandLogger.CommandsForDay(levelManager.CurrentDay);
            
            Assert.IsNotEmpty(commandsLogged);
            
            yield return null;
        }
        
        // TODO: hacer un test que ejecutar varios commandos y
        // validar que la lista tiene esos comandos.
        
        
        // TODO: hacer un test que ejecutar varios commandos y
        // para varios dias y las listas de comandos son correctas.

    }
}