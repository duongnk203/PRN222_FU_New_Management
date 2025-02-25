using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222_Assignment_01.Repositories;

namespace PRN222_Assignment_01.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly IReportRepository _reportRepository;
        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public IActionResult Report()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateReport(DateTime startDate, DateTime endDate)
        {
            var reportData = _reportRepository.GenerateReport(startDate, endDate);
            return View("Report", reportData);
        }
    }
}
