namespace MVCLearn.DTOs
{
    public class SendMessageRequestDto
    {
        public Guid SenderId { get; set; }
        public string messageBody { get; set; } = string.Empty;
        public List<UserMessageDto> users { get; set; }
    }
}
