namespace WebPage.DesafioFullStack.Models
{
    public class DeviceCommunicationPageModel
    {
        public List<DeviceSelector> SelectedDevices { get; set; }
        public List<CommandSelector> CommandsList { get; set; }
        public string DeviceDataSerialized { get; set; }
        public string ChartLabelsSerialized { get; set; }
        public string SelectedCommand { get; set; }
        public string ResultCommandSender { get; set; }
        public string RefererLastPage { get; set; }
    }
}
