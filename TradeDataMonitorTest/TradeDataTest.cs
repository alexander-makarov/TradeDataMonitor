using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeDataMonitoring;

namespace TradeDataMonitorTest
{
    [TestClass]
    public class TradeDataTest
    {
        [TestMethod]
        public void Constuctor_NoException()
        {
            // arrange
            var date = new DateTime(2013, 5, 20);
            var open = new decimal(30.16);
            var high = new decimal(30.39);
            var low = new decimal(30.02);
            var close = new decimal(30.17);
            var volume = 1478200;

            // act
            var data = new TradeData(date, open, high, low, close, volume);

            // assert
            Assert.AreEqual(data.Date, date);
            Assert.AreEqual(data.Open, open);
            Assert.AreEqual(data.High, high);
            Assert.AreEqual(data.Low, low);
            Assert.AreEqual(data.Close, close);
            Assert.AreEqual(data.Volume, volume);
        }

        #region Equals method
        [TestMethod]
        public void Equals_SameValues_NoException()
        {
            // arrange
            var date = new DateTime(2013, 5, 20);
            var open = new decimal(30.16);
            var high = new decimal(30.39);
            var low = new decimal(30.02);
            var close = new decimal(30.17);
            var volume = 1478200;

            // act
            var data1 = new TradeData(date, open, high, low, close, volume);
            var data2 = new TradeData(date, open, high, low, close, volume);

            // assert
            Assert.IsTrue(data1.Equals(data2));
            Assert.IsTrue(data2.Equals(data1));
            Assert.IsTrue(data1 == data2);
            Assert.AreEqual(data1, data2);
        }

        [TestMethod]
        public void Equals_NullValue_NoException()
        {
            // arrange
            var date = new DateTime(2013, 5, 20);
            var open = new decimal(30.16);
            var high = new decimal(30.39);
            var low = new decimal(30.02);
            var close = new decimal(30.17);
            var volume = 1478200;

            // act
            var data1 = new TradeData(date, open, high, low, close, volume);
            TradeData data2 = null;

            // assert
            Assert.IsFalse(data1.Equals(data2));
            Assert.IsFalse(data1 == data2);
            Assert.AreNotEqual(data1, data2);
        }

        [TestMethod]
        public void Equals_DiffVolumeValues_NoException()
        {
            // arrange
            var date = new DateTime(2013, 5, 20);
            var open = new decimal(30.16);
            var high = new decimal(30.39);
            var low = new decimal(30.02);
            var close = new decimal(30.17);
            var volume = 1478200;

            // act
            var data1 = new TradeData(date, open, high, low, close, volume);
            var data2 = new TradeData(date, open, high, low, close, volume + 1);

            // assert
            Assert.IsFalse(data1.Equals(data2));
            Assert.IsFalse(data2.Equals(data1));
            Assert.IsFalse(data1 == data2);
            Assert.AreNotEqual(data1, data2);
        }

        [TestMethod]
        public void Equals_DiffCloseValues_NoException()
        {
            // arrange
            var date = new DateTime(2013, 5, 20);
            var open = new decimal(30.16);
            var high = new decimal(30.39);
            var low = new decimal(30.02);
            var close = new decimal(30.17);
            var volume = 1478200;

            // act
            var data1 = new TradeData(date, open, high, low, close, volume);
            var data2 = new TradeData(date, open, high, low, close + 1, volume);

            // assert
            Assert.IsFalse(data1.Equals(data2));
            Assert.IsFalse(data2.Equals(data1));
            Assert.IsFalse(data1 == data2);
            Assert.AreNotEqual(data1, data2);
        }

        [TestMethod]
        public void Equals_DiffLowValues_NoException()
        {
            // arrange
            var date = new DateTime(2013, 5, 20);
            var open = new decimal(30.16);
            var high = new decimal(30.39);
            var low = new decimal(30.02);
            var close = new decimal(30.17);
            var volume = 1478200;

            // act
            var data1 = new TradeData(date, open, high, low, close, volume);
            var data2 = new TradeData(date, open, high, low + 1, close, volume);

            // assert
            Assert.IsFalse(data1.Equals(data2));
            Assert.IsFalse(data2.Equals(data1));
            Assert.IsFalse(data1 == data2);
            Assert.AreNotEqual(data1, data2);
        }

        [TestMethod]
        public void Equals_DiffHighValues_NoException()
        {
            // arrange
            var date = new DateTime(2013, 5, 20);
            var open = new decimal(30.16);
            var high = new decimal(30.39);
            var low = new decimal(30.02);
            var close = new decimal(30.17);
            var volume = 1478200;

            // act
            var data1 = new TradeData(date, open, high, low, close, volume);
            var data2 = new TradeData(date, open, high + 1, low, close, volume);

            // assert
            Assert.IsFalse(data1.Equals(data2));
            Assert.IsFalse(data2.Equals(data1));
            Assert.IsFalse(data1 == data2);
            Assert.AreNotEqual(data1, data2);
        }

