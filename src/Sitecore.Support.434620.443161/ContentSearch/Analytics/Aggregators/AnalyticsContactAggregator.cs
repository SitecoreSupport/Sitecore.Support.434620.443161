namespace Sitecore.Support.ContentSearch.Analytics.Aggregators
{
  using System.Collections.Generic;
  using Sitecore.Analytics.Aggregation.Pipeline;
  using Sitecore.ContentSearch.Analytics.Aggregators;
  using Sitecore.ContentSearch.Analytics.Models;
  using Sitecore.Diagnostics;

  public class AnalyticsContactAggregator : ObservableAggregator<IContactIndexable>
  {
    [UsedImplicitly]
    public AnalyticsContactAggregator([NotNull] string name) : base(name)
    {
      Assert.ArgumentNotNull(name, nameof(name));
    }

    protected override IEnumerable<IContactIndexable> ResolveIndexables(AggregationPipelineArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      var context = args.Context;
      Assert.IsNotNull(context, nameof(context));

      if (context.Contact == null)
      {
        yield break;
      }

      yield return new Sitecore.Support.ContentSearch.Analytics.Models.ContactIndexable(context);
    }
  }
}
