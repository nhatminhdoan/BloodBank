using BloodDonationApp.Models;
using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BloodDonationApp.Controllers
{
    public class AccountStatusController : Controller
    {
        OnlineBloodBankDbEntities DB = new OnlineBloodBankDbEntities();

        public ActionResult AllAccountStatus()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var accountstatuses = DB.AccountStatusTables.ToList();
            var listaccountstatuses = new List<AccountStatusMV>();
            foreach (var accountstatus in accountstatuses)
            {
                var addaccountstatus = new AccountStatusMV();
                addaccountstatus.AccountStatusID = accountstatus.AccountStatusID;
                addaccountstatus.AccountStatus = accountstatus.AccountStatus;
                listaccountstatuses.Add(addaccountstatus);
            }
            return View(listaccountstatuses);
        }

        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var accountstatus = new AccountStatusMV();
            return View(accountstatus);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccountStatusMV accountStatusMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var accountstatusTable = new AccountStatusTable();
                accountstatusTable.AccountStatusID = accountStatusMV.AccountStatusID;
                accountstatusTable.AccountStatus = accountStatusMV.AccountStatus;
                DB.AccountStatusTables.Add(accountstatusTable);
                DB.SaveChanges();

                return RedirectToAction("AllAccountStatus");
            }
            return View(accountStatusMV);
        }

        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var accountstatus = DB.AccountStatusTables.Find(id);
            if (accountstatus == null)
            {
                return HttpNotFound();
            }
            var AccountStatusMV = new AccountStatusMV();
            AccountStatusMV.AccountStatusID = accountstatus.AccountStatusID;
            AccountStatusMV.AccountStatus = accountstatus.AccountStatus;
            return View(AccountStatusMV);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountStatusMV accountStatusMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var accountstatusTable = new AccountStatusTable();
                accountstatusTable.AccountStatusID = accountStatusMV.AccountStatusID;
                accountstatusTable.AccountStatus = accountStatusMV.AccountStatus;
                DB.Entry(accountstatusTable).State = EntityState.Modified;
                DB.SaveChanges();

                return RedirectToAction("AllAccountStatus");
            }
            return View(accountStatusMV);
        }

        public ActionResult Delete(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var accountstatus = DB.AccountStatusTables.Find(id);
            if (accountstatus == null)
            {
                return HttpNotFound();
            }
            var accountStatusMV = new AccountStatusMV();
            accountStatusMV.AccountStatusID = accountstatus.AccountStatusID;
            accountStatusMV.AccountStatus = accountstatus.AccountStatus;
            return View(accountStatusMV);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var accountstatus = DB.AccountStatusTables.Find(id);
            DB.AccountStatusTables.Remove(accountstatus);
            DB.SaveChanges();
            return RedirectToAction("AllAccountStatus");
        }
    }
}