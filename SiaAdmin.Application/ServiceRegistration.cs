using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation; 
using SiaAdmin.Application.Validators.Survey;
using Microsoft.AspNetCore.Identity;
using SiaAdmin.Application.Validators.SurveyAssigned;
using SiaAdmin.Domain.Entities.Models;
using SiaAdmin.Application.Behaviours;

namespace SiaAdmin.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection collection)
        {
            collection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            collection.AddAutoMapper(Assembly.GetExecutingAssembly());
            collection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            collection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}
