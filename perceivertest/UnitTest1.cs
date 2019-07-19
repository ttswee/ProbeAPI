using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using PerceiverAPI;
using GlobalAPI;
namespace perceivertest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var processlist = new List<WindowsProcesses>();
            var i = new PerceiverAPIs();
            processlist = i.GetProcesses();
            Assert.IsTrue(processlist.Count > 0);
        }
    }
}
