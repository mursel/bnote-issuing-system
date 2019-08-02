using Dapper;
using mjenica.Areas.admin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace mjenica.DAL
{
    public class KorisnikService
    {
        private static KorisnikService _instance;

        string confString = ConfigurationManager.ConnectionStrings["MjeniceDb"].ConnectionString;

        public static KorisnikService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new KorisnikService();
                }
                return _instance;
            }
        }

        public KorisnikService() { }

        public ModelStatus AddNew(Korisnik obj)
        {
            ModelStatus _tempStatus = new ModelStatus();

            using (var cn = new SqlConnection(confString))
            {
                cn.Open();
                using (var tr = cn.BeginTransaction())
                {
                    try
                    {
                        cn.Execute("insert into KS_Sifre (Username, UserID) values (@A, @B)", new { A = obj.Username, B = obj.UserID }, transaction: tr);
                        tr.Commit();
                    }
                    catch(Exception e)
                    {
                        tr.Rollback();
                        _tempStatus.JeGreska = true;
                        _tempStatus.Opis = e.Message;
                    }
                    
                }
            }
            return _tempStatus;
        }

        public Korisnik GetByUserID(string userId)
        {
            Korisnik _temp = new Korisnik();
            if (!string.IsNullOrEmpty(userId))
            {
                using (var cn = new SqlConnection(confString))
                {
                    cn.Open();

                    _temp = cn.Query<Korisnik>("select Id, UserID, Username from KS_Sifre where UserID = @A", new { A = userId }).SingleOrDefault();
                }
            }

            return _temp;
        }

        public ModelStatus Edit(Korisnik korisnik)
        {
            ModelStatus _tempStatus = new ModelStatus();
            if (korisnik!=null)
            {
                using (var cn = new SqlConnection(confString))
                {
                    cn.Open();

                    using (var tr = cn.BeginTransaction())
                    {
                        try
                        {
                            cn.Execute("update KS_Sifre set UserID=@A, Username=@B where Id=@C", new { A = korisnik.UserID, B = korisnik.Username, C = korisnik.Id }, transaction: tr);
                            tr.Commit();
                        }
                        catch (Exception e)
                        {
                            tr.Rollback();
                            _tempStatus.JeGreska = true;
                            _tempStatus.Opis = e.Message;
                        }
                    }
                }
            }
            return _tempStatus;
        }

        //public ModelStatus Edit(string userId)
        //{
            
        //}

        public Korisnik GetById(int? id)
        {
            Korisnik _temp = new Korisnik();
            if (id != null)
            {
                using (var cn = new SqlConnection(confString))
                {
                    cn.Open();

                    _temp = cn.Query<Korisnik>("select Id, UserID, Username from KS_Sifre where Id = @A", new { A = id }).First();
                }
            }

            return _temp;
        }

        public string VratiSifruKorisnika(string _user)
        {
            string _temp = string.Empty;
            using (var cn = new SqlConnection(confString))
            {
                cn.Open();

                _temp = cn.Query<string>("select UserID from KS_Sifre where Username = @A", new { A = _user }).Single();
            }
            return _temp;
        }

        public List<Korisnik> VratiSve()
        {
            List<Korisnik> _temp = new List<Korisnik>();
            using (var cn = new SqlConnection(confString))
            {
                cn.Open();

                _temp = cn.Query<Korisnik>("select Id, UserID, Username from KS_Sifre").ToList();
            }
            return _temp;
        }
    }
}