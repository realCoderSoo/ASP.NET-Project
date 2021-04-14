using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab4.Data;
using Lab4.Models;
using Lab4.Models.ViewModels;

namespace Lab4.Controllers
{
    public class CommunitiesController : Controller
    {
        private readonly SchoolCommunityContext _context;

        public CommunitiesController(SchoolCommunityContext context)
        {
            _context = context;
        }

        // GET: Communities
        public async Task<IActionResult> Index(string ID)
        {
            var viewModel = new CommunityViewModel();
            viewModel.Community = await _context.Community
                  .Include(i => i.CommunityMembership)
                  .AsNoTracking()
                  .OrderBy(i => i.Title)
                  .ToListAsync();
            viewModel.Student = await _context.Student
                .Include(i => i.CommunityMembership)
                .AsNoTracking().OrderBy(i => i.FirstName)
                .ToListAsync();

            if (ID != null)
            {
                ViewData["CommunityID"] = ID;
                viewModel.CommunityMembership = viewModel.Community.Where(
                    x => x.ID.Equals(ID)).Single().CommunityMembership;

            }

            return View(viewModel);
        }

        // GET: Communities/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var community = await _context.Community
                .FirstOrDefaultAsync(m => m.ID == id);
            if (community == null)
            {
                return NotFound();
            }

            return View(community);
        }

        // GET: Communities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Communities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Budget")] Community community)
        {
            if (ModelState.IsValid)
            {
                _context.Add(community);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(community);
        }

        // GET: Communities/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var community = await _context.Community.FindAsync(id);
            if (community == null)
            {
                return NotFound();
            }
            return View(community);
        }

        // POST: Communities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Title,Budget")] Community community)
        {
            if (id != community.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(community);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommunityExists(community.ID))
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
            return View(community);
        }


        // GET: Communities/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var community = await _context.Community
                .FirstOrDefaultAsync(m => m.ID == id);
            if (community == null)
            {
                return NotFound();
            }

            return View(community);
        }

        // POST: Communities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var community = await _context.Community.FindAsync(id);
            _context.Community.Remove(community);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> EditMemberships(int ID)
        //{
        //    //viewModel.studentMembershipViewModel = await _context.StudentMembership.ToListAsync();
        //    var viewModel = new CommunityViewModel();
        //    viewModel.Community = await _context.Community
        //          .Include(i => i.CommunityMembership)
        //          .AsNoTracking()
        //          .OrderBy(i => i.Title)
        //          .ToListAsync();
        //    viewModel.Student = await _context.Student.ToListAsync();

        //    if (ID >= 0)
        //    {
        //        viewModel.EditStudent = _context.Student.SingleOrDefault(x => x.ID.Equals(ID));
        //        //viewModel.EditStudent.ID = ID;
        //    }
        //    //viewModel.EditStudent = await _context.Student.Where(b => b.ID.Equals(ID));

        //    return View(viewModel);
        //}

        //public async Task<IActionResult> AddMembership(int studentId, string communityId)
        //{
        //    var communityMemberShip = new CommunityMembership();
        //    communityMemberShip.StudentID = studentId;
        //    communityMemberShip.CommunityID = communityId;
        //    _context.CommunityMembership.Add(communityMemberShip);
        //    _context.SaveChanges();

        //    //return RedirectToAction(nameof(EditMemberships));
        //    //return View();
        //    return View();
        //}

        private bool CommunityExists(string id)
        {
            return _context.Community.Any(e => e.ID == id);
        }
    }
}
