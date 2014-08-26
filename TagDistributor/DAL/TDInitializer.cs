using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TagDistributor.DAL
{
    public class TDInitializer : DropCreateDatabaseIfModelChanges<TDContext>
    {
        protected override void Seed(TDContext context)
        {
            var tagDistributs = new List<Models.TagDistribut>
            {
                new Models.TagDistribut{ BeginID=100001, EndID=104020, Username="dataCenter", DistributDate=DateTime.Parse("2014-08-20") },
                new Models.TagDistribut{ BeginID=125800, EndID=126019, Username="dataCenter", DistributDate=DateTime.Parse("2014-08-20") },
                new Models.TagDistribut{ BeginID=600000, EndID=600085, Username="dataCenter", DistributDate=DateTime.Parse("2014-08-20") },
                new Models.TagDistribut{ BeginID=1000000, EndID=1024100, Username="dataCenter", DistributDate=DateTime.Parse("2014-08-20") },
                new Models.TagDistribut{ BeginID=6000002, EndID=6000007, Username="dataCenter", DistributDate=DateTime.Parse("2014-08-20") }
            };

            tagDistributs.ForEach(s => context.TagDistributs.Add(s));
            context.SaveChanges();
        }
    }
}