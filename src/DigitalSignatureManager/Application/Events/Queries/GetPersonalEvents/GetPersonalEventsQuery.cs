using System;
using System.Linq.Expressions;
using Application.Common;
using Application.Common.Base.Queries;
using Application.Common.Contracts;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Events.Queries.GetPersonalEvents
{
    public class GetPersonalEventsQuery : BaseFilterQuery<Event, PersonalEventModel, FilterPagingQueryModel>,
        IGetPersonalEventsQuery
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetPersonalEventsQuery(
            ILogger<BaseFilterQuery<Event, PersonalEventModel, FilterPagingQueryModel>> logger,
            IDigitalSignatureManagerDbContext context, IHttpContextAccessor httpContextAccessor) : base(logger, context)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Expression<Func<Event, bool>> GetFilterExpression(FilterPagingQueryModel filterModel)
        {
            var userId = _httpContextAccessor.GetUserIdFromClaims();

            return x => x.TriggeredById == userId;
        }

        protected override Expression<Func<Event, PersonalEventModel>> GetMappingExpression() =>
            x => new PersonalEventModel
            {
                Created = x.Created,
                Error = x.Error,
                Type = x.Type,
                IsSuccessful = x.IsSuccessful,
                InputDocumentB64 = x.InputDocumentB64,
                InputDocumentName = x.InputDocumentName,
                OutputDocumentB64 = x.OutputDocumentB64
            };
    }
}