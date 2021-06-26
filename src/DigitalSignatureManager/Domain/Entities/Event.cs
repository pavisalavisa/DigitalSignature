using Domain.Common;
using Domain.Common.Base;
using Domain.Enums;

namespace Domain.Entities
{
    public class Event : AuditableEntity
    {
        public EventType Type { get; set; }
        public int TriggeredById { get; set; }
        public string InputDocumentB64 { get; set; }
        public string OutputDocumentB64 { get; set; }
        public bool IsSuccessful { get; set; }
        public string Error { get; set; }
        public virtual ApplicationUser TriggeredBy { get; set; }
    }
}