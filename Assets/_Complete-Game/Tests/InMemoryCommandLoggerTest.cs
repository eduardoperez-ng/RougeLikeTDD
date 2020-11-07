using System.Collections;
using System.Collections.Generic;
using Completed.Commands;
using Completed.Commands.Logger;
using Completed.Interfaces;
using NSubstitute;
using NUnit.Framework;
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
        
        [UnityTest]
        public IEnumerator Commands_Logged_For_One_Day_Pass()
        {
            var levelManager = Substitute.For<ILevelManager>();
            var commandLogger = new InMemoryCommandLogger(levelManager);
            var commandsToExecute = new List<Command>()
            {
                new MoveUpCommand(),
                new MoveDownCommand(),
                new MoveLeftCommand(),
                new MoveRightCommand()
            };

            LogCommandsForDay(1, commandsToExecute, levelManager, commandLogger);
            
            var commandsLogged= commandLogger.CommandsForDay(levelManager.CurrentDay);
            Assert.AreEqual(commandsToExecute, commandsLogged);
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator Commands_Logged_For_Several_Days_Pass()
        {
            var levelManager = Substitute.For<ILevelManager>();
            var commandLogger = new InMemoryCommandLogger(levelManager);
            var commandsToExecuteForDayOne = new List<Command>()
            {
                new MoveUpCommand(),
                new MoveDownCommand(),
                new MoveLeftCommand(),
                new MoveRightCommand()
            };
            var commandsToExecuteForDayTwo = new List<Command>()
            {
                new MoveDownCommand(),
                new MoveUpCommand(),
                new MoveRightCommand(),
                new MoveLeftCommand()
            };
            
            LogCommandsForDay(1, commandsToExecuteForDayOne, levelManager, commandLogger);
            LogCommandsForDay(2, commandsToExecuteForDayTwo, levelManager, commandLogger);
            
            var commandsLogged= commandLogger.CommandsForDay(2);
            Assert.AreEqual(commandsToExecuteForDayTwo, commandsLogged);
            yield return null;
        }

        private void LogCommandsForDay(int day, List<Command> commands, ILevelManager levelManager, ICommandLogger commandLogger)
        {
            levelManager.CurrentDay.Returns(day);
            foreach (var command in commands)
            {
                commandLogger.LogCommand(command);
            }
        }
    }
}