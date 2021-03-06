﻿using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        public PublisherRepository(SifterContext dbContext, IMemoryCache memoryCache)
        : base(dbContext, memoryCache)
        {

        }

        public async Task<DataSourceResult> GetPublishers(DataSourceRequest request)
        {
            DataSourceResult list = await GetAll().ToDataSourceResultAsync(request);
            var publishers = (IEnumerable<Publisher>)list.Data;
            var result = publishers.Select(x => new PublisherViewModel
            {
                PublisherID = x.PublisherID,
                Name = x.Name
            }).AsEnumerable();
            list.Data = result;
            return list;
        }

        public async Task<int> CreatePublisher(PublisherViewModel model, bool updateForm = false)
        {
            var newPublisher = new Publisher
            {
                Name = model.Name
            };
            await Create(newPublisher);
            return newPublisher.PublisherID;
        }
        public async Task UpdatePublisher(PublisherViewModel model, bool updateForm = false)
        {
            var dbPublisher = await GetAll().Where(x => x.PublisherID == model.PublisherID).FirstOrDefaultAsync();
            if (dbPublisher != null)
            {
                dbPublisher.Name = model.Name;
                await Update(dbPublisher);
            }
        }
    }
}
