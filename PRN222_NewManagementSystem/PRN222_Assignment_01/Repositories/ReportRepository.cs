
using PRN222_Assignment_01.Models;

namespace PRN222_Assignment_01.Repositories
{
    public interface IReportRepository
    {
        List<NewsArticle> GenerateReport(DateTime startDate, DateTime endDate);
    }
    public class ReportRepository : IReportRepository
    {
        private readonly FUNewsManagementContext _context;
        public ReportRepository(FUNewsManagementContext context)
        {
            _context = context;
        }
        public List<NewsArticle> GenerateReport(DateTime startDate, DateTime endDate)
        {
            
                return _context.NewsArticles
                              .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                              .OrderByDescending(n => n.CreatedDate)
                              .ToList();
            
        }
    }
}
