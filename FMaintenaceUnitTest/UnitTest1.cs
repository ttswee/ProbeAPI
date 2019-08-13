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
                MSch MJobTest = new MSch();
                MJobTest._AppPath = "C:\\xml";
                var newJob = new MaintSch() { JobType = JobType.Delete, SpecificDay = 0, IntervalToKeep = 60, IncludeSubFolder = false, FolderName = "F:\\SCBCRES\\CCMS_MORT\\DONE\\", TargetFolderName = "", JobName = "DELETECCMSMORTDONE", IsJobActive = true, KeepIntervalsType = KeepIntervalType.Days, SpecialDay = SpecialDay.NotInUse, DebugMode = false, FileExt = "*.xml" };
                Assert.IsTrue(MJobTest.addSchedule(newJob));
                newJob = new MaintSch() { JobType = JobType.Delete, SpecificDay = 0, IntervalToKeep = 60, IncludeSubFolder = false, FolderName = "F:\\SCBCRES\\EBBS_MORT\\DONE\\", TargetFolderName = "", JobName = "DELETEEBBSDONE", IsJobActive = true, KeepIntervalsType = KeepIntervalType.Days, SpecialDay = SpecialDay.NotInUse, DebugMode = false, FileExt = "*.xml" };
                Assert.IsTrue(MJobTest.addSchedule(newJob));
                newJob = new MaintSch() { JobType = JobType.Delete, SpecificDay = 0, IntervalToKeep = 60, IncludeSubFolder = false, FolderName = "F:\\SCBCRES\\RLS_MORT\\DONE\\", TargetFolderName = "", JobName = "DELETERLSDONE", IsJobActive = true, KeepIntervalsType = KeepIntervalType.Days, SpecialDay = SpecialDay.NotInUse, DebugMode = false, FileExt = "*.xml" };
                Assert.IsTrue(MJobTest.addSchedule(newJob));
                newJob = new MaintSch() { JobType = JobType.Delete, SpecificDay = 0, IntervalToKeep = 60, IncludeSubFolder = false, FolderName = "E:\\MSSQL\\Backup\\", TargetFolderName = "", JobName = "MSSQLBACKUP", IsJobActive = true, KeepIntervalsType = KeepIntervalType.Days, SpecialDay = SpecialDay.NotInUse, DebugMode = false, FileExt = "*.xml" };
                Assert.IsTrue(MJobTest.addSchedule(newJob));
                newJob = new MaintSch() { JobType = JobType.Move, SpecificDay = 0, IntervalToKeep = 1, IncludeSubFolder = false, FolderName = "E:\\Malaysia\\Fraud ", TargetFolderName = "E:\\Archive (Mark for Deletion)\\E\\Fraud\\", JobName = "FRAUD", IsJobActive = true, KeepIntervalsType = KeepIntervalType.Year, SpecialDay = SpecialDay.NotInUse, DebugMode = false, FileExt = "*.*" };
                Assert.IsTrue(MJobTest.addSchedule(newJob));
                newJob = new MaintSch() { JobType = JobType.Delete, SpecificDay = 0, IntervalToKeep = 1, IncludeSubFolder = false, FolderName = "E:\\temp\\", TargetFolderName = "", JobName = "EDRIVETEMP", IsJobActive = true, KeepIntervalsType = KeepIntervalType.Year, SpecialDay = SpecialDay.NotInUse, DebugMode = false, FileExt = "*.zip" };
                Assert.IsTrue(MJobTest.addSchedule(newJob));
                newJob = new MaintSch() { JobType = JobType.Delete, SpecificDay = 0, IntervalToKeep = 1, IncludeSubFolder = false, FolderName = "E:\\Malaysia\\cdm\\", TargetFolderName = "", JobName = "MALAYSIACDM", IsJobActive = true, KeepIntervalsType = KeepIntervalType.Year, SpecialDay = SpecialDay.NotInUse, DebugMode = false, FileExt = "*.*" };
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
