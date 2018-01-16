using System;
using System.Linq;
using System.Data.Entity;
using SaphirCongesCore.Models;
using SalesFirst.Core.Model;
using SalesFirst.Core.Data;

namespace SaphirCongesCore.Data
{
    public class SaphirCongesDB : ClientDb
    {
        public DbSet<Conges> Conges { get; set; }
        public DbSet<CongesGeneral> CongesGeneral { get; set; }
        public DbSet<CongesDescription> CongesDescription { get; set; }
        public DbSet<EmployeQuota> EmployeQuota { get; set; }
        


        public IQueryable<Conges> GetAllConges
        {
            get { return Conges; }
        }

        public IQueryable<CongesDescription> GetAllCongesDescriptions
        {
            get { return CongesDescription; }
        }

        public IQueryable<CongesDescription> GetAllEmployeCongesDescriptions
        {
            get { return CongesDescription.Where(r => r.TypePour == TypePour.CongeEmploye && r.TypeConge != "pending"); }
        }

        public IQueryable<CongesDescription> GetAllCongesGeneralDescriptions
        {
            get { return CongesDescription.Where(r => r.TypePour == TypePour.JoursFeries && r.TypeConge != "pending"); }
        }

        public IQueryable<CongesGeneral> GetAllCongesGeneral
        {
            get { return CongesGeneral; }
        }   

       
        public EmployeQuota GetEmployeQuotaByEmploye(Employee employe)
        {
            return EmployeQuota.Where(r => r.EmployeID == employe.EmployeeId).SingleOrDefault();
        }

        public IQueryable<EmployeQuota> GetAllEmployeQuota
        {
            get { return EmployeQuota; }
        }

    

        public IQueryable<Conges> GetCongesByEmploye(Employee employe)
        {
            return Conges.Where(r => r.Employe.Username == employe.Username).OrderByDescending(s => s.CongesID);
        }
        
        public IQueryable<Conges> GetCongesNonRefuseByEmploye(Employee employe)
        {
            return Conges.Where(r => r.Employe.Username == employe.Username && r.Statut != "Rejete");
        }

        public IQueryable<Conges> GetCongesNonRefuses()
        {
            return Conges.Where(r => r.Statut != "Rejete");
        }

        public IQueryable<Conges> GetCongesAccepteByEmploye(Employee employe)
        {
            if (employe != null)
            { return Conges.Where(r => r.Employe.Username == employe.Username && r.Statut == "Accepte"); }
            else
            { return null; }

        }

        public IQueryable<Conges> GetCongesAcceptes()
        {
            return Conges.Where(r => r.Statut == "Accepte").OrderByDescending(s => s.CongesID);
        }

        public IQueryable<Conges> GetCongesRefuse()
        {
            return Conges.Where(r => r.Statut == "Rejete" || r.StartDate < DateTime.Today).OrderByDescending(s => s.CongesID);
        }

        public IQueryable<Conges> GetCongesEnAttente()
        {
            return Conges.Where(r => r.Statut == null && r.StartDate >= DateTime.Today).OrderByDescending(s => s.CongesID);
        }



        public IQueryable<Conges> GetAnneeActuelleConges(Employee employe)
        {
            return Conges.Where(r => r.Statut == null && r.StartDate >= DateTime.Today).OrderByDescending(s => s.CongesID);
        }
              
    }
}
