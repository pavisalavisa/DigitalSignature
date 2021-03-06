using System;
using Domain.Enums;

namespace Application.Events.Queries.GetPersonalEvents
{
    public class PersonalEventModel
    {
        public int Id { get; set; }
        public EventType Type { get; set; }
        public string InputDocumentName { get; set; }
        public bool IsSuccessful { get; set; }
        public string Error { get; set; }
        public DateTime Created { get; set; }
    }
}