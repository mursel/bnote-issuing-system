using Microsoft.Owin.Security;
using mjenica.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace mjenica.Controllers
{
    public class RacuniController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel loginPodaci, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(this.Index());

            IAuthenticationManager iAuthManager = HttpContext.GetOwinContext().Authentication;

            PrincipalContext pc = new PrincipalContext(ContextType.Domain, "here-goes-domain-name");

            UserPrincipal up = UserPrincipal.FindByIdentity(pc, loginPodaci.KorisnickoIme);
            bool isSuccess = pc.ValidateCredentials(loginPodaci.KorisnickoIme, loginPodaci.Lozinka); // pogledati ContextOptions SSL ??

            if (!isSuccess)
                return View(this.Index());

            ClaimsIdentity ci = new ClaimsIdentity(MjenicaAuthType.AuthType, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            ci.AddClaim(new Claim(ClaimTypes.Name, up.SamAccountName));
            ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, up.SamAccountName));
            ci.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "active directory"));

            if (!string.IsNullOrEmpty(up.EmailAddress))
            {
                ci.AddClaim(new Claim(ClaimTypes.Email, up.EmailAddress));
            }

            foreach (var gr in up.GetAuthorizationGroups())
            {
                ci.AddClaim(new Claim(ClaimTypes.Role, gr.Name));
            }

            iAuthManager.SignOut(MjenicaAuthType.AuthType);
            iAuthManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, ci);
            
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Naslovna");
        }

        private int VratiSifruKS(string korisnickoIme)
        {
            int sifra = -1;
            string confString = ConfigurationManager.ConnectionStrings["MjeniceDb"].ConnectionString;
            using (SqlConnection cn = new SqlConnection(confString))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("SELECT UserID FROM KS_Sifre WHERE Username = '" + korisnickoIme + "'", cn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    sifra = int.Parse(dr["UserID"].ToString());
                }

                dr.Close();
            }

            return sifra;
        }

        public ActionResult Logout()
        {
            IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut(MjenicaAuthType.AuthType);

            return RedirectToAction("Index", "Naslovna");
        }
        
    }
}