namespace DynamicsXrmClient.Batches
{
    public interface IBatchRequest : IDynamicsXRMBatchAsyncComposable
    {
        public string Id { get; set; }
    }
}
