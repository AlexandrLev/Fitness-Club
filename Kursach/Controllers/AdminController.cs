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
        //Main method
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

        //USER
        //EDIT User
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
            editUser.ConclusionDate = user.ConclusionDate;
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
                User userCh = await _context.Users.FirstOrDefaultAsync(u => u.Login == user.Login & u.UserId != user.UserId);
                if (userCh == null)
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
                else
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                    return await EditUser(id);
                }
            }
            return RedirectToAction(nameof(EditDB));
        }

        //ADD User
        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            var memberTickets = _context.MemberTickets.ToList();
            
            var editUser = new EditUser();
            editUser.MemberTickets = new SelectList(memberTickets, "MemberTicketId", "Name");
            return View(editUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(User user)
        {
            
            if (ModelState.IsValid)
            {
                User userCh = await _context.Users.FirstOrDefaultAsync(u => u.Login == user.Login);
                if (userCh == null)
                {
                    try
                    {
                        _context.Users.Add(user);
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
                else
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                    return View();
                }
            }
            return RedirectToAction(nameof(EditDB));
        }

        //DELETE User
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

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditDB));
        }


        //GYM
        //ADD Gym
        [HttpGet]
        public async Task<IActionResult> AddGym()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGym(Gym gym)
        {

            if (ModelState.IsValid)
            {
                Gym gymCh = await _context.Gyms.FirstOrDefaultAsync(u => u.Name == gym.Name);
                if (gymCh == null)
                {
                    try
                    {
                        _context.Gyms.Add(gym);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                        
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                    return View();
                }
            }
            return RedirectToAction(nameof(EditDB));
        }

        //EDIT Gym
        public async Task<IActionResult> EditGym(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gym = await _context.Gyms.FindAsync(id);
            if (gym == null)
            {
                return NotFound();
            }
            var editGym = new EditGym();
            editGym.GymId = gym.GymId;
            editGym.Address = gym.Address;
            editGym.Name = gym.Name;
            editGym.PhoneNumber = gym.PhoneNumber;
            
            return View(editGym);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGym(int id, /*[Bind("ID,Title,ReleaseDate,Genre,Price")] */ Gym gym)
        {
            if (id != gym.GymId)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                Gym gymCh = await _context.Gyms.FirstOrDefaultAsync(u => u.Name == gym.Name & u.GymId != gym.GymId);
                if (gymCh == null)
                {
                    try
                    {
                        _context.Update(gym);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        Gym gymCheck = await _context.Gyms.FirstOrDefaultAsync(u => u.GymId != gym.GymId);
                        if (gymCheck == null)

                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                    return await EditGym(id);
                }
            }
            return RedirectToAction(nameof(EditDB));
        }

        //DELETE Gym
        public async Task<IActionResult> DeleteGym(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gym = await _context.Gyms
                .FirstOrDefaultAsync(m => m.GymId == id);
            if (gym == null)
            {
                return NotFound();
            }

            return View(gym);
        }
        [HttpPost, ActionName("DeleteGym")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGymConfirmed(int id)
        {
            var gym = await _context.Gyms.FindAsync(id);
            _context.Gyms.Remove(gym);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditDB));
        }


        //MEMBER TICKET
        //EDIT MemberTicket
        public async Task<IActionResult> EditMemberTicket(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberTicket = await _context.MemberTickets.FindAsync(id);
            var gyms = _context.Gyms.ToList();
            if (memberTicket == null)
            {
                return NotFound();
            }
            var editMemberTicket = new EditMemberTicket();
            editMemberTicket.MemberTicketId = memberTicket.MemberTicketId;
            editMemberTicket.Name = memberTicket.Name;
            editMemberTicket.Cost = memberTicket.Cost;
            editMemberTicket.ValidityPeriod = memberTicket.ValidityPeriod;
            editMemberTicket.GymId = memberTicket.MemberTicketId;
            editMemberTicket.Gyms = new SelectList(gyms, "GymId", "Name");
            return View(editMemberTicket);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMemberTicket(int id, /*[Bind("ID,Title,ReleaseDate,Genre,Price")] */ MemberTicket memberTicket)
        {
            if (id != memberTicket.MemberTicketId)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                MemberTicket memberTicketCh = await _context.MemberTickets.FirstOrDefaultAsync(u => u.Name == memberTicket.Name & u.MemberTicketId != memberTicket.MemberTicketId);
                if (memberTicketCh == null)
                {
                    try
                    {
                        _context.Update(memberTicket);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        MemberTicket memberTicketCheck = await _context.MemberTickets.FirstOrDefaultAsync(u => u.MemberTicketId == memberTicket.MemberTicketId);
                        if (memberTicketCheck == null)

                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Некорректное название");
                    return await EditMemberTicket(id);
                }
            }
            return RedirectToAction(nameof(EditDB));
        }

        //ADD MemberTicket
        [HttpGet]
        public async Task<IActionResult> AddMemberTicket()
        {
            var gyms = _context.Gyms.ToList();

            var editMemberTicket = new EditMemberTicket();
            editMemberTicket.Gyms = new SelectList(gyms, "GymId", "Name");
            return View(editMemberTicket);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMemberTicket(MemberTicket memberTicket)
        {

            if (ModelState.IsValid)
            {
                MemberTicket memberTicketCh = await _context.MemberTickets.FirstOrDefaultAsync(u => u.Name == memberTicket.Name);
                if (memberTicketCh == null)
                {
                    try
                    {
                        _context.MemberTickets.Add(memberTicket);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        MemberTicket memberTicketCheck = await _context.MemberTickets.FirstOrDefaultAsync(u => u.MemberTicketId == memberTicket.MemberTicketId);
                        if (memberTicketCheck == null)

                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                    return View();
                }
            }
            return RedirectToAction(nameof(EditDB));
        }

        //DELETE MemberTicket
        public async Task<IActionResult> DeleteMemberTicket(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberTicket = await _context.MemberTickets
                .FirstOrDefaultAsync(m => m.MemberTicketId == id);
            if (memberTicket == null)
            {
                return NotFound();
            }

            return View(memberTicket);
        }
        [HttpPost, ActionName("DeleteMemberTicket")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMemberTicketConfirmed(int id)
        {
            var memberTicket = await _context.MemberTickets.FindAsync(id);
            _context.MemberTickets.Remove(memberTicket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditDB));
        }




        //TRAINING
        //EDIT Training
        public async Task<IActionResult> EditTraining(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Trainings.FindAsync(id);
            var gyms = _context.Gyms.ToList();
            var coaches = _context.Users.Where(u => u.RoleId == 2).ToList();
            if (training == null)
            {
                return NotFound();
            }
            var editTraining = new EditTraining();
            editTraining.Name = training.Name;
            editTraining.TimeOfStarting = training.TimeOfStarting;
            editTraining.Time = training.TimeOfStarting;


            editTraining.TrainingDuration = training.TrainingDuration;
            editTraining.TrainingId = training.TrainingId;

            editTraining.GymId = training.GymId;
            editTraining.Gyms = new SelectList(gyms, "GymId", "Name");
            editTraining.CoachId = training.CoachId;
            editTraining.Coaches = new SelectList(coaches, "UserId", "LastName");
            return View(editTraining);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTraining(int id, /*[Bind("ID,Title,ReleaseDate,Genre,Price")] */ EditTraining edtraining)
        {
            if (id != edtraining.TrainingId)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                Training trainingCh = await _context.Trainings.FirstOrDefaultAsync(u => u.Name == edtraining.Name & u.TrainingId != edtraining.TrainingId);
                if (trainingCh == null)
                {
                    try
                    {
                        Training training = new Training();
                        DateTime datetime = new DateTime(edtraining.TimeOfStarting.Year, edtraining.TimeOfStarting.Month, edtraining.TimeOfStarting.Day,
                            edtraining.Time.Hour, edtraining.Time.Minute, edtraining.Time.Second);
                        training.TimeOfStarting = datetime;
                        training.Name = edtraining.Name;
                        training.TrainingDuration = edtraining.TrainingDuration;
                        training.TrainingId = edtraining.TrainingId;

                        training.GymId = edtraining.GymId;
                        training.CoachId = edtraining.CoachId;
                        _context.Update(training);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        Training trainingCheck = await _context.Trainings.FirstOrDefaultAsync(u => u.TrainingId == edtraining.TrainingId);
                        if (trainingCheck == null)

                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                    return await EditTraining(id);
                }
            }
            return RedirectToAction(nameof(EditDB));
        }

        //ADD Training
        [HttpGet]
        public async Task<IActionResult> AddTraining()
        {
            var gyms = _context.Gyms.ToList();
            var coaches = _context.Users.Where(u => u.RoleId == 2).ToList();

            var editTraining = new EditTraining(); 
            editTraining.Gyms = new SelectList(gyms, "GymId", "Name");
            editTraining.Coaches = new SelectList(coaches, "UserId", "LastName");
            return View(editTraining);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTraining(EditTraining edtraining)
        {

            if (ModelState.IsValid)
            {
                Training trainingCh = await _context.Trainings.FirstOrDefaultAsync(u => u.Name == edtraining.Name );
                if (trainingCh == null)
                {
                    try
                    {
                        Training training = new Training();
                        DateTime datetime = new DateTime(edtraining.TimeOfStarting.Year, edtraining.TimeOfStarting.Month, edtraining.TimeOfStarting.Day,
                            edtraining.Time.Hour, edtraining.Time.Minute, edtraining.Time.Second);
                        training.TimeOfStarting = datetime;
                        training.Name = edtraining.Name;
                        training.TrainingDuration = edtraining.TrainingDuration;
                        training.TrainingId = edtraining.TrainingId;

                        training.GymId = edtraining.GymId;
                        training.CoachId = edtraining.CoachId;
                        _context.Trainings.Add(training);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        Training trainingCheck = await _context.Trainings.FirstOrDefaultAsync(u => u.TrainingId == edtraining.TrainingId);
                        if (trainingCheck == null)

                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                    return View();
                }
            }
            return RedirectToAction(nameof(EditDB));
        }

        //DELETE Training
        public async Task<IActionResult> DeleteTraining(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Trainings
                .FirstOrDefaultAsync(m => m.TrainingId == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        [HttpPost, ActionName("DeleteTraining")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainingConfirmed(int id)
        {
            var training = await _context.Trainings.FindAsync(id);
            _context.Trainings.Remove(training);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditDB));
        }



        //TRAINING REGISTRATION
        //ADD Training
        [HttpGet]
        public async Task<IActionResult> AddTrainingRegistration()
        {
            var trainings = _context.Trainings.ToList();
            var clients = _context.Users.Where(u => u.RoleId == 1).ToList();

            var editTrainingRegistration = new EditTrainingRegistration();
            editTrainingRegistration.Trainings = new SelectList(trainings, "TrainingId", "Name");
            editTrainingRegistration.Clients = new SelectList(clients, "UserId", "LastName");
            return View(editTrainingRegistration);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTrainingRegistration(TrainingRegistration trainingRegistration)
        {

            if (ModelState.IsValid)
            {
                TrainingRegistration trainingRegistrationsCh = await _context.TrainingRegistrations.FirstOrDefaultAsync(u => u.TrainingId == trainingRegistration.TrainingId & 
                    u.ClientId == trainingRegistration.ClientId);
                if (trainingRegistrationsCh == null)
                {
                    try
                    {
                        _context.TrainingRegistrations.Add(trainingRegistration);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (trainingRegistrationsCh == null)

                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                    return View();
                }
            }
            return RedirectToAction(nameof(EditDB));
        }

        //DELETE Training
        public async Task<IActionResult> DeleteTrainingRegistration(int? TrainingId, int? ClientId)
        {
            if (ClientId == null || TrainingId == null)
            {
                return NotFound();
            }

            var trainingRegistration = await _context.TrainingRegistrations
                .FirstOrDefaultAsync(u => u.TrainingId == TrainingId &
                    u.ClientId == ClientId);
            if (trainingRegistration == null)
            {
                return NotFound();
            }

            return View(trainingRegistration);
        }

        [HttpPost, ActionName("DeleteTrainingRegistration")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainingRegistrationConfirmed(int TrainingId, int ClientId)
        {
            var trainingRegistration = await _context.TrainingRegistrations
                .FirstOrDefaultAsync(u => u.TrainingId == TrainingId &
                    u.ClientId == ClientId);
            _context.TrainingRegistrations.Remove(trainingRegistration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EditDB));
        }


        //Date Validation
        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckDate(DateTime ConclusionDate)
        {
            //DateTime d1 = new DateTime(ConclusionDate.Year, ConclusionDate.Month, ConclusionDate.Day);
            //DateTime d1 = new DateTime(2021, 4, 1);
            DateTime d2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //DateTime d2 = new DateTime(2021, 3, 31);
            if (ConclusionDate <= d2)
                return Json(true);
            else
                return Json(false);
        }
    }
}
