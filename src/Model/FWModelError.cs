namespace BootWrapper.BW.Model
{
    public class BWModelError : BWBaseModel
    {
        public string Title { get; set; }
        public string FriendlyMessage { get; set; }
        public string DetailMessage { get; set; }        
        public string HandleErrorInfo { get; set; }        
    }
}