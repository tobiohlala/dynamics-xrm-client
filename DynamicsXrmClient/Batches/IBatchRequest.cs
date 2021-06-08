namespace DynamicsXrmClient.Batches
{
    public interface IBatchRequest : IXRMBatchAsyncComposable
    {
        public string Id { get; set; }
    }
}
