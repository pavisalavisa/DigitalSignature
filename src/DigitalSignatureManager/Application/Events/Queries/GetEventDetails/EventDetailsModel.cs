using System;
using Domain.Enums;

namespace Application.Events.Queries.GetEventDetails
{
    public class EventDetailsModel
    {
        public int Id { get; set; }
        public EventType Type { get; set; }
        public string InputDocumentName { get; set; }
        public string InputDocumentB64 { get; set; }
        public string OutpuDocumentB64 { get; set; }
        public bool IsSuccessful { get; set; }
        public string Error { get; set; }
        public DateTime Created { get; set; }
    }
}