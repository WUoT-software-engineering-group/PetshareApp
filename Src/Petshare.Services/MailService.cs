using Petshare.CrossCutting.Enums;
using Petshare.Services.Abstract;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Petshare.Services;

public class MailService : IMailService
{
    private readonly string _apiKey;
    private readonly string _mail;
    private readonly string _mailName;

    private readonly Dictionary<string, string> _mailTemplates = new()
    {
        { "application-rejected", "d-2cfce0d4db3b4e569d6a879c9ecf9413"},
        { "application-accepted", "d-14a722b7091e401dad3bd32897a30489"}
    };

    public MailService(string apiKey, string mail, string mailName)
    {
        _apiKey = apiKey;
        _mail = mail;
        _mailName = mailName;
    }

    public async Task<Response?> SendApplicationDecisionMail(string userFullName, string userEmail, ApplicationStatus applicationStatus, string petName,
        string petBreed)
    {
        var templateData = new
        {
            name = userFullName,
            pet_name = petName,
            pet_breed = petBreed,
        };

        var templateId = applicationStatus switch
        {
            ApplicationStatus.Accepted => _mailTemplates["application-accepted"],
            ApplicationStatus.Rejected => _mailTemplates["application-rejected"],
            _ => throw new NotImplementedException()
        };

        return await SendMail(templateId, userEmail, userFullName, templateData);
    }

    private async Task<Response?> SendMail(string templateId, string userEmail, string userFullName,
        object templateData)
    {
        var client = new SendGridClient(_apiKey);
        var from = new EmailAddress(_mail, _mailName);
        var to = new EmailAddress(userEmail, userFullName);
        var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, templateData);
        return await client.SendEmailAsync(msg);
    }   
}