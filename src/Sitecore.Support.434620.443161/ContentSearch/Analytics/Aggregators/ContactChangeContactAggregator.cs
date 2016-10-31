namespace Sitecore.Support.ContentSearch.Analytics.Aggregators
{
  using System;
  using System.Collections.Generic;
  using Sitecore.Analytics.Aggregation.Pipelines.ContactProcessing;
  using Sitecore.ContentSearch;
  using Sitecore.ContentSearch.Analytics.Aggregators;
  using Sitecore.ContentSearch.Analytics.Models;
  using Sitecore.Diagnostics;

  public class ContactChangeContactAggregator : ContactChangeProcessor<IContactIndexable>
  {
    [UsedImplicitly]
    public ContactChangeContactAggregator([NotNull] string name) : base(name)
    {
      Assert.ArgumentNotNull(name, nameof(name));
    }

    protected override IEnumerable<IContactIndexable> ResolveIndexables(ContactProcessingArgs args)
    {                                         
      Assert.ArgumentNotNull(args, nameof(args));
                                                                                 
      var changeEventReason = GetChangeEventReason(args);

      var contact = args.GetContact();
      if (contact != null)
      {
        var indexable = new Sitecore.Support.ContentSearch.Analytics.Models.ContactIndexable(contact);

        yield return new ContactChangeIndexable(indexable, changeEventReason);
      }
      else
      {
        var id = new IndexableUniqueId<Guid>(args.ContactId.Guid);

        yield return new ContactChangeIndexable(id, changeEventReason);
      }                           
    }
  }
}
