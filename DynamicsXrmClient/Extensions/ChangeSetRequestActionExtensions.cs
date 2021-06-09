using DynamicsXrmClient.Batches;
using System;
using System.Net.Http;

namespace DynamicsXrmClient.Extensions
{
    internal static class ChangeSetRequestActionExtensions
    {
        internal static (HttpMethod, string) Resolve<T>(this ChangeSetRequestAction changeSetRequestAction, T row)
        {
            return changeSetRequestAction switch
            {
                ChangeSetRequestAction.Create => (HttpMethod.Post, row.GetLogicalCollectionName()),
                ChangeSetRequestAction.Update => (HttpMethod.Patch, $"{row.GetLogicalCollectionName()}({row.GetDataverseRowId()})"),
                ChangeSetRequestAction.Upsert => (HttpMethod.Patch, $"{row.GetLogicalCollectionName()}({row.GetDataverseRowId()})"),
                ChangeSetRequestAction.Delete => (HttpMethod.Delete, $"{row.GetLogicalCollectionName()}({row.GetDataverseRowId()})"),

                _ => throw new NotImplementedException()
            };
        }
    }
}
