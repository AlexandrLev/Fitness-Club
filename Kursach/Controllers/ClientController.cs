using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Kursach;
using Kursach.Models.Coach;
using Kursach.Models.Client;

namespace Kursach.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        private KursuchDBContext _context;
        public ClientController(KursuchDBContext context)
        {
            _context = context;

        }
        public async Task<IActionResult> ControlMemberTicket()
        {
            User AuthUser = await _context.Users.Include(u => u.MemberTicket).ThenInclude(c => c.Gym)
                .FirstOrDefaultAsync(u => u.Login == User.Identity.Name.ToString());
            
            
            return View(AuthUser);
        }

        public async Task<IActionResult> ShowListTraining()
        {
            User AuthUser = await _context.Users.Include(u => u.MemberTicket).ThenInclude(c => c.Gym)
                .FirstOrDefaultAsync(u => u.Login == User.Identity.Name.ToString());
            var TrReg = _context.TrainingRegistrations.Include(u => u.Training)
                .ThenInclude(c => c.Gym).Include(u => u.Training)
                .ThenInclude(c => c.Coach).Where(p => p.ClientId == AuthUser.UserId).ToList();
            ShowListTrainings showListClass = new ShowListTrainings();
            showListClass.TrainingRegistrations = TrReg;
            showListClass.Client = AuthUser;
            return View(showListClass);
        }

        //ChooseMemberTicket
        [HttpGet]
        public async Task<IActionResult> ChooseMemberTicket()
        {
            var memberTickets = _context.MemberTickets.Include(u => u.Gym);
            var memTicks = new MemberTicket[memberTickets.Count()];
            int i = 0;
            foreach (var item in memberTickets)
            {
                memTicks[i] = item;
                i++;
            }
            return View(memTicks);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChooseMemberTicket(int id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(ControlMemberTicket)); 
            }

            User AuthUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == User.Identity.Name.ToString());
            var mems = await _context.MemberTickets
                .FirstOrDefaultAsync(m => m.MemberTicketId == id);
            if (mems!=null)
            {
                AuthUser.MemberTicketId = id;
            }
            AuthUser.ConclusionDate= DateTime.Now.Date;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ControlMemberTicket));
        }

        //DeleteMemberFromUser
        public async Task<IActionResult> DeleteMemberFromUser(int? id)
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

        [HttpPost, ActionName("DeleteMemberFromUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMemberFromUserConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            user.MemberTicketId = null;
            user.ConclusionDate = null;
            user.MemberTicket = null;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ControlMemberTicket));
        }

        public async Task<IActionResult> MyTraining()
        {
            User AuthUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == User.Identity.Name.ToString());
            TrainingRegistration[] TrReg = _context.TrainingRegistrations.Include(u => u.Training).ThenInclude(c => c.Gym).Where(p => p.ClientId == AuthUser.UserId).ToArray();
       
            return View(TrReg);
        }

        //DeleteMemberFromUser
        public async Task<IActionResult> DeleteTrainingRegistration(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User AuthUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == User.Identity.Name.ToString());

            var trainingRegistration = await _context.TrainingRegistrations
                .FirstOrDefaultAsync(u => u.TrainingId == id &
                    u.ClientId == AuthUser.UserId);
            if (trainingRegistration == null)
            {
                return NotFound();
            }
           
            return View(trainingRegistration);
        }

        [HttpPost, ActionName("DeleteTrainingRegistration")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainingRegistrationConfirmed(int id)
        {
            User AuthUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == User.Identity.Name.ToString());

            var trainingRegistration = await _context.TrainingRegistrations
                .FirstOrDefaultAsync(u => u.TrainingId == id &
                    u.ClientId == AuthUser.UserId);
            
            _context.TrainingRegistrations.Remove(trainingRegistration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyTraining));
        }


        //RegisterOnTraining
        [HttpGet]
        public async Task<IActionResult> RegisterOnTraining()
        {
            User AuthUser = await _context.Users.Include(u => u.MemberTicket)
                .FirstOrDefaultAsync(u => u.Login == User.Identity.Name.ToString());
            var trainings = _context.Trainings.Include(u => u.Gym).Include(u => u.Coach)
                .Where(p => p.GymId == AuthUser.MemberTicket.GymId).ToArray();
            
            
            return View(trainings);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterOnTraining(int id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(MyTraining));
            }

            User AuthUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == User.Identity.Name.ToString());
            TrainingRegistration tr = new TrainingRegistration();
            tr.TrainingId = id;
            tr.ClientId = AuthUser.UserId;
            var check = _context.TrainingRegistrations.FirstOrDefaultAsync(u => u.ClientId == tr.ClientId && u.TrainingId == tr.TrainingId);
            if (check !=null)
            {
                return RedirectToAction(nameof(MyTraining));
            }
            else
            {
                _context.TrainingRegistrations.Add(tr);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyTraining));
            }
        }
    }
}
