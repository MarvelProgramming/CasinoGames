using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoGames.Core.Tests
{
    public class ChipstackTests
    {
        [Test]
        public void DecreaseSize_Decreases_Size_By_Correct_Amount()
        {
            #region Arrange

            var chipstack = new Chipstack(new CasinoChip(CasinoChipType.One), 10);

            #endregion

            #region Act

            chipstack.DecreaseSize(1);

            #endregion

            #region Assert

            Assert.AreEqual(chipstack.Size, 9, $"chipstack size is {chipstack.Size}, expected 9");

            #endregion
        }

        [Test]
        public void DecreaseSize_Throws_With_Negative_Input()
        {
            #region Arrange

            var chipstack = new Chipstack(new CasinoChip(CasinoChipType.One), 0);

            #endregion

            #region Assert

            Assert.Throws(typeof(ArgumentException), () => chipstack.DecreaseSize(-1), $"Expected {nameof(Chipstack.DecreaseSize)} to throw with negative input");

            #endregion
        }

        [Test]
        public void DecreaseSize_Does_Not_Result_In_Negative_Size()
        {
            #region Arrange

            var chipstack = new Chipstack(new CasinoChip(CasinoChipType.One), 0);

            #endregion

            #region Act

            chipstack.DecreaseSize(1);

            #endregion

            #region Assert

            Assert.AreEqual(chipstack.Size, 0, $"chipstack size is {chipstack.Size}, expected 0");

            #endregion
        }
    }
}
