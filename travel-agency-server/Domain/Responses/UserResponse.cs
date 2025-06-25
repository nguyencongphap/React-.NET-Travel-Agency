namespace Domain.Responses
{
    public record UserResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required DateTime DateJoined { get; set; }
        public required int ItineraryCreated { get; set; }
        public List<string> Roles { get; set; }
    }
}
