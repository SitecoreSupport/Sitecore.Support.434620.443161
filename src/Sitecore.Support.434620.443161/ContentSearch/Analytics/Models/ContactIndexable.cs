namespace Sitecore.Support.ContentSearch.Analytics.Models
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using MongoDB.Driver.Builders;
  using Sitecore.Analytics.Aggregation.Data.Model;
  using Sitecore.Analytics.Data.DataAccess.MongoDb;
  using Sitecore.Analytics.Model;
  using Sitecore.Analytics.Model.Entities;
  using Sitecore.ContentSearch;
  using Sitecore.Diagnostics;

  public class ContactIndexable : Sitecore.ContentSearch.Analytics.Models.ContactIndexable
  {    
    public ContactIndexable([NotNull] IVisitAggregationContext context) : this(context.Contact)
    {
      Assert.ArgumentNotNull(context, nameof(context));
    }

    public ContactIndexable([NotNull] IContact contact) : base(contact)
    {
      Assert.ArgumentNotNull(contact, nameof(contact));

      var query = Query.And(Query.EQ("ContactId", contact.Id.Guid));
      Assert.IsNotNull(query, nameof(query));

      var driver = MongoDbDriver.FromConnectionString("analytics");
      Assert.IsNotNull(driver, nameof(driver));

      var interactions = driver.Interactions;
      Assert.IsNotNull(interactions, nameof(interactions));

      var visitData = interactions.FindAs<VisitData>(query);
      Assert.IsNotNull(visitData, nameof(visitData));

      var visits = visitData.ToList();

      visits.Sort((x, y) => DateTime.Compare(y.StartDateTime, x.StartDateTime));

      var interaction = visits.FirstOrDefault();
      Assert.IsNotNull(interaction, nameof(interaction));

      var fields = Fields as List<IIndexableDataField>;
      Assert.IsNotNull(fields, nameof(fields));

      fields.Add(new IndexableDataField<DateTime>("contact.LatestVisitDate", interaction.StartDateTime));
    }     
  }
}

