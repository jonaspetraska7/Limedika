using Microsoft.AspNetCore.Mvc;
using Common.Entities;
using Common.Interfaces;

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
        public async Task<IActionResult> Index()
        {
            try
            {
                return View((await _clientService.GetClients()).OrderBy(x => Convert.ToInt32(x.Name.Split(". ")[2])));
            }
            catch (Exception)
            {
                return View(await _clientService.GetClients());
            } 
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
        public async Task<ActionResult> Index(IFormFile file)
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

            return View(await _clientService.GetClients());
        }

        public async Task<IActionResult> UpdatePostCodes()
        {
            await _clientPageService.UpdatePostCodes();
            return View();
        }

        private async Task<bool> ClientExistsAsync(Guid id)
        {
            var clients = await _clientService.GetClients();
            return clients.Any(e => e.Id == id);
        }
    }
}
