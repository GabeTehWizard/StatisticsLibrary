using Microsoft.VisualStudio.TestTools.UnitTesting;
using StatisticsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StatisticsLibrary.ZTable; // This is my using statement to get access to the library

namespace StatisticsLibrary.Tests
{
    [TestClass()]
    public class ZTableTests
    {
        /// <summary>
        /// Testing for Correctness - Testing if the correct dictionary values are returned.
        /// Parameters: testZscore = -3.46 
        /// Expected = 0.0003
        /// </summary>
        [TestMethod()]
        public void GetValueByZScoreTestA1()
        {
            //Arrange
            double testZscore = -3.46;
            double expected = .0003;
            double delta = .00001;

            //Act
            double actual = GetValueByZScore(testZscore);

            //Assert
            Assert.AreEqual(expected, actual, delta, $"The z-value was not the expected result for {testZscore}");
        }

        /// <summary>
        /// Boundary Testing - Testing at Lower Boundary
        /// Paramaters: lowerBoundary = -3.49
        /// Expected: .0002
        /// </summary>
        [TestMethod()]
        public void GetValueByZScoreTestA2()
        {
            //Arrange
            double lowerBoundary = -3.49;
            double expected = .0002;
            double delta = .00001;

            //Act
            double actual = GetValueByZScore(lowerBoundary);

            //Assert
            Assert.AreEqual(expected, actual, delta, $"The z-value was not the expected result for the lower boundary: {lowerBoundary}");
        }

        /// <summary>
        /// Boundary Testing - Below the Lower Boundary
        /// Delta: .001
        /// Parameters: lowerBoundary = -3.49 => LowerBoundary - Delta = -3.491
        /// Expected: ArgumentOutOfRangeException
        /// </summary>
        [TestMethod()]
        public void GetValueByZScoreTestA3()
        {
            //Arrange
            double lowerBoundary = -3.49;
            double delta = .001;
            lowerBoundary = lowerBoundary - delta;

            try
            {
                //Act
                GetValueByZScore(lowerBoundary);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                //Assert
                StringAssert.Contains(ex.Message, zScoreOutOfRangeMessage, zScoreOutOfRangeMessage);
                return;
            }

            // Assert - Fail if the test gets here, because we have not reached our expected exception
            Assert.Fail();
        }
    }
}