using Microsoft.AspNetCore.Mvc;
using Common.Entities;
using Common.Data;
using LinqToDB;

namespace LimedikaMVC.Controllers
{
    public class ClientsController : Controller
    {
        private readonly LimedikaDataConnection _connection;

        public ClientsController(LimedikaDataConnection connection)
        {
            _connection = connection;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
              return View(await _connection.Clients.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _connection.Clients == null)
            {
                return NotFound();
            }

            var client = await _connection.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
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
                await _connection.InsertAsync(client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _connection.Clients == null)
            {
                return NotFound();
            }

            var client = await _connection.Clients.SingleOrDefaultAsync(client => client.Id == id);
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
                    await _connection.UpdateAsync(client);
                }
                catch (Exception)
                {
                    if (!ClientExists(client.Id))
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
            if (id == null || _connection.Clients == null)
            {
                return NotFound();
            }

            var client = await _connection.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_connection.Clients == null)
            {
                return Problem("Entity set 'LimedikaDataContext.Clients'  is null.");
            }
            await _connection.Clients.Where(client => client.Id == id).DeleteAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(Guid id)
        {
          return _connection.Clients.Any(e => e.Id == id);
        }
    }
}
