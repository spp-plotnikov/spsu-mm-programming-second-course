﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client.ServiceReference18 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference18.IService1")]
    public interface IService1 {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/work", ReplyAction="http://tempuri.org/IService1/workResponse")]
        void work();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/work", ReplyAction="http://tempuri.org/IService1/workResponse")]
        System.Threading.Tasks.Task workAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetListOfFilters", ReplyAction="http://tempuri.org/IService1/GetListOfFiltersResponse")]
        string[] GetListOfFilters();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetListOfFilters", ReplyAction="http://tempuri.org/IService1/GetListOfFiltersResponse")]
        System.Threading.Tasks.Task<string[]> GetListOfFiltersAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/ApplyFilter", ReplyAction="http://tempuri.org/IService1/ApplyFilterResponse")]
        System.Drawing.Bitmap ApplyFilter(System.Drawing.Bitmap image, string nameOfFilter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/ApplyFilter", ReplyAction="http://tempuri.org/IService1/ApplyFilterResponse")]
        System.Threading.Tasks.Task<System.Drawing.Bitmap> ApplyFilterAsync(System.Drawing.Bitmap image, string nameOfFilter);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetProgress", ReplyAction="http://tempuri.org/IService1/GetProgressResponse")]
        int GetProgress();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetProgress", ReplyAction="http://tempuri.org/IService1/GetProgressResponse")]
        System.Threading.Tasks.Task<int> GetProgressAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/Stop", ReplyAction="http://tempuri.org/IService1/StopResponse")]
        void Stop();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/Stop", ReplyAction="http://tempuri.org/IService1/StopResponse")]
        System.Threading.Tasks.Task StopAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IService1Channel : Client.ServiceReference18.IService1, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Service1Client : System.ServiceModel.ClientBase<Client.ServiceReference18.IService1>, Client.ServiceReference18.IService1 {
        
        public Service1Client() {
        }
        
        public Service1Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Service1Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void work() {
            base.Channel.work();
        }
        
        public System.Threading.Tasks.Task workAsync() {
            return base.Channel.workAsync();
        }
        
        public string[] GetListOfFilters() {
            return base.Channel.GetListOfFilters();
        }
        
        public System.Threading.Tasks.Task<string[]> GetListOfFiltersAsync() {
            return base.Channel.GetListOfFiltersAsync();
        }
        
        public System.Drawing.Bitmap ApplyFilter(System.Drawing.Bitmap image, string nameOfFilter) {
            return base.Channel.ApplyFilter(image, nameOfFilter);
        }
        
        public System.Threading.Tasks.Task<System.Drawing.Bitmap> ApplyFilterAsync(System.Drawing.Bitmap image, string nameOfFilter) {
            return base.Channel.ApplyFilterAsync(image, nameOfFilter);
        }
        
        public int GetProgress() {
            return base.Channel.GetProgress();
        }
        
        public System.Threading.Tasks.Task<int> GetProgressAsync() {
            return base.Channel.GetProgressAsync();
        }
        
        public void Stop() {
            base.Channel.Stop();
        }
        
        public System.Threading.Tasks.Task StopAsync() {
            return base.Channel.StopAsync();
        }
    }
}
