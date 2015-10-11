using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelpers;
using TradeDataMonitoring;
using XmlFileTradeData;

namespace XmlFileTradeDataTest
{
    [TestClass]
    public class XmlFileTradeDataLoaderTest
    {
        private XmlFileTradeDataLoader _loader; 

        [TestInitialize]
        public void InitializeTest()
        {
            _loader = new XmlFileTradeDataLoader();
        }

        [TestMethod]
        public void CouldLoad_XmlFile_NoException()
        {
            // arrange
            var xmlFilePath = "X:\\folder1\\folder2\\tradedata.xml";
            var xmlFile = new FileInfo(xmlFilePath);

            // act
            var couldLoad = _loader.CouldLoad(xmlFile);

            // assert
            Assert.IsTrue(couldLoad, "Should be able to load .xml files");
        }

        [TestMethod]
        public void CouldLoad_NotSupportedFile_NoException()
        {
            // arrange
            var csvFilePath = "X:\\folder1\\folder2\\tradedata.csv";
            var csvFile = new FileInfo(csvFilePath);

            // act
            var couldLoad = _loader.CouldLoad(csvFile);

            // assert
            Assert.IsFalse(couldLoad, "Should not be able to load any files except .xml ones");
        }

        [TestMethod]
        public void LoadTradeData_XmlFile_NoException()
        {
            // arrange
            const string xmlFileAsString =
              @"<?xml version='1.0' encoding='utf-8' ?>
                <values>
                    <value date='2014-5-20' open='30.16' high='30.39' low='30.02' close='30.17' volume='1478200' />
                    <value date='2014-5-17' open='29.77' high='30.26' low='29.77' close='30.26' volume='2481400' />
                    <value date='2014-5-16' open='29.78' high='29.94' low='29.55' close='29.67' volume='1077000' />
                </values>";
            var fileInMemory = Helpers.GenerateStreamFromString(xmlFileAsString);
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
                Assert.AreEqual(expected, actual, "One of the values has been read incorrect");
            }
        }

        [TestMethod]
        public void LoadTradeData_EmptyXmlFile_NoException()
        {
            // arrange
            const string xmlFileAsString = "";
            var fileInMemory = Helpers.GenerateStreamFromString(xmlFileAsString);

            // act
            var package = _loader.LoadTradeData(fileInMemory);

            // assert
            Assert.IsTrue(package.TradeDataList.Count == 0); // check the count
        }

        [TestMethod]
        public void LoadTradeData_CorruptedXmlFile_NoException()
        {
            // arrange
            const string xmlFileAsString =
              @"<?xml version='1.0' encoding='utf-8' ?>
                <values>
                    <value date='2014-5-20' open='30.16' high='30.39' low='30.02' close='30.17' volume='1478200' />
                    2014-##this-line-is-somewhat-corrupted####.26,00-jkl277,30.26,2481j400
                    <value date='2014-5-16' open='29.78' high='29.94' low='29.55' close='29.67' volume='1077000' />
                </values>";

            var fileInMemory = Helpers.GenerateStreamFromString(xmlFileAsString);
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
                Assert.AreEqual(expected, actual, "One of the values has been read incorrect");
            }
        }
    }
}
