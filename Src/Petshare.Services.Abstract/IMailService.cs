using Petshare.CrossCutting.Enums;
using SendGrid;

namespace Petshare.Services.Abstract;

public interface IMailService
{
    Task<Response?> SendApplicationDecisionMail(string userFullName, string userEmail, ApplicationStatus applicationStatus, string petName, string petBreed);
}