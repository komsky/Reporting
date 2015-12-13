using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ImgTec.Web.Models;
using ImgTec.Web.Models.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ImgTec.Web.Controllers
{
    [Authorize(Roles = "Customers, Agents, Admins")]
    public partial class CasesController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _manager;
        public CasesController()
        {
            _context = new ApplicationDbContext();
            _manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }

        public CasesController(ApplicationDbContext context, UserManager<ApplicationUser> manager)
        {
            _context = context;
            _manager = manager;
        }


        public virtual async Task<ActionResult> Index()
        {
            if (User.IsInRole("Agents") || User.IsInRole("Admins"))
            {
                return View(_context.Cases.Include(x => x.Customer).ToList());
            }
            else
            {
                //2) Customer users shall be able to view their own company's cases
                var userId = User.Identity.GetUserId();
                var currentUser = await _manager.FindByIdAsync(userId);
                ViewBag.CurrentUserId = currentUser.Id;
                return View(_context.Cases.Include(x => x.Customer).Where(x => x.Customer.Company == currentUser.Company).ToList());
            }
        }

        public virtual ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = _context.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        // GET: Cases/Create
        public virtual ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Create([Bind(Include = "Id,Title,CaseState,Description,CasePriority,AgentReply")] Case @case)
        {
            var currentUser = await _manager.FindByIdAsync(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                @case.Customer = currentUser;
                _context.Cases.Add(@case);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(@case);
        }

        [Authorize(Roles = "Agents, Admins")]
        // GET: Cases/Edit/5
        public virtual ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = _context.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        [Authorize(Roles = "Agents, Admins")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit([Bind(Include = "Id,Title, Description, CaseState,AgentReply")] Case @case)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(@case.AgentReply))
                {
                    ModelState.AddModelError("AgentReply", "Please add reply to this case before saving");
                    return View(@case);
                }
                var currentCase = _context.Cases.First(x => x.Id == @case.Id);
                currentCase.AgentReply = @case.AgentReply;
                currentCase.CaseState = @case.CaseState;
                _context.Entry(currentCase).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    var reply = error;
                }
            }
            return View(@case);
        }

        [Authorize(Roles = "Agents, Admins")]
        public virtual ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = _context.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        [Authorize(Roles = "Agents, Admins")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual ActionResult DeleteConfirmed(int id)
        {
            Case @case = _context.Cases.Find(id);
            _context.Cases.Remove(@case);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public virtual async Task<ActionResult> Assign(int id)
        {
            var agentId = User.Identity.GetUserId();
            var currentAgent = await _manager.FindByIdAsync(agentId);
            var currentCase = _context.Cases.First(x => x.Id == id);
            currentCase.Agent = currentAgent;
            currentCase.CaseState = CaseState.Assigned;
            _context.Entry(currentCase).State = EntityState.Modified;
            var saveResult = await _context.SaveChangesAsync();

            return View(MVC.Cases.Views.Index, _context.Cases.Include(x => x.Customer).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
