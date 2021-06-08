using DynamicsXrmClient.Batches;
using System;
using System.Net.Http;

namespace DynamicsXrmClient.Extensions
{
    internal static class ChangeSetRequestActionExtensions
    {
        internal static (HttpMethod, string) Resolve(this ChangeSetRequestAction changeSetRequestAction, IXRMEntity entity)
        {
            return changeSetRequestAction switch
            {
                ChangeSetRequestAction.Create => (HttpMethod.Post, entity.GetType().GetLogicalCollectionName()),
                ChangeSetRequestAction.Update => (HttpMethod.Patch, $"{entity.GetType().GetLogicalCollectionName()}({entity.Id})"),
                ChangeSetRequestAction.Upsert => (HttpMethod.Patch, $"{entity.GetType().GetLogicalCollectionName()}({entity.Id})"),
                ChangeSetRequestAction.Delete => (HttpMethod.Delete, $"{entity.GetType().GetLogicalCollectionName()}({entity.Id})"),

                _ => throw new NotImplementedException()
            };
        }
    }
}
