﻿using BloodDonationApp.Models;
using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodDonationApp.Controllers
{
    public class RegisterationController : Controller
    {
        OnlineBloodBankDbEntities DB = new OnlineBloodBankDbEntities();
        static RegisterationMV registerationmv;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectUser(RegisterationMV registerationMV)
        {
            registerationmv = registerationMV;
            if (registerationMV.UserTypeID == 2)
            {
                return RedirectToAction("DonorUser");
            }
            else if (registerationMV.UserTypeID == 3)
            {
                return RedirectToAction("SeekerUser");
            }
            else if (registerationMV.UserTypeID == 4)
            {
                return RedirectToAction("HospitalUser");
            }
            else if (registerationMV.UserTypeID == 5)
            {
                return RedirectToAction("BloodBankUser");
            }
            else
            {
                return RedirectToAction("MainHome", "Home");
            }

        }

        public ActionResult HospitalUser()
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registerationmv.CityID);
            return View(registerationmv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HospitalUser(RegisterationMV registerationMV)
        {
            if (ModelState.IsValid)
            {
                var checktitle = DB.HospitalTables.Where(h => h.FullName == registerationMV.Hospital.FullName.Trim()).FirstOrDefault();
                if (checktitle == null)
                {
                    using (var transaction = DB.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new UserTable();
                            user.UserName = registerationMV.User.UserName;
                            user.Password = registerationMV.User.Password;
                            user.EmailAddress = registerationMV.User.EmailAddress;
                            user.AccountStatusID = 1;
                            user.UserTypeID = registerationMV.UserTypeID;
                            user.Description = registerationMV.User.Description;
                            DB.UserTables.Add(user);
                            DB.SaveChanges();

                            var hospital = new HospitalTable();
                            hospital.FullName = registerationMV.Hospital.FullName;
                            hospital.Address = registerationMV.Hospital.Address;
                            hospital.PhoneNo = registerationMV.Hospital.PhoneNo;
                            hospital.WebSite = registerationMV.Hospital.WebSite;
                            hospital.Email = registerationMV.Hospital.Email;
                            hospital.Location = registerationMV.Hospital.Address;
                            hospital.CityID = registerationMV.CityID;
                            hospital.UserID = user.UserID;
                            DB.HospitalTables.Add(hospital);
                            DB.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thank For Registration! Your Query will be Review Shortly! ";
                            return RedirectToAction("MainHome", "Home");
                        }
                        catch
                        {
                            ModelState.AddModelError(string.Empty, "Please Provide Corect Information!");
                            transaction.Rollback();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Hospital Already Refistered!");
                }
            }

                ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registerationMV.CityID);
                return View(registerationMV);
            
        }

        public ActionResult DonorUser()
        {
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupId", "BloodGroup", "0");
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registerationmv.CityID);
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", "0");
            return View(registerationmv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DonorUser(RegisterationMV registerationMV)
        {
            if (ModelState.IsValid)
            {
                var checktitle = DB.DonorTables.Where(h => h.FullName == registerationMV.Donor.FullName.Trim() && h.CNIC== registerationMV.Donor.CNIC).FirstOrDefault();
                if (checktitle == null)
                {
                    using (var transaction = DB.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new UserTable();
                            user.UserName = registerationMV.User.UserName;
                            user.Password = registerationMV.User.Password;
                            user.EmailAddress = registerationMV.User.EmailAddress;
                            user.AccountStatusID = 1;
                            user.UserTypeID = registerationMV.UserTypeID;
                            user.Description = registerationMV.User.Description;
                            DB.UserTables.Add(user);
                            DB.SaveChanges();

                            var donor = new DonorTable();
                            donor.FullName = registerationMV.Donor.FullName;
                            donor.BloodGroupID = registerationMV.BloodGroupID;
                            donor.Location = registerationMV.Donor.Location;
                            donor.ContactNo = registerationMV.Donor.ContactNo;
                            donor.LastDonationDate = registerationMV.Donor.LastDonationDate;
                            donor.CNIC = registerationMV.Donor.CNIC;
                            donor.GenderID = registerationMV.GenderID;
                            donor.CityID = registerationMV.CityID;
                            donor.UserID = user.UserID;
                            DB.DonorTables.Add(donor);
                            DB.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thank For Registration! Your Query will be Review Shortly! ";
                            return RedirectToAction("MainHome", "Home");
                        }
                        catch
                        {
                            ModelState.AddModelError(string.Empty, "Please Provide Corect Information!");
                            transaction.Rollback();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Donor Already Refistered!");
                }
            }
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", registerationMV.GenderID);
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupId", "BloodGroup", registerationMV.BloodGroupID);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registerationMV.CityID);
            return View(registerationMV);
        }

        public ActionResult BloodBankUser()
        {
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registerationmv.CityID);
            return View(registerationmv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BloodBankUser(RegisterationMV registerationMV)
        {
            if (ModelState.IsValid)
            {
                var checktitle = DB.BloodBankTables.Where(h => h.BloodBankName == registerationMV.BloodBank.BloodBankName.Trim() && h.PhoneNo == registerationMV.BloodBank.PhoneNo).FirstOrDefault();
                if (checktitle == null)
                {
                    using (var transaction = DB.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new UserTable();
                            user.UserName = registerationMV.User.UserName;
                            user.Password = registerationMV.User.Password;
                            user.EmailAddress = registerationMV.User.EmailAddress;
                            user.AccountStatusID = 1;
                            user.UserTypeID = registerationMV.UserTypeID;
                            user.Description = registerationMV.User.Description;
                            DB.UserTables.Add(user);
                            DB.SaveChanges();

                            var bloodBank = new BloodBankTable();
                            bloodBank.BloodBankName = registerationMV.BloodBank.BloodBankName;
                            bloodBank.Address = registerationMV.BloodBank.Location;
                            bloodBank.Location = registerationMV.BloodBank.Location;
                            bloodBank.PhoneNo = registerationMV.BloodBank.PhoneNo;
                            bloodBank.WebSite = registerationMV.BloodBank.WebSite;
                            bloodBank.Email = registerationMV.BloodBank.Email;
                            bloodBank.CityID = registerationMV.CityID;
                            bloodBank.UserID = user.UserID;
                            DB.BloodBankTables.Add(bloodBank);
                            DB.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thank For Registration! Your Query will be Review Shortly! ";
                            return RedirectToAction("MainHome", "Home");
                        }
                        catch
                        {
                            ModelState.AddModelError(string.Empty, "Please Provide Corect Information!");
                            transaction.Rollback();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Blood Bank Already Refistered!");
                }
            }
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registerationMV.CityID);
            return View(registerationMV);
        }

        public ActionResult SeekerUser()
        {
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupId", "BloodGroup", "0");
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", "0");
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", "0");
            return View(registerationmv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeekerUser(RegisterationMV registerationMV)
        {
            if (ModelState.IsValid)
            {
                var checktitle = DB.SeekerTables.Where(h => h.FullName == registerationMV.Seeker.FullName.Trim() && h.CNIC == registerationMV.Seeker.CNIC).FirstOrDefault();
                if (checktitle == null)
                {
                    using (var transaction = DB.Database.BeginTransaction())
                    {
                        try
                        {
                            var user = new UserTable();
                            user.UserName = registerationMV.User.UserName;
                            user.Password = registerationMV.User.Password;
                            user.EmailAddress = registerationMV.User.EmailAddress;
                            user.AccountStatusID = 1;
                            user.UserTypeID = registerationMV.UserTypeID;
                            user.Description = registerationMV.User.Description;
                            DB.UserTables.Add(user);
                            DB.SaveChanges();

                            var seeker = new SeekerTable();
                            seeker.FullName = registerationMV.Seeker.FullName;
                            seeker.Age = registerationMV.Seeker.Age;
                            seeker.BloodGroupID = registerationMV.BloodGroupID;
                            seeker.Address = registerationMV.Seeker.Address;
                            seeker.ContactNo = registerationMV.Seeker.ContactNo;
                            seeker.RegistrationDate = DateTime.Now;
                            seeker.CNIC = registerationMV.Seeker.CNIC;
                            seeker.GenderID = registerationMV.GenderID;
                            seeker.CityID = registerationMV.CityID;
                            seeker.UserID = user.UserID;
                            DB.SeekerTables.Add(seeker);
                            DB.SaveChanges();
                            transaction.Commit();
                            ViewData["Message"] = "Thank For Registration! Your Query will be Review Shortly! ";
                            return RedirectToAction("MainHome", "Home");
                        }
                        catch
                        {
                            ModelState.AddModelError(string.Empty, "Please Provide Corect Information!");
                            transaction.Rollback();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Seeker Already Refistered!");
                }
            }
            ViewBag.BloodGroupID = new SelectList(DB.BloodGroupsTables.ToList(), "BloodGroupId", "BloodGroup", registerationMV.BloodGroupID);
            ViewBag.CityID = new SelectList(DB.CityTables.ToList(), "CityID", "City", registerationmv.CityID);
            ViewBag.GenderID = new SelectList(DB.GenderTables.ToList(), "GenderID", "Gender", registerationMV.GenderID);
            return View(registerationMV);
        }
    }
}