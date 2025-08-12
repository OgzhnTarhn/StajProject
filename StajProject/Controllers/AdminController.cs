using System.Web.Mvc;
using SAP.Middleware.Connector;
using StajProject.Models;
using StajProject.Helpers;
using System.Collections.Generic;

public class AdminController : Controller
{
    // Admin dashboard: Kullanıcıları listele
    [HttpGet]
    public ActionResult Dashboard()
    {
        if (Session["Role"] == null || Session["Role"].ToString() != "A")
            return RedirectToAction("Login", "Account");

        var userList = new List<SAPUserModel>();

        try
        {
            RfcDestination dest;
            IRfcFunction func = SapConnectorBase.CreateFunction("ZUSR_GET_ALL_USERS", out dest);
            func.Invoke(dest);

            IRfcTable etUsers = func.GetTable("ET_USERS");

            foreach (IRfcStructure row in etUsers)
            {
                userList.Add(new SAPUserModel
                {
                    Username = row.GetString("USERNAME"),
                    Role = row.GetString("ROLE")
                });
            }
        }
        catch (RfcAbapException ex)
        {
            ViewBag.ErrorMessage = "SAP Error: " + ex.Message;
        }

        return View(userList); // Views/Admin/Dashboard.cshtml
    }

    // Kullanıcı ekleme (GET)
    [HttpGet]
    public ActionResult AddUser()
    {
        return View(new RegisterModel());
    }

    // Kullanıcı ekleme (POST)
    [HttpPost]
    public ActionResult AddUser(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                RfcDestination dest;
                IRfcFunction func = SapConnectorBase.CreateFunction("ZUSR_INSERT_USER", out dest);

                func.SetValue("IV_USERNAME", model.Username);
                func.SetValue("IV_PASSWORD", model.Password);
                func.SetValue("IV_ROLE", model.Role); // Admin panelinde seçilen rol gönderilir

                func.Invoke(dest);
                ViewBag.Message = func.GetString("EV_RESULT");
                return RedirectToAction("Dashboard", "Admin"); // Admin işlem sonrası User Management'a geri dön
            }
            catch (RfcAbapException ex)
            {
                ViewBag.ErrorMessage = "SAP Error: " + ex.Message;
            }
        }
        return View(model);
    }
    [HttpGet]
    public ActionResult EditUser(string username)
    {
        if (Session["Role"] == null || Session["Role"].ToString() != "A")
            return RedirectToAction("Login", "Account");

        var model = new EditUserProfileModel { Username = username };

        // Kullanıcının mevcut rolünü SAP'den çek
        try
        {
            RfcDestination dest;
            IRfcFunction func = SapConnectorBase.CreateFunction("ZUSR_GET_USER", out dest);
            func.SetValue("IV_USERNAME", username);
            func.SetValue("IV_PASSWORD", ""); // Admin şifre bilmiyor
            func.Invoke(dest);

            IRfcTable etUserInfo = func.GetTable("ET_USER_INFO");
            if (etUserInfo.Count > 0)
            {
                var userRow = etUserInfo[0];
                model.Role = userRow.GetString("ROLE");
            }
        }
        catch (RfcAbapException ex)
        {
            ViewBag.ErrorMessage = "SAP Error: " + ex.Message;
        }

        return View(model);
    }

    [HttpPost]
    public ActionResult EditUser(EditUserProfileModel model)
    {
        if (Session["Role"] == null || Session["Role"].ToString() != "A")
            return RedirectToAction("Login", "Account");

        if (ModelState.IsValid)
        {
            try
            {
                RfcDestination dest;
                IRfcFunction func = SapConnectorBase.CreateFunction("ZUSR_UPDATE_USER", out dest);

                func.SetValue("IV_USERNAME", model.Username);
                func.SetValue("IV_OLD_PASSWORD", ""); // Admin şifre girmez
                func.SetValue("IV_NEW_USERNAME", ""); // Admin doesn't change
                func.SetValue("IV_NEW_PASSWORD", ""); // Admin değiştirmez
                func.SetValue("IV_ROLE", model.Role); // Only role

                func.Invoke(dest);

                model.Message = func.GetString("EV_RESULT");
                TempData["Message"] = model.Message;
                return RedirectToAction("Dashboard", "Admin"); // Admin işlem sonrası User Management'a geri dön
            }
            catch (RfcAbapException ex)
            {
                model.Message = "SAP Error: " + ex.Message;
            }
        }
        return View(model);
    }



    [HttpGet]
    public ActionResult DeleteUser(string username)
    {
        if (Session["Role"] == null || Session["Role"].ToString() != "A")
            return RedirectToAction("Login", "Account");

        string message = "";

        try
        {
            RfcDestination dest;
            IRfcFunction func = SapConnectorBase.CreateFunction("ZUSR_DELETE_USER", out dest);

            func.SetValue("IV_USERNAME", username);
            func.Invoke(dest);

            message = func.GetString("EV_RESULT");
            TempData["Message"] = message;  // Dashboard'da göstereceğiz
        }
        catch (RfcAbapException ex)
        {
            TempData["Message"] = "SAP Error: " + ex.Message;
        }

        return RedirectToAction("Dashboard", "Admin"); // Admin işlem sonrası User Management'a geri dön
    }

}
