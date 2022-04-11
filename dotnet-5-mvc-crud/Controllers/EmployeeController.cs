using dotnet_5_mvc_crud.Data;
using dotnet_5_mvc_crud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_5_mvc_crud.Controllers
{
    public class EmployeeController :Controller
    {
        private ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee Employee)
        {
            // validate that our model meets the requirement
            if (ModelState.IsValid)
            {
                try
                {
                    // update the ef core context in memory 
                    _context.Add(Employee);

                    // sync the changes of ef code in memory with the database
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Something went wrong {ex.Message}");
                }
            }

            ModelState.AddModelError(string.Empty, $"Something went wrong, invalid model");

            // We return the object back to view
            return View(Employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var exist = await _context.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();

            return View(exist);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee Employee)
        {
            // validate that our model meets the requirement
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the Employee exist based on the id
                    var exist = _context.Employees.Where(x => x.Id == Employee.Id).FirstOrDefault();

                    // if the Employee is not null we update the information
                    if (exist != null)
                    {
                        exist.FirstName = Employee.FirstName;
                        exist.LastName = Employee.LastName;
                        exist.Mobile = Employee.Mobile;
                        exist.Email = Employee.Email;

                        // we save the changes into the db
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Something went wrong {ex.Message}");
                }
            }

            ModelState.AddModelError(string.Empty, $"Something went wrong, invalid model");

            return View(Employee);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var exist = await _context.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();

            return View(exist);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Employee Employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var exist = _context.Employees.Where(x => x.Id == Employee.Id).FirstOrDefault();

                    if (exist != null)
                    {
                        _context.Remove(exist);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Something went wrong {ex.Message}");
                }
            }

            ModelState.AddModelError(string.Empty, $"Something went wrong, invalid model");

            return View();
        }
    }
}