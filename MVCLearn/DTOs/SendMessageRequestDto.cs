namespace MVCLearn.DTOs
{
    public class SendMessageRequestDto
    {
        public string messageBody { get; set; } = string.Empty;
        public List<UserMessageDto> users { get; set; }
    }
}