        [TestMethod]
        public void Equals_DiffOpenValues_NoException()
        {
            // arrange
            var date = new DateTime(2013, 5, 20);
            var open = new decimal(30.16);
            var high = new decimal(30.39);
            var low = new decimal(30.02);
            var close = new decimal(30.17);
            var volume = 1478200;

            // act
            var data1 = new TradeData(date, open, high, low, close, volume);
            var data2 = new TradeData(date, open + 1, high, low, close, volume);

            // assert
            Assert.IsFalse(data1.Equals(data2));
            Assert.IsFalse(data2.Equals(data1));
            Assert.IsFalse(data1 == data2);
            Assert.AreNotEqual(data1, data2);
        }

        [TestMethod]
        public void Equals_DiffDateValues_NoException()
        {
            // arrange
            var date = new DateTime(2013, 5, 20);
            var open = new decimal(30.16);
            var high = new decimal(30.39);
            var low = new decimal(30.02);
            var close = new decimal(30.17);
            var volume = 1478200;

            // act
            var data1 = new TradeData(date, open, high, low, close, volume);
            var data2 = new TradeData(date.AddDays(1), open, high, low, close + 1, volume);

            // assert
            Assert.IsFalse(data1.Equals(data2));
            Assert.IsFalse(data2.Equals(data1));
            Assert.IsFalse(data1 == data2);
            Assert.AreNotEqual(data1, data2);
        }
        #endregion

        #region Parse method (static)
        [TestMethod]
        public void Parse_AllCorrectValues_NoException()
        {
            // arrange
            const string correctCsvString = "2013-5-20,30.16,30.39,30.02,30.17,1478200";
            var corectValues = correctCsvString.Split(',');

            // act
            var data = TradeData.Parse(corectValues);
            
            // assert
            Assert.AreEqual(data.Date, new DateTime(2013, 5, 20));
            Assert.AreEqual(data.Open, new decimal(30.16));
            Assert.AreEqual(data.High, new decimal(30.39));
            Assert.AreEqual(data.Low, new decimal(30.02));
            Assert.AreEqual(data.Close, new decimal(30.17));
            Assert.AreEqual(data.Volume, 1478200);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Parse_NullValuesArray_ArgumentNullException()
        {
            // arrange
            string[] nullValuesArray = null;

            // act
            var data = TradeData.Parse(nullValuesArray);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_NotAllValues_FormatException()
        {
            // arrange
            const string csvStringWithNoVolume = "2013-5-20,30.16,30.39,30.02,30.17";
            var valuesWithoutVolume = csvStringWithNoVolume.Split(',');

            // act
            var data = TradeData.Parse(valuesWithoutVolume);
        }

        [TestMethod]
        public void Parse_AllValuesPlusExtras_NoException()
        {
            // arrange
            const string csvString = "2013-5-20,30.16,30.39,30.02,30.17,1478200,some-extra-data1, some-extra-data2";
            var valuesWithExtras = csvString.Split(',');

            // act
            var data = TradeData.Parse(valuesWithExtras);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_IncorrectDateValue_FormatException()
        {
            // arrange
            const string csvStringWithIncorrectDateFormat = "2013####,30.16,30.39,30.02,30.17,1478200";
            var incorrectValues = csvStringWithIncorrectDateFormat.Split(',');

            // act
            var data = TradeData.Parse(incorrectValues);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_IncorrectNullDateValue_FormatException()
        {
            // arrange
            const string csvStringWithIncorrectDateFormat = "2013####,30.16,30.39,30.02,30.17,1478200";
            var incorrectValues = csvStringWithIncorrectDateFormat.Split(',');
            incorrectValues[0] = null;

            // act
            var data = TradeData.Parse(incorrectValues);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_IncorrectOpenValue_FormatException()
        {
            // arrange
            const string csvStringWithIncorrectDateFormat = "2013-5-20,30##16,30.39,30.02,30.17,1478200";
            var incorrectValues = csvStringWithIncorrectDateFormat.Split(',');

            // act
            var data = TradeData.Parse(incorrectValues);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_IncorrectHighValue_FormatException()
        {
            // arrange
            const string csvStringWithIncorrectDateFormat = "2013-5-20,30.16,30##.39,30.02,30.17,1478200";
            var incorrectValues = csvStringWithIncorrectDateFormat.Split(',');

            // act
            var data = TradeData.Parse(incorrectValues);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_IncorrectLowValue_FormatException()
        {
            // arrange
            const string csvStringWithIncorrectDateFormat = "2013-5-20,30.16,30.39,30##.02,30.17,1478200";
            var incorrectValues = csvStringWithIncorrectDateFormat.Split(',');

            // act
            var data = TradeData.Parse(incorrectValues);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_IncorrectCloseValue_FormatException()
        {
            // arrange
            const string csvStringWithIncorrectDateFormat = "2013-5-20,30.16,30.39,30.02,30##17,1478200";
            var incorrectValues = csvStringWithIncorrectDateFormat.Split(',');

            // act
            var data = TradeData.Parse(incorrectValues);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_IncorrectVolumeValue_FormatException()
        {
            // arrange
            const string csvStringWithIncorrectDateFormat = "2013-5-20,30.16,30.39,30.02,30.17,##123#";
            var incorrectValues = csvStringWithIncorrectDateFormat.Split(',');

            // act
            var data = TradeData.Parse(incorrectValues);
        }
        #endregion
    }
}
