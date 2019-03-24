using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileMaintenance;
namespace FMaintenaceUnitTest
{
    [TestClass]
    public class TestJobMaintenance
    {
        [TestMethod]
        public void TestAddJobConfig()
        {
                var newJob = new MaintSch() { JobType = JobType.Move, Interval = JobInterval.Day, SpecificDay = SpecialDay.NotInUse, IntervalToKeep = 30, IncludeSubFolder = false, FolderName = "C:\\test\\", TargetFolderName = "" };
                MSch MJobTest = new MSch();
                Assert.IsTrue(MJobTest.addSchedule(newJob));
        }

        [TestMethod]
        public void TestMoveJobs()
        { 
        }

        [TestMethod]
        public void TestDeleteJob()
        {
        }
    }
}
