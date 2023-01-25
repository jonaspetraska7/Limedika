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
        public async Task<IActionResult> Index()
        {
              return View((await _logService.GetLogs()).OrderBy(x => x.TimeStamp));
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
