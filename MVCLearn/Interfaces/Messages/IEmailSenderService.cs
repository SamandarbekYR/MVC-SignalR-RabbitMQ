using MVCLearn.DTOs;

namespace MVCLearn.Interfaces.Messages
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(MessageDto messageDto);
    }
}
