using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelpers;
using TradeDataMonitoring;

namespace TradeDataMonitorTest
{
    [TestClass]
    public class TxtFileTradeDataLoaderTest
    {
        private TxtFileTradeDataLoader _loader; 

        [TestInitialize]
        public void InitializeTest()
        {
            _loader = new TxtFileTradeDataLoader();
        }

        [TestMethod]
        public void CouldLoad_TxtFile_NoException()
        {
            // arrange
            var csvFilePath = "X:\\folder1\\folder2\\tradedata.txt";
            var csvFile = new FileInfo(csvFilePath);

            // act
            var couldLoad = _loader.CouldLoad(csvFile);

            // assert
            Assert.IsTrue(couldLoad, "Should be able to load .txt files");
        }

        [TestMethod]
        public void CouldLoad_NotSupportedFile_NoException()
        {
            // arrange
            var csvFilePath = "X:\\folder1\\folder2\\tradedata.xml";
            var csvFile = new FileInfo(csvFilePath);

            // act
            var couldLoad = _loader.CouldLoad(csvFile);

            // assert
            Assert.IsFalse(couldLoad, "Should not be able to load any files except .txt ones");
        }

        [TestMethod]
        public void LoadTradeData_TxtFile_NoException()
        {
            // arrange
            const string txtFileAsString =
              @"Date;Open;High;Low;Close;Volume
                2014-5-20;30.16;30.39;30.02;30.17;1478200
                2014-5-17;29.77;30.26;29.77;30.26;2481400
                2014-5-16;29.78;29.94;29.55;29.67;1077000";
            var fileInMemory = Helpers.GenerateStreamFromString(txtFileAsString);
            #region prepare expected values
            var expectedPackage = new TradeDataPackage();
            expectedPackage.TradeDataList.Add(
                new TradeData(
                    new DateTime(2014, 5, 20),
                    new decimal(30.16),
                    new decimal(30.39),
                    new decimal(30.02),
                    new decimal(30.17),
                    1478200
                    ));
            expectedPackage.TradeDataList.Add(
                new TradeData(
                    new DateTime(2014, 5, 17),
                    new decimal(29.77),
                    new decimal(30.26),
                    new decimal(29.77),
                    new decimal(30.26),
                    2481400
                    ));
            expectedPackage.TradeDataList.Add(
                new TradeData(
                    new DateTime(2014, 5, 16),
                    new decimal(29.78),
                    new decimal(29.94),
                    new decimal(29.55),
                    new decimal(29.67),
                    1077000
                    ));
            #endregion

            // act
            var package = _loader.LoadTradeData(fileInMemory);

            // assert
            Assert.IsTrue(package.TradeDataList.Count == 3); // check the count

            for (int i = 0; i < 3; i++) // check all the values
            {
                var actual = package.TradeDataList[i];
                var expected = expectedPackage.TradeDataList[i];
                Assert.AreEqual(actual, expected, "One of the values has been read incorrect");
            }
        }

        [TestMethod]
        public void LoadTradeData_EmptyTxtFile_NoException()
        {
            // arrange
            const string txtFileAsString = "";
            var fileInMemory = Helpers.GenerateStreamFromString(txtFileAsString);

            // act
            var package = _loader.LoadTradeData(fileInMemory);

            // assert
            Assert.IsTrue(package.TradeDataList.Count == 0); // check the count
        }

        [TestMethod]
        public void LoadTradeData_CorruptedTxtFile_NoException()
        {
            // arrange
            const string txtFileAsString =
              @"Date;Open;High;Low;Close;Volume
                2014-5-20;30.16;30.39;30.02;30.17;1478200
                2014-##this-line-is-somewhat-corrupted####.26,00-jkl277,30.26,2481j400
                2014-5-16;29.78;29.94;29.55;29.67;1077000";

            var fileInMemory = Helpers.GenerateStreamFromString(txtFileAsString);
            #region prepare expected values
            var expectedPackage = new TradeDataPackage();
            expectedPackage.TradeDataList.Add(
                new TradeData(
                    new DateTime(2014, 5, 20),
                    new decimal(30.16),
                    new decimal(30.39),
                    new decimal(30.02),
                    new decimal(30.17),
                    1478200
                    ));
            expectedPackage.TradeDataList.Add(
                new TradeData(
                    new DateTime(2014, 5, 16),
                    new decimal(29.78),
                    new decimal(29.94),
                    new decimal(29.55),
                    new decimal(29.67),
                    1077000
                    ));
            #endregion

            // act
            var package = _loader.LoadTradeData(fileInMemory);

            // assert
            Assert.IsTrue(package.TradeDataList.Count == 2); // check the count

            for (int i = 0; i < 2; i++) // check all the values
            {
                var actual = package.TradeDataList[i];
                var expected = expectedPackage.TradeDataList[i];
                Assert.AreEqual(actual, expected, "One of the values has been read incorrect");
            }
        }
    }
}
