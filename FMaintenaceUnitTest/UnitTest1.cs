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
                var newJob = new MaintSch() { JobType = JobType.Move, Interval = JobInterval.Month, SpecificDay = SpecialDay.NotInUse, IntervalToKeep = 2, IncludeSubFolder = false, FolderName = "C:\\inetpub\\temp\\", TargetFolderName = "c:\\temp",JobName="FirstTest",IsJobActive=true,KeepIntervalsType=KeepIntervalType.Month };
                MSch MJobTest = new MSch();
                MJobTest._AppPath = "C:\\ProbeAPI\\ProbeGateway\\bin\\Release";
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
