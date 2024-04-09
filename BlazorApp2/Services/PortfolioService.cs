using BlazorApp2.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp2.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly ApplicationDbContext _context;

        public PortfolioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PortfolioItem>> GetLatestPortfolioItemsAsync(int count)
        {
            // Assuming there's a DateCreated or similar field to sort by
            return await _context.PortfolioItems
                                 .OrderByDescending(p => p.DateCreated)
                                 .Take(count)
                                 .ToListAsync();
        }
    }
}
