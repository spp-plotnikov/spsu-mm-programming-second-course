﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client.MyServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MyServiceReference.IService", CallbackContract=typeof(Client.MyServiceReference.IServiceCallback))]
    public interface IService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetFilters", ReplyAction="http://tempuri.org/IService/GetFiltersResponse")]
        string[] GetFilters();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetFilters", ReplyAction="http://tempuri.org/IService/GetFiltersResponse")]
        System.Threading.Tasks.Task<string[]> GetFiltersAsync();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IService/GetPicture")]
        void GetPicture(System.Drawing.Bitmap map, string filter);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IService/GetPicture")]
        System.Threading.Tasks.Task GetPictureAsync(System.Drawing.Bitmap map, string filter);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IService/StopWork")]
        void StopWork();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IService/StopWork")]
        System.Threading.Tasks.Task StopWorkAsync();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IService/ProgressPerSecond")]
        void ProgressPerSecond();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IService/ProgressPerSecond")]
        System.Threading.Tasks.Task ProgressPerSecondAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IService/SendResult")]
        void SendResult(byte[] res);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IService/SendProgress")]
        void SendProgress(float progr);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChannel : Client.MyServiceReference.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.DuplexClientBase<Client.MyServiceReference.IService>, Client.MyServiceReference.IService {
        
        public ServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public string[] GetFilters() {
            return base.Channel.GetFilters();
        }
        
        public System.Threading.Tasks.Task<string[]> GetFiltersAsync() {
            return base.Channel.GetFiltersAsync();
        }
        
        public void GetPicture(System.Drawing.Bitmap map, string filter) {
            base.Channel.GetPicture(map, filter);
        }
        
        public System.Threading.Tasks.Task GetPictureAsync(System.Drawing.Bitmap map, string filter) {
            return base.Channel.GetPictureAsync(map, filter);
        }
        
        public void StopWork() {
            base.Channel.StopWork();
        }
        
        public System.Threading.Tasks.Task StopWorkAsync() {
            return base.Channel.StopWorkAsync();
        }
        
        public void ProgressPerSecond() {
            base.Channel.ProgressPerSecond();
        }
        
        public System.Threading.Tasks.Task ProgressPerSecondAsync() {
            return base.Channel.ProgressPerSecondAsync();
        }
    }
}
