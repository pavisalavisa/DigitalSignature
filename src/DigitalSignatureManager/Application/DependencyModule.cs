using Application.Certificates.Queries;
using Application.Events.Queries.GetEventDetails;
using Application.Events.Queries.GetPersonalEvents;
using Application.Signature.Commands.DownloadSignedPdf;
using Application.Signature.Commands.SignBinary;
using Application.Signature.Commands.SignPdf;
using Application.Signature.Queries.GetSignatureServiceHealth;
using Application.Users.Commands.AssignCertificate;
using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.RegisterUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.GetPersonalInformation;
using Application.Users.Queries.GetUserById;
using Application.Verification.Commands.VerifyBinary;
using Application.Verification.Commands.VerifyPdf;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            AddUserServices(services);
            AddCertificateServices(services);
            AddSignatureServices(services);
            AddEventServices(services);

            return services;
        }

        private static void AddUserServices(IServiceCollection services)
        {
            services.AddScoped<IRegisterUserCommand, RegisterUserCommand>();
            services.AddScoped<IGetAllUsersQuery, GetAllUsersQuery>();
            services.AddScoped<IGetUserByIdQuery, GetUserByIdQuery>();
            services.AddScoped<IUpdateUserCommand, UpdateUserCommand>();
            services.AddScoped<IDeleteUserCommand, DeleteUserCommand>();
            services.AddScoped<IAssignCertificateCommand, AssignCertificateCommand>();
            services.AddScoped<IGetPersonalInformationQuery, GetPersonalInformationQuery>();
        }

        private static void AddCertificateServices(IServiceCollection services)
        {
            services.AddScoped<IGetAllCertificatesQuery, GetAllCertificatesQuery>();
        }

        private static void AddSignatureServices(IServiceCollection services)
        {
            services.AddScoped<IGetSignatureServiceHealth, GetSignatureServiceHealth>();
            services.AddScoped<ISignPdfCommand, SignPdfCommand>();
            services.AddScoped<IDownloadSignedPdfCommand, DownloadSignedPdfCommand>();
            services.AddScoped<ISignBinaryCommand, SignBinaryCommand>();
            services.AddScoped<IVerifyPdfCommand, VerifyPdfCommand>();
            services.AddScoped<IVerifyBinaryCommand, VerifyBinaryCommand>();
        }

        private static void AddEventServices(IServiceCollection services)
        {
            services.AddScoped<IGetPersonalEventsQuery, GetPersonalEventsQuery>();
            services.AddScoped<IGetEventDetailsQuery, GetEventDetailsQuery>();
        }
    }
}