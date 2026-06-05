namespace SmartTourism360.Infrastructure.Storage
{
    public class MinIoOptions
    {
        public const string MinIo = "MinIo";

        public string Endpoint { get; set; } = null!;
        public string? PublicEndpoint { get; set; }
        public string AccessKey { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
        public string BucketName { get; set; } = null!;
        public bool Secure { get; set; }
    }
}
