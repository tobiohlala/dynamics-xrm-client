namespace DynamicsXrmClient
{
    public class DynamicsXrmConnectionParams
    {
        public string TenantId { get; set; }

        public string ServiceRootUri { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { internal get; set; }
    }
}
