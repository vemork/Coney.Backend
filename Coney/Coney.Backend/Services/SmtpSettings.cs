namespace Coney.Backend.Services
{
    public class MailSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public string UrlFrontend { get; set; }

        public string From { get; set; }
        public string NameEs { get; set; }
        public string NameEn { get; set; }
        public string SubjectConfirmationEs { get; set; }
        public string SubjectConfirmationEn { get; set; }
        public string BodyConfirmationEs { get; set; }
        public string BodyConfirmationEn { get; set; }

        public string SubjectRecoveryEs { get; set; }
        public string SubjectRecoveryEn { get; set; }

        public string BodyRecoveryEs { get; set; }
        public string BodyRecoveryEn { get; set; }
    }
}