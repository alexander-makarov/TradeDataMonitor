using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeDataMonitoring;

namespace TradeDataMonitorTest
{
    [TestClass]
    public class TradeDataPackageTest
    {
        [TestMethod]
        public void Constuctor_NoParams_NoException()
        {
            // arrange
            var date = new DateTime(2013, 5, 20);
            var open = new decimal(30.16);
            var high = new decimal(30.39);
            var low = new decimal(30.02);
            var close = new decimal(30.17);
            var volume = 1478200;

            // act
            var package = new TradeDataPackage();

            // assert
            Assert.IsTrue(package.TradeDataList.Count == 0);
        }

        [TestMethod]
        public void Constuctor_TradeDataList_NoException()
        {
            // arrange
            var date = new DateTime(2013, 5, 20);
            var open = new decimal(30.16);
            var high = new decimal(30.39);
            var low = new decimal(30.02);
            var close = new decimal(30.17);
            var volume = 1478200;
            var data = new TradeData(date, open, high, low, close, volume);
            var dataList = new List<TradeData> {data, data, data};

            // act
            var package = new TradeDataPackage(dataList);

            // assert
            Assert.IsTrue(package.TradeDataList.Count == dataList.Count);
            Assert.AreSame(package.TradeDataList, dataList);
        }
    }
}
