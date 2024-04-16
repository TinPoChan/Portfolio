using BlazorApp2.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp2.Services
{
    public interface IPortfolioService
    {
        Task<List<PortfolioItem>> GetLatestPortfolioItemsAsync(int count);
    }
}
