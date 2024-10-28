namespace CodersCupAward.ViewModels
{
    public class MessageEmailViewModel
    {
        public string? EmailFromAddress { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? EmailTo { get; set; }
        public string? EmailCc { get; set; }
        public string? ReplyTo { get; set; }
        public string? AttachmentPath { get; set; }
        public string? AttachmentName { get; set; }
    }
}
