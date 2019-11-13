using Kendo.Mvc.UI;
using ResearchApp.Models;
using ResearchApp.ViewModel;
using System.Threading.Tasks;

namespace ResearchApp.Data
{
    public interface IPublisherRepository : IGenericRepository<Publisher>
    {
        Task<DataSourceResult> GetPublishers(DataSourceRequest request);
        Task<int> CreatePublisher(PublisherViewModel model);
        Task UpdatePublisher(PublisherViewModel model);
    }
}
