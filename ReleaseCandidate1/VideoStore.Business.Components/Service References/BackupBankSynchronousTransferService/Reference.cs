﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VideoStore.Business.Components.BackupBankSynchronousTransferService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BackupBankSynchronousTransferService.IBackupBankSynchronousTransferService")]
    public interface IBackupBankSynchronousTransferService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBackupBankSynchronousTransferService/Transfer", ReplyAction="http://tempuri.org/IBackupBankSynchronousTransferService/TransferResponse")]
        [System.ServiceModel.TransactionFlowAttribute(System.ServiceModel.TransactionFlowOption.Allowed)]
        void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBackupBankSynchronousTransferServiceChannel : VideoStore.Business.Components.BackupBankSynchronousTransferService.IBackupBankSynchronousTransferService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BackupBankSynchronousTransferServiceClient : System.ServiceModel.ClientBase<VideoStore.Business.Components.BackupBankSynchronousTransferService.IBackupBankSynchronousTransferService>, VideoStore.Business.Components.BackupBankSynchronousTransferService.IBackupBankSynchronousTransferService {
        
        public BackupBankSynchronousTransferServiceClient() {
        }
        
        public BackupBankSynchronousTransferServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BackupBankSynchronousTransferServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BackupBankSynchronousTransferServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BackupBankSynchronousTransferServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void Transfer(double pAmount, int pFromAcctNumber, int pToAcctNumber) {
            base.Channel.Transfer(pAmount, pFromAcctNumber, pToAcctNumber);
        }
    }
}
