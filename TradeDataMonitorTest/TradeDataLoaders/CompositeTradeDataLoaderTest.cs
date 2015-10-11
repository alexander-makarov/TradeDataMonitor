using System.Collections.Generic;
using System.IO;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeDataMonitoring;
using TradeDataMonitoring.TradeDataLoaders;

namespace TradeDataMonitorTest.TradeDataLoaders
{
    [TestClass]
    public class CompositeTradeDataLoaderTest
    {
        private CompositeTradeDataLoader _loader;

        [TestInitialize]
        public void InitializeTest()
        {
            var loader1 = new CsvFileTradeDataLoader();
            var loader2 = new TxtFileTradeDataLoader();
            var loaders = new List<ITradeDataLoader> { loader1, loader2 };
            _loader = new CompositeTradeDataLoader(loaders);
        }

        [TestMethod]
        public void CouldLoad_SupportedFile_NoException()
        {
            // arrange
            const string csvFilePath = "X:\\folder1\\folder2\\tradedata.csv";
            var csvFile = new FileInfo(csvFilePath);
            const string txtFilePath = "X:\\folder1\\folder2\\tradedata.txt";
            var txtFile = new FileInfo(txtFilePath);

            // act
            var couldLoadCsv = _loader.CouldLoad(csvFile);
            var couldLoadTxt = _loader.CouldLoad(txtFile);

            // assert
            Assert.IsTrue(couldLoadCsv, "Should be able to load .csv files");
            Assert.IsTrue(couldLoadTxt, "Should be able to load .txt files");
        }

        [TestMethod]
        public void CouldLoad_NotSupportedFile_NoException()
        {
            // arrange
            var xmlFilePath = "X:\\folder1\\folder2\\tradedata.xml";
            var xmlFile = new FileInfo(xmlFilePath);

            // act
            var couldLoad = _loader.CouldLoad(xmlFile);

            // assert
            Assert.IsFalse(couldLoad, "Should not be able to load any files except .xml ones");
        }

        [TestMethod]
        public void LoadTradeData_SupportedFile_NoException()
        {
            // arrange
            // note: here we are not using anything from [TestInitialize]
            const string csvFilePath = "X:\\folder1\\folder2\\tradedata.csv";
            var csvFile = new FileInfo(csvFilePath);

            var csvLoader = A.Fake<ITradeDataLoader>(); // setup first mocked loader
            csvLoader.CallsTo(l => l.CouldLoad(csvFile)).Returns(true); // supports .csv
            csvLoader.CallsTo(l => l.LoadTradeData(csvFile))
                .WithAnyArguments()
                .Returns(new TradeDataPackage()); // returns empty package from file

            var notCsvLoader = A.Fake<ITradeDataLoader>(); // setup second mocked loader
            notCsvLoader.CallsTo(l => l.CouldLoad(csvFile)).Returns(false); // does not support .csv

            // create composite to test:
            var loaders = new List<ITradeDataLoader> { csvLoader, notCsvLoader };
            var loader = new CompositeTradeDataLoader(loaders); // inject

            // act
            loader.LoadTradeData(csvFile); // try load csv

            // assert
            csvLoader.CallsTo(l => l.LoadTradeData(csvFile)).MustHaveHappened();
            notCsvLoader.CallsTo(l => l.LoadTradeData(csvFile)).MustNotHaveHappened();
        }
    }
}
