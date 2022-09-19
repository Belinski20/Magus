using System;
using Magus;
using Magus.Entity;
using Magus.GameBoard;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unit_Tests
{
    [TestClass]
    public class CharacterClassUnitTests
    {
        [TestMethod]
        public void CheckCharacterIsNotNull()
        {
            Form1 form = new Form1();
            GameManager gM = new GameManager(form);
            gM.ChangeGameState(Magus.UI.MenuState.Splash);
            Character character = gM.GetCharacterManager().Player;
            Assert.IsNotNull(character, "Character is null");
        }

        [TestMethod]
        public void CheckCharacterClassChanges()
        {
            Form1 form = new Form1();
            GameManager gM = new GameManager(form);
            gM.ChangeGameState(Magus.UI.MenuState.Splash);
            ClassTypes cClass = gM.GetCharacterManager().Player.CharacterClass;
            Image cIcon = gM.GetCharacterManager().Player.Icon;
            gM.GetNextClass();
            ClassTypes cClass2 = gM.GetCharacterManager().Player.CharacterClass;
            Image cIcon2 = gM.GetCharacterManager().Player.Icon;
            Assert.IsTrue(cClass != cClass2, "Character class did not change: " + cClass + " to " + cClass2);
            Assert.IsTrue(cIcon != cIcon2, "Character icon did not change");
            Assert.IsTrue(Enum.IsDefined(typeof(ClassTypes), cClass2), "New character class is out of bounds");
            Assert.IsTrue(Enum.IsDefined(typeof(ClassTypes), cClass), "Original Character class is out of bounds");
        }

        [TestMethod]
        public void CheckCharacterNextClass()
        {
            Form1 form = new Form1();
            GameManager gM = new GameManager(form);
            gM.ChangeGameState(Magus.UI.MenuState.Splash);
            ClassTypes cClass = gM.GetCharacterManager().Player.CharacterClass;
            gM.GetNextClass();
            ClassTypes cClass2 = gM.GetCharacterManager().Player.CharacterClass;
            Assert.IsTrue(cClass == ClassTypes.Barbarian, "Character start class is not Barbarian: " + cClass);
            Assert.IsTrue(cClass2 == ClassTypes.Shaman, "Next character class is not Shaman: " + cClass2);
        }

        [TestMethod]
        public void CheckCharacterPrevClass()
        {
            Form1 form = new Form1();
            GameManager gM = new GameManager(form);
            gM.ChangeGameState(Magus.UI.MenuState.Splash);
            ClassTypes cClass = gM.GetCharacterManager().Player.CharacterClass;
            gM.GetPreviousClass();
            ClassTypes cClass2 = gM.GetCharacterManager().Player.CharacterClass;
            Assert.IsTrue(cClass == ClassTypes.Barbarian, "Character start class is not Barbarian: " + cClass);
            Assert.IsTrue(cClass2 == ClassTypes.DuckMage, "Prev character class is not DuckMage: " + cClass2);
        }

        [TestMethod]
        public void CheckCharacterNameNotEmpty()
        {
            Form1 form = new Form1();
            GameManager gM = new GameManager(form);
            gM.ChangeGameState(Magus.UI.MenuState.Splash);
            string cName = gM.GetCharacterManager().Player.Name;
            Assert.IsFalse(cName == "", "Character Name is empty");
            Assert.IsNotNull(cName, "Character Name is null");
        }
    }
}
