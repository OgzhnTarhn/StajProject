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
            ViewBag.ErrorMessage = "SAP Hatası: " + ex.Message;
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
                return RedirectToAction("Dashboard");
            }
            catch (RfcAbapException ex)
            {
                ViewBag.ErrorMessage = "SAP Hatası: " + ex.Message;
            }
        }
        return View(model);
    }

    // --- (İLERİDE) Kullanıcı düzenleme ve silme action'larını buraya ekleyeceksin! ---
}
