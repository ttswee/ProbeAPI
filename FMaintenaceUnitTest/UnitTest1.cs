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
            try
            {
                var newJob = new MaintSch() { JobType = JobType.Move, SpecificDay = 0, IntervalToKeep = 2, IncludeSubFolder = true, FolderName = "c:\\swee\\", TargetFolderName = "c:\\swee\\release", JobName = "TestMove1", IsJobActive = true, KeepIntervalsType = KeepIntervalType.Month, SpecialDay = SpecialDay.NotInUse, DebugMode = true, FileExt = "*.xml;*.txt" };
                MSch MJobTest = new MSch();
                MJobTest._AppPath = "C:\\ProbeAPI\\ProbeGateway\\bin\\Release";
                Assert.IsTrue(MJobTest.addSchedule(newJob));
                newJob = new MaintSch() { JobType = JobType.Move, SpecificDay = 0, IntervalToKeep = 15, IncludeSubFolder = true, FolderName = "c:\\swee\\", TargetFolderName = "c:\\swee\\release", JobName = "TestMove2", IsJobActive = true, KeepIntervalsType = KeepIntervalType.Days, SpecialDay = SpecialDay.NotInUse, DebugMode = true, FileExt = "*.xml;*.txt" };
                Assert.IsTrue(MJobTest.addSchedule(newJob));

            }
            catch (Exception ex)
            {
                //Assert.IsTrue( ex.Message=="Same job already exist");
            }


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
