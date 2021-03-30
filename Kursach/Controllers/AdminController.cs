using Kursach.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Controllers
{
    public class AdminController : Controller
    {

        private KursuchDBContext _context;
        public AdminController(KursuchDBContext context)
        {
            _context = context;
        }
        public IActionResult EditDB()
        {
            EditDBModel Model = new EditDBModel();
            Model.Users = _context.Users.ToList();
            Model.Gyms = _context.Gyms.ToList();
            Model.MemberTickets = _context.MemberTickets.ToList();
            Model.Trainings = _context.Trainings.ToList();
            Model.TrainingRegistrations = _context.TrainingRegistrations.ToList();
            return View(Model);
        }

        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            var memberTickets = _context.MemberTickets.ToList();
            if (user == null)
            {
                return NotFound();
            }
            var editUser = new EditUser();
            editUser.Login = user.Login;
            editUser.Password = user.Password;
            editUser.PassportData = user.PassportData;
            editUser.LastName = user.LastName;
            editUser.FirstName = user.FirstName;
            editUser.Patronymic = user.Patronymic;
            editUser.RoleId = user.RoleId;
            editUser.UserId = user.UserId;
            editUser.MemberTicketId = user.MemberTicketId;
            editUser.MemberTickets = new SelectList(memberTickets, "MemberTicketId", "Name");
            return View(editUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, /*[Bind("ID,Title,ReleaseDate,Genre,Price")] */ User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    User userCheck = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
                    if (userCheck == null)
                   
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(EditDB));
        }


        // GET: Movies/Delete/5
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditDB));
        }
    }
}
