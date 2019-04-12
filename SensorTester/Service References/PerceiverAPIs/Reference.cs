﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SensorTester.PerceiverAPIs {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DriveSpaces", Namespace="http://schemas.datacontract.org/2004/07/GlobalAPI")]
    [System.SerializableAttribute()]
    public partial class DriveSpaces : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string driveLetterField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long freeSpaceField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string driveLetter {
            get {
                return this.driveLetterField;
            }
            set {
                if ((object.ReferenceEquals(this.driveLetterField, value) != true)) {
                    this.driveLetterField = value;
                    this.RaisePropertyChanged("driveLetter");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long freeSpace {
            get {
                return this.freeSpaceField;
            }
            set {
                if ((this.freeSpaceField.Equals(value) != true)) {
                    this.freeSpaceField = value;
                    this.RaisePropertyChanged("freeSpace");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MaintSch", Namespace="http://schemas.datacontract.org/2004/07/FileMaintenance")]
    [System.SerializableAttribute()]
    public partial class MaintSch : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool DebugModeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FileExtField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FolderNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IncludeSubFolderField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IntervalToKeepField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsJobActiveField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string JobNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private SensorTester.PerceiverAPIs.JobType JobTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private SensorTester.PerceiverAPIs.KeepIntervalType KeepIntervalsTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private SensorTester.PerceiverAPIs.SpecialDay SpecialDayField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int SpecificDayField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TargetFolderNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool DebugMode {
            get {
                return this.DebugModeField;
            }
            set {
                if ((this.DebugModeField.Equals(value) != true)) {
                    this.DebugModeField = value;
                    this.RaisePropertyChanged("DebugMode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FileExt {
            get {
                return this.FileExtField;
            }
            set {
                if ((object.ReferenceEquals(this.FileExtField, value) != true)) {
                    this.FileExtField = value;
                    this.RaisePropertyChanged("FileExt");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FolderName {
            get {
                return this.FolderNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FolderNameField, value) != true)) {
                    this.FolderNameField = value;
                    this.RaisePropertyChanged("FolderName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IncludeSubFolder {
            get {
                return this.IncludeSubFolderField;
            }
            set {
                if ((this.IncludeSubFolderField.Equals(value) != true)) {
                    this.IncludeSubFolderField = value;
                    this.RaisePropertyChanged("IncludeSubFolder");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int IntervalToKeep {
            get {
                return this.IntervalToKeepField;
            }
            set {
                if ((this.IntervalToKeepField.Equals(value) != true)) {
                    this.IntervalToKeepField = value;
                    this.RaisePropertyChanged("IntervalToKeep");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsJobActive {
            get {
                return this.IsJobActiveField;
            }
            set {
                if ((this.IsJobActiveField.Equals(value) != true)) {
                    this.IsJobActiveField = value;
                    this.RaisePropertyChanged("IsJobActive");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string JobName {
            get {
                return this.JobNameField;
            }
            set {
                if ((object.ReferenceEquals(this.JobNameField, value) != true)) {
                    this.JobNameField = value;
                    this.RaisePropertyChanged("JobName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public SensorTester.PerceiverAPIs.JobType JobType {
            get {
                return this.JobTypeField;
            }
            set {
                if ((this.JobTypeField.Equals(value) != true)) {
                    this.JobTypeField = value;
                    this.RaisePropertyChanged("JobType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public SensorTester.PerceiverAPIs.KeepIntervalType KeepIntervalsType {
            get {
                return this.KeepIntervalsTypeField;
            }
            set {
                if ((this.KeepIntervalsTypeField.Equals(value) != true)) {
                    this.KeepIntervalsTypeField = value;
                    this.RaisePropertyChanged("KeepIntervalsType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public SensorTester.PerceiverAPIs.SpecialDay SpecialDay {
            get {
                return this.SpecialDayField;
            }
            set {
                if ((this.SpecialDayField.Equals(value) != true)) {
                    this.SpecialDayField = value;
                    this.RaisePropertyChanged("SpecialDay");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int SpecificDay {
            get {
                return this.SpecificDayField;
            }
            set {
                if ((this.SpecificDayField.Equals(value) != true)) {
                    this.SpecificDayField = value;
                    this.RaisePropertyChanged("SpecificDay");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string TargetFolderName {
            get {
                return this.TargetFolderNameField;
            }
            set {
                if ((object.ReferenceEquals(this.TargetFolderNameField, value) != true)) {
                    this.TargetFolderNameField = value;
                    this.RaisePropertyChanged("TargetFolderName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="JobType", Namespace="http://schemas.datacontract.org/2004/07/FileMaintenance")]
    public enum JobType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Delete = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Move = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Compress = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="KeepIntervalType", Namespace="http://schemas.datacontract.org/2004/07/FileMaintenance")]
    public enum KeepIntervalType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Days = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Month = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Year = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SpecialDay", Namespace="http://schemas.datacontract.org/2004/07/FileMaintenance")]
    public enum SpecialDay : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NotInUse = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        LastDayOfMonth = 1,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PerceiverAPIs.ISpaceProbe")]
    public interface ISpaceProbe {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISpaceProbe/GetDriveInfo", ReplyAction="http://tempuri.org/ISpaceProbe/GetDriveInfoResponse")]
        SensorTester.PerceiverAPIs.DriveSpaces[] GetDriveInfo();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISpaceProbeChannel : SensorTester.PerceiverAPIs.ISpaceProbe, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SpaceProbeClient : System.ServiceModel.ClientBase<SensorTester.PerceiverAPIs.ISpaceProbe>, SensorTester.PerceiverAPIs.ISpaceProbe {
        
        public SpaceProbeClient() {
        }
        
        public SpaceProbeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SpaceProbeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SpaceProbeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SpaceProbeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public SensorTester.PerceiverAPIs.DriveSpaces[] GetDriveInfo() {
            return base.Channel.GetDriveInfo();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PerceiverAPIs.IFolderMaintenance")]
    public interface IFolderMaintenance {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFolderMaintenance/GetFolderInfo", ReplyAction="http://tempuri.org/IFolderMaintenance/GetFolderInfoResponse")]
        System.IO.FileInfo[] GetFolderInfo();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFolderMaintenanceChannel : SensorTester.PerceiverAPIs.IFolderMaintenance, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FolderMaintenanceClient : System.ServiceModel.ClientBase<SensorTester.PerceiverAPIs.IFolderMaintenance>, SensorTester.PerceiverAPIs.IFolderMaintenance {
        
        public FolderMaintenanceClient() {
        }
        
        public FolderMaintenanceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FolderMaintenanceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FolderMaintenanceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FolderMaintenanceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.IO.FileInfo[] GetFolderInfo() {
            return base.Channel.GetFolderInfo();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PerceiverAPIs.IJobMaintenance")]
    public interface IJobMaintenance {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobMaintenance/GetJobList", ReplyAction="http://tempuri.org/IJobMaintenance/GetJobListResponse")]
        SensorTester.PerceiverAPIs.MaintSch[] GetJobList();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IJobMaintenanceChannel : SensorTester.PerceiverAPIs.IJobMaintenance, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class JobMaintenanceClient : System.ServiceModel.ClientBase<SensorTester.PerceiverAPIs.IJobMaintenance>, SensorTester.PerceiverAPIs.IJobMaintenance {
        
        public JobMaintenanceClient() {
        }
        
        public JobMaintenanceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public JobMaintenanceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JobMaintenanceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JobMaintenanceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public SensorTester.PerceiverAPIs.MaintSch[] GetJobList() {
            return base.Channel.GetJobList();
        }
    }
}