using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public class PublisherRepository : GenericRepository<Publisher>, IPublisherRepository
    {
        public PublisherRepository(SifterContext dbContext)
        : base(dbContext)
        {

        }

        public async Task<DataSourceResult> GetPublishers(DataSourceRequest request)
        {
            DataSourceResult list = await GetAll().ToDataSourceResultAsync(request);
            var publishers = (IEnumerable<Publisher>)list.Data;
            var result = publishers.Select(x => new PublisherViewModel
            {
                PublisherID = x.PublisherId,
                Name = x.Name
            }).AsEnumerable();
            list.Data = result;
            return list;
        }

        public async Task<int> CreatePublisher(PublisherViewModel model)
        {
            //int newPublisherId = await GetAll().MaxAsync(x => x.PublisherID);
            //newPublisherId++;
            var newPublisher = new Publisher
            {
                //PublisherID = newPublisherId,
                Name = model.Name
            };
            await Create(newPublisher);
            return newPublisher.PublisherId;
        }
        public async Task UpdatePublisher(PublisherViewModel model)
        {
            var dbPublisher = await GetAll().Where(x => x.PublisherId == model.PublisherID).FirstOrDefaultAsync();
            if (dbPublisher != null)
            {
                //dbPublisher.PublisherID = model.PublisherID;
                dbPublisher.Name = model.Name;
                await Update(dbPublisher);
            }
        }
    }
}
