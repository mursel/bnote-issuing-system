using Dapper;
using mjenica.Areas.admin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace mjenica.DAL
{
    public class MjenicaService
    {
        private static MjenicaService _instance;
        string confString = ConfigurationManager.ConnectionStrings["MjeniceDb"].ConnectionString;

        public static MjenicaService Instance
        {
            get
            {
                if (_instance==null)
                {
                    _instance = new MjenicaService();
                }
                return _instance;
            }
        }

        public List<Mjenica> VratiSve()
        {
            List<Mjenica> _mjenice = new List<Mjenica>();
            using (var cn = new SqlConnection(confString))
            {
                cn.Open();

                _mjenice = cn.Query<Mjenica>("select MjenicaId, SifraKorisnika, Datum, BrojMjenice, JeValidna from IzdateMjenice").ToList();
            }
            return _mjenice;
        }

        public Mjenica GetById(int? id)
        {
            Mjenica _temp = new Mjenica();
            if (id != null)
            {
                using (var cn = new SqlConnection(confString))
                {
                    cn.Open();

                    _temp = cn.Query<Mjenica>("select MjenicaId, Datum, SifraKorisnika, BrojMjenice, JeValidna from IzdateMjenice where MjenicaId = @A", new { A = id }).First();
                }
            }

            return _temp;
        }

        //internal Mjenica GetBySN(string brojMjenice)
        //{
        //    throw new NotImplementedException();
        //}

        public ModelStatus Edit(Mjenica mjenica)
        {
            ModelStatus _tempStatus = new ModelStatus();
            if (mjenica != null)
            {
                using (var cn = new SqlConnection(confString))
                {
                    cn.Open();

                    using (var tr = cn.BeginTransaction())
                    {
                        try
                        {
                            cn.Execute("update IzdateMjenice set JeValidna=@A where MjenicaId=@B", new { A = mjenica.JeValidna, B = mjenica.MjenicaId }, transaction: tr);
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

        public string VratiZadnjiSerijskiBroj()
        {
            string _temp = string.Empty;
            using (var cn = new SqlConnection(confString))
            {
                cn.Open();

                _temp = cn.Query<string>("select CAST(BrojMjenice as int) from IzdateMjenice order by BrojMjenice desc").First();
            }
            return _temp;
        }

        public ModelStatus DodajMjenicu(string sifra, string finalSerBroj)
        {
            ModelStatus status = new ModelStatus();
            using (var cn = new SqlConnection(confString))
            {
                cn.Open();

                using (var tr = cn.BeginTransaction())
                {
                    try
                    {
                        cn.Execute("insert into IzdateMjenice (Datum, SifraKorisnika, BrojMjenice, JeValidna) values (@A, @B, @C, @D)",
                            new { A = DateTime.Now, B = sifra, C = finalSerBroj, D = 1 }, transaction: tr);
                        tr.Commit();
                    }
                    catch(Exception ex)
                    {                        
                        status.JeGreska = true;
                        status.Opis = ex.Message;
                        tr.Rollback();
                        return status;
                    }
                }
            }
            return status;
        }
    }
}