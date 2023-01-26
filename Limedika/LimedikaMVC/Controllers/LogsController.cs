using Microsoft.AspNetCore.Mvc;
using Common.Entities;
using LinqToDB;
using Common.Interfaces;

namespace LimedikaMVC.Controllers
{
    public class LogsController : Controller
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        // GET: Logs
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewBag.TimeStampSortParm = string.IsNullOrEmpty(sortOrder) ? "time_desc" : "";
            ViewBag.UserActionSortParm = sortOrder == "action" ? "action_desc" : "action";

            var logs = (await _logService.GetLogs()).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                logs = logs.Where(s => s.TimeStamp.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "time_desc":
                    logs = logs.OrderByDescending(s => s.TimeStamp);
                    break;
                case "action":
                    logs = logs.OrderBy(s => s.UserAction);
                    break;
                case "action_desc":
                    logs = logs.OrderByDescending(s => s.UserAction);
                    break;
                default:
                    logs = logs.OrderBy(s => s.TimeStamp);
                    break;
            }

            return View(logs.ToList());
        }

        // GET: Logs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Logs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TimeStamp,UserAction")] Log log)
        {
            if (ModelState.IsValid)
            {
                log.Id = Guid.NewGuid();
                await _logService.InsertLog(log);
                return RedirectToAction(nameof(Index));
            }
            return View(log);
        }
    }
}
