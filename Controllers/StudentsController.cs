using Lab4.Data;
using Lab4.Models;
using Lab4.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolCommunityContext _context;

        public StudentsController(SchoolCommunityContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(int? ID)
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

            if (ID > 0)
            {
                ViewData["StudentID"] = ID;
                viewModel.CommunityMembership = viewModel.Student.Where(
                    x => x.ID.Equals(ID)).Single().CommunityMembership;

            }

            return View(viewModel);
            /*return View(await _context.Student.ToListAsync());*/
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LastName,FirstName,EnrollmentDate,FullName")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }


        public async Task<IActionResult> EditMemberships(int ID)
        {

            //viewModel.studentMembershipViewModel = await _context.StudentMembership.ToListAsync();
            var viewModel = new CommunityViewModel();
            //viewModel.Community = await _context.Community
            //      .Include(i => i.CommunityMembership)
            //      .AsNoTracking()
            //      .OrderBy(i => i.Title)
            //      .ToListAsync();
            //viewModel.Student = await _context.Student.ToListAsync();

            //if (ID >= 0)
            //{
            //    viewModel.EditStudent = _context.Student.SingleOrDefault(x => x.ID.Equals(ID));
            //}

            viewModel.Community = await _context.Community
                  .Include(i => i.CommunityMembership)
                  .AsNoTracking().OrderBy(i => i.Title)
                  .ToListAsync();
            viewModel.Student = await _context.Student
                .Include(i => i.CommunityMembership)
                .AsNoTracking().OrderBy(i => i.FirstName)
                .ToListAsync();

            if (ID > 0)
            {
                ViewData["StudentID"] = ID;
                viewModel.CommunityMembership = viewModel.Student.Where(
                    x => x.ID.Equals(ID)).Single().CommunityMembership;
                
                //if(viewModel.CommunityMembership.Count() == 0)
                //{
                //    viewModel.IsMember = false;
                //}
                //else
                //{
                //    viewModel.IsMember = true;
                //}
                viewModel.EditStudent = _context.Student.SingleOrDefault(x => x.ID.Equals(ID));


            }


            return View(viewModel);
        }

        public async Task<IActionResult> AddMembership(int studentId, string communityId)
        {
            var viewModel = new CommunityViewModel();
            //viewModel.Student = await _context.Student.ToListAsync();
            //viewModel.EditStudent = _context.Student.SingleOrDefault(x => x.ID.Equals(studentId));
         

            var student = _context.Student.SingleOrDefault(x => x.ID == (studentId));
            var community =  _context.Community.SingleOrDefault(x => x.ID.Equals(communityId));
            var communityMemberShip = new CommunityMembership();
            communityMemberShip.Student = student;
            communityMemberShip.StudentID = studentId;
            communityMemberShip.Community = community;
            communityMemberShip.CommunityID = communityId;
            _context.CommunityMembership.Add(communityMemberShip);
            _context.SaveChanges();

            //return RedirectToAction(nameof(EditMemberships));
            return RedirectToAction("EditMemberships", new { id = studentId });

            //return View();
        }

        public async Task<IActionResult> RemoveMembership(int studentId, string communityId)
        {

            var viewModel = new CommunityViewModel();
            viewModel.Student = await _context.Student.ToListAsync();
            viewModel.EditStudent = _context.Student.SingleOrDefault(x => x.ID.Equals(studentId));


            var student = _context.Student.SingleOrDefault(x => x.ID == (studentId));
            var community = _context.Community.SingleOrDefault(x => x.ID.Equals(communityId));
            var communityMemberShip = new CommunityMembership();
            communityMemberShip.Student = student;
            communityMemberShip.Community = community;
            //communityMemberShip.StudentID = studentId;
            //communityMemberShip.CommunityID = communityId;
            _context.CommunityMembership.Remove(communityMemberShip);
            _context.SaveChanges();

            //viewModel.CommunityViewModel.Memberships = await _context.Community

                  //.ToListAsync();

            //return RedirectToAction(nameof(EditMemberships));
            return RedirectToAction("EditMemberships", new { id = studentId });

        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstName,EnrollmentDate,FullName")] Student student)
        {
            if (id != student.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.ID == id);
        }
    }
}
