using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.RegisterUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.GetUserById;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //Users
            services.AddScoped<IRegisterUserCommand, RegisterUserCommand>();
            services.AddScoped<IGetAllUsersQuery, GetAllUsersQuery>();
            services.AddScoped<IGetUserByIdQuery, GetUserByIdQuery>();
            services.AddScoped<IUpdateUserCommand, UpdateUserCommand>();
            services.AddScoped<IDeleteUserCommand, DeleteUserCommand>();

            return services;
        }
    }
}