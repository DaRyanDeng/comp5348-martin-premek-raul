﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bank.Process.SystemLogService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SystemLogService.ISystemLogService")]
    public interface ISystemLogService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemLogService/LogEvent", ReplyAction="http://tempuri.org/ISystemLogService/LogEventResponse")]
        void LogEvent(string sSource, string sMessage);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISystemLogServiceChannel : Bank.Process.SystemLogService.ISystemLogService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SystemLogServiceClient : System.ServiceModel.ClientBase<Bank.Process.SystemLogService.ISystemLogService>, Bank.Process.SystemLogService.ISystemLogService {
        
        public SystemLogServiceClient() {
        }
        
        public SystemLogServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SystemLogServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SystemLogServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SystemLogServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void LogEvent(string sSource, string sMessage) {
            base.Channel.LogEvent(sSource, sMessage);
        }
    }
}
