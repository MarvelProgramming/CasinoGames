using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CasinoGames.Core.Tests
{
    public class CasinoChipTests
    {
        [Test]
        public void GetValue_Returns_Correct_CasinoChipType_Constant()
        {
            #region Assemble

            List<CasinoChip> chips = new List<CasinoChip>();
            int[] chipTypes = (int[])Enum.GetValues(typeof(CasinoChipType));

            foreach (int chipType in chipTypes)
            {
                chips.Add(new CasinoChip((CasinoChipType)chipType));
            }

            #endregion

            #region Assert

            for (int i = 0; i < chips.Count; i++)
            {
                CasinoChip chip = chips[i];
                int chipTypeValue = chip.GetValue();
                Assert.AreEqual(chipTypeValue, chipTypes[i], $"chip[{i}] returned {chipTypeValue}, expected {chipTypes[i]}");
            }

            #endregion
        }
    }
}
