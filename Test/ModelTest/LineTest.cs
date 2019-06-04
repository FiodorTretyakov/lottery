using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Lottery.Models;

namespace Test
{
    [TestClass]
    public class LineTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LineIncorrectSize()
        {
            new Line(new List<int> { 1, 2 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LineWrongValues()
        {
            new Line(new List<int> { 1, 2, 3 });
        }

        [TestMethod]
        public void LineCreated()
        {
            var numbers = new List<int> { 1, 2, 1 };
            var line = new Line(numbers);

            Assert.IsNotNull(line);
            Assert.AreEqual(JsonConvert.SerializeObject(numbers), JsonConvert.SerializeObject(line.Numbers));
        }

        [TestMethod]
        public void ResultIs10()
        {
            Assert.AreEqual(10, new Line(new List<int> { 1, 0, 1 }).Result);
            Assert.AreEqual(10, new Line(new List<int> { 2, 0, 0 }).Result);
        }

        [TestMethod]
        public void ResultIs5()
        {
            Assert.AreEqual(5, new Line(new List<int> { 1, 1, 1 }).Result);
            Assert.AreEqual(5, new Line(new List<int> { 0, 0, 0 }).Result);
        }

        [TestMethod]
        public void ResultIs1()
        {
            Assert.AreEqual(1, new Line(new List<int> { 1, 2, 2 }).Result);
            Assert.AreEqual(1, new Line(new List<int> { 0, 1, 2 }).Result);
        }

        [TestMethod]
        public void ResultIs0()
        {
            Assert.AreEqual(0, new Line(new List<int> { 1, 2, 1 }).Result);
            Assert.AreEqual(0, new Line(new List<int> { 2, 1, 2 }).Result);
        }
    }
}