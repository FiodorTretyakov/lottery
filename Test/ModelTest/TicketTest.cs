using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lottery.Models;

namespace Test
{
    [TestClass]
    public class TicketTest
    {
        [TestMethod]
        public void CreateTicket()
        {
            var ticket = new Ticket(new List<List<int>> { new List<int> { 1, 2, 1 } });

            Assert.IsNotNull(ticket);
            Assert.AreEqual(1, ticket.Lines.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UncheckCheckedFailed()
        {
            var ticket = new Ticket(new List<List<int>> { new List<int> { 1, 2, 1 } });
            ticket.IsChecked = true;
            ticket.IsChecked = false;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InitZeroLinesFailed()
        {
            new Ticket(new List<List<int>>());
        }
    }
}