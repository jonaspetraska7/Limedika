using Microsoft.AspNetCore.Mvc;
using Common.Entities;
using Common.Interfaces;
using X.PagedList;

namespace LimedikaMVC.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IClientPageService _clientPageService;
        private readonly IBufferedFileUploadService _bufferedFileUploadService;

        public ClientsController(IClientService clientService, 
            IClientPageService clientPageService, 
            IBufferedFileUploadService bufferedFileUploadService)
        {
            _clientService = clientService;
            _clientPageService = clientPageService;
            _bufferedFileUploadService = bufferedFileUploadService;
        }

        // GET: Clients
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.AddressSortParm = string.IsNullOrEmpty(sortOrder) ? "address_desc" : "";
            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.PostCodeSortParm = sortOrder == "postcode" ? "poscode_dec" : "postcode";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var logs = (await _clientService.GetClients()).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                logs = logs.Where(s => s.Address.Contains(searchString) || 
                    s.Name.Contains(searchString) || 
                    s.PostCode.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "address_desc":
                    logs = logs.OrderByDescending(s => s.Address);
                    break;
                case "name":
                    logs = logs.OrderBy(s => s.Name);
                    break;
                case "name_desc":
                    logs = logs.OrderByDescending(s => s.Name);
                    break;
                case "postcode":
                    logs = logs.OrderBy(s => s.PostCode);
                    break;
                case "postcode_desc":
                    logs = logs.OrderByDescending(s => s.PostCode);
                    break;
                default:
                    logs = logs.OrderBy(s => s.Address);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(logs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || await _clientService.GetClients() == null)
            {
                return NotFound();
            }

            var client = await _clientService.GetClient(id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,PostCode")] Client client)
        {
            if (ModelState.IsValid)
            {
                client.Id = Guid.NewGuid();
                await _clientService.InsertClient(client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || await _clientService.GetClients() == null)
            {
                return NotFound();
            }

            var client = await _clientService.GetClient(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Address,PostCode")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _clientService.UpdateClient(client);
                }
                catch (Exception)
                {
                    if (! await ClientExistsAsync(client.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || await _clientService.GetClients() == null)
            {
                return NotFound();
            }

            var client = await _clientService.GetClient(id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (await _clientService.GetClients() == null)
            {
                return Problem("Entity set 'LimedikaDataConnection.Clients'  is null.");
            }
            await _clientService.DeleteClient(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> Upload(IFormFile file)
        {
            try
            {
                var clients = await _bufferedFileUploadService.UploadFile(file);

                if (clients != null)
                {
                    await _clientPageService.ImportClients(clients);
                    ViewBag.Message = "File Upload Successful";
                }
                else
                {
                    ViewBag.Message = "File Upload Failed";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"File Upload Failed : {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdatePostCodes()
        {
            await _clientPageService.UpdatePostCodes();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ClientExistsAsync(Guid id)
        {
            var clients = await _clientService.GetClients();
            return clients.Any(e => e.Id == id);
        }
    }
}
