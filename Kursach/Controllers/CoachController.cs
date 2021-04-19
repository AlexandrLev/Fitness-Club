using Kursach.Models.Admin;
using Kursach.Models.Coach;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Controllers
{
    [Authorize(Roles = "Coach")]
    public class CoachController : Controller
    {
        
        private KursuchDBContext _context;
        public CoachController(KursuchDBContext context)
        {
            _context = context;

        }

        public async Task<IActionResult> ControleTraining()
        {
            User AuthUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == User.Identity.Name.ToString());
            var trainings = _context.Trainings.Include(u => u.Coach).Include(u => u.Gym).Where(u => u.CoachId == AuthUser.UserId).ToArray();
            return View(trainings);
        }

        public async Task<IActionResult> UpcomingTraining()
        {
            User AuthUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == User.Identity.Name.ToString());
            var trainings = _context.Trainings.Include(u => u.Coach).Include(u => u.Gym).Where(u => u.CoachId == AuthUser.UserId).ToArray();
            return View(trainings);
        }

        public async Task<IActionResult> ShowList(int id)
        {
            var training = await _context.Trainings.Include(u => u.Gym).FirstOrDefaultAsync(u => u.TrainingId == id);
            var trainingReg = _context.TrainingRegistrations.Include(u => u.Client).Where(u => u.TrainingId == id).ToList();
            ShowListClass objForList = new ShowListClass();
            objForList.Training = training;
            objForList.TrainingRegistrations = trainingReg;

            return View(objForList);
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
            var editTraining = new ControleTraining();
            editTraining.Name = training.Name;
            editTraining.TimeOfStarting = training.TimeOfStarting;
            editTraining.Time = training.TimeOfStarting;


            editTraining.TrainingDuration = training.TrainingDuration;
            editTraining.TrainingId = training.TrainingId;

            editTraining.GymId = training.GymId;
            editTraining.Gyms = new SelectList(gyms, "GymId", "Name");
            return View(editTraining);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTraining(int id, /*[Bind("ID,Title,ReleaseDate,Genre,Price")] */ ControleTraining edtraining)
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
            return RedirectToAction(nameof(ControleTraining));
        }

        //ADD Training
        [HttpGet]
        public async Task<IActionResult> AddTraining()
        {
            var gyms = _context.Gyms.ToList();
            var controleTraining = new ControleTraining();
            controleTraining.Gyms = new SelectList(gyms, "GymId", "Name");
            return View(controleTraining);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTraining(ControleTraining edtraining)
        {

            if (ModelState.IsValid)
            {
                User AuthUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == User.Identity.Name.ToString());
                Training trainingCh = await _context.Trainings.FirstOrDefaultAsync(u => u.Name == edtraining.Name);
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
                        training.CoachId = AuthUser.UserId;
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
            return RedirectToAction(nameof(ControleTraining));
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
            return RedirectToAction(nameof(ControleTraining));
        }
    }
}
