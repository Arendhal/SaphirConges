using System;
using System.Linq;
using SalesFirst.Core.Model;
using SaphirCongesCore.Data;
using SaphirCongesCore.Models;
using SalesFirst.Core.Service;
using System.Collections.Generic;
using System.Web;

namespace SaphirCongesCore.Utils
{
    public static class Utils
    {
        //Renvoie une string de classes css qui definisse les couleurs des types de conges
        public static string CongesColors()
        {
            string cssClass = "";
            string css = "";
            SaphirCongesDB db = new SaphirCongesDB();
            List<CongesDescription> list = db.GetAllCongesDescriptions.ToList();
            foreach (var congestype in list)
            {
                cssClass = "#year-calendar ." + congestype.TypeConge + "{";
                cssClass += "background-color:" + congestype.CongesColor + ";";
                cssClass += "color: white;}";
                css += cssClass;
            }
            return css;
        }

        //Renvoie la couleur des conges
        public static string GetCongesTypeColor(string CongesType)
        {
            string color = "";
            SaphirCongesDB db = new SaphirCongesDB();
            List<CongesDescription> list = db.GetAllCongesDescriptions.ToList();
            foreach (var congestype in list)
            {
                if (congestype.TypeConge == CongesType)
                {
                    color = congestype.CongesColor;
                }
            }
            return color;
        }

        //Check solde conges d'un employe
        public static double GetTotalSold(Employee employe)
        {
            SaphirCongesDB db = new SaphirCongesDB();
            EmployeQuota quotas = db.GetEmployeQuotaByEmploye(employe);
            double result = quotas.PaidQuota;
            return result;
        }

        public static DateTime SetAnneeActuelle(DateTime date)
        {
            return new DateTime(DateTime.Now.Year, date.Month, date.Day);
        }



        public static List<CongesEnum> ListeCongesComplete(int year)
        {
            SaphirCongesDB db = new SaphirCongesDB();
            List<Conges> list = db.GetCongesNonRefuses().ToList();
            List<CongesEnum> newlist = new List<CongesEnum>().ToList();
            foreach (var item in list)
            {
                DateTime startDate = item.StartDate;
                DateTime endDate = item.EndDate;
                TimeSpan diff = endDate - startDate;
                int days = diff.Days;
                bool b = false;
                for (int i = 0; i <= days; i++)
                {
                    var tmp = i;
                    var testDate = startDate.AddDays(i);
                    if (testDate.Year == year)
                    {
                        CongesEnum congesEnum = new CongesEnum();
                        congesEnum.day = 1;
                        if (item.HalfDay != null && b == false && tmp == days)
                        {
                            congesEnum.day = congesEnum.day / 2;
                            b = true;
                        }
                        //congesEnum.name = item.Employe.Username.ToString();
                        congesEnum.date = testDate;
                        congesEnum.type = item.TypeConges;
                        congesEnum.Statut = item.Statut;

                        newlist.Add(congesEnum);
                    }
                }
            }
            List<CongesGeneral> list2 = db.GetAllCongesGeneral.ToList();
            foreach (var item in list2)
            {
                DateTime startDate = item.StartDate;
                DateTime endDate = item.EndDate;
                TimeSpan diff = endDate - startDate;
                int days = diff.Days;
                for (int i = 0; i <= days; i++)
                {
                    var testDate = startDate.AddDays(i);
                    if (testDate.Year == year)
                    {
                        CongesEnum congesEnum = new CongesEnum();
                        congesEnum.name = item.Nom;
                        congesEnum.date = testDate;
                        congesEnum.type = item.Type;
                        congesEnum.Statut = item.Nom;
                        congesEnum.day = 1;
                        newlist.Add(congesEnum);
                    }
                }
            }
            return newlist.OrderBy(r => r.date).ToList();
        }

        public static List<Conges> ListeCongesAnnuels(Employee employe, int year)
        {
            SaphirCongesDB db = new SaphirCongesDB();
            List<Conges> list = db.GetCongesAccepteByEmploye(employe).ToList();
            List<Conges> newliste = new List<Conges>();

            foreach (var item in list)
            {
                DateTime startDate = item.StartDate;
                DateTime endDate = item.EndDate;
                TimeSpan diff = endDate - startDate;
                int days = diff.Days;
              
                bool b = false;
                for (int i = 0; i <= days; i++)
                {
                    var testDate = startDate.AddDays(i);
                    if (testDate.Year == year )
                    {
                        Conges conges = new Conges();
                        conges.BookedBy = item.BookedBy;
                        conges.BookingDate = item.BookingDate;
                        conges.Employe = item.Employe;
                        conges.EndDate = testDate;
                        conges.CongesDescription = item.CongesDescription;
                        conges.CongesID = item.CongesID;
                        conges.TypeConges = item.TypeConges;
                        conges.NoOfDays = 1;
                        conges.StartDate = testDate;
                        conges.Statut = item.Statut;
                        conges.HalfDay = item.HalfDay;
                        
                        if (conges.HalfDay != null && b == false)
                        {
                            conges.NoOfDays = conges.NoOfDays / 2;
                            b = true;
                        }
                        if (testDate.DayOfWeek == DayOfWeek.Saturday || testDate.DayOfWeek == DayOfWeek.Sunday)
                        {
                            conges.NoOfDays--;
                        }
                        newliste.Add(conges);
                    }
                }
            }
            return newliste;
        }



        public static float CongesPris(Employee employe)
        {
            SaphirCongesDB db = new SaphirCongesDB();
            List<Conges> list = db.GetCongesAccepteByEmploye(employe).ToList();
            float res = list.Select(s => s.NoOfDays).Sum();
            return res;
        }

        //Conges posés pour une année
        public static float CongesPosesInYear(Employee employe, int year)
        {
            SaphirCongesDB db = new SaphirCongesDB();
            List<Conges> list = db.GetCongesAccepteByEmploye(employe).ToList();
            List<Conges> newList = Utils.ListeCongesAnnuels(employe, year);
            float result = newList.Select(r => r.NoOfDays).Sum();
            return result;
        }

        //Deduit les weekend et jours feries du nombre total de conges pris
        public static int ActualCongesPris(Employee employe)
        {
            SaphirCongesDB db = new SaphirCongesDB();
            List<Conges> list = db.GetCongesAccepteByEmploye(employe).ToList();
            var weekends = 0;

            foreach (var item in list)
            {
                DateTime startDate = item.StartDate;
                DateTime endDate = item.EndDate;
                TimeSpan diff = endDate - startDate;
                int days = diff.Days;
                for (int i = 0; i <= days; i++)
                {
                    var testDate = startDate.AddDays(i);
                    switch (testDate.DayOfWeek)
                    {
                        case DayOfWeek.Saturday:
                            weekends++;
                            break;
                        case DayOfWeek.Sunday:
                            weekends++;
                            break;
                    }
                }
            }
            return weekends;
        }



        //Retourne les differents types de congés et le nombre total de jours de ces differents congés
        public static Dictionary<string, float> TypesCongesPrisInYear(string username, int year)
        {
            SaphirCongesDB db = new SaphirCongesDB();
            EmployeeService employeService = new EmployeeService(new SalesFirst.Core.Data.EmployeeRepository(new SaphirCongesCore.Data.SaphirCongesDB()));
            SalesFirst.Core.Model.Employee employe = employeService.GetEmployeeByUsername(username);
            List<Conges> list = Utils.ListeCongesAnnuels(employe, year);

            var query = list.GroupBy(n => n.TypeConges, (key, values) => new { Group = key, Count = values.Count() });
            var result = list.GroupBy(c => c.TypeConges).Select(cd =>
                        new
                        {
                            Group = cd.Key,
                            Count = cd.Count(),
                            Sum = cd.Sum(c => c.NoOfDays)
                        });
            Dictionary<string, float> groups = new Dictionary<string, float>();
            foreach (var item in result)
            {
                groups.Add(item.Group,(item.Sum));
            }
            return groups;
        }

        public static Dictionary<string, float> TypesCongesPris(string username)
        {
            SaphirCongesDB db = new SaphirCongesDB();
            EmployeeService employeService = new EmployeeService(new SalesFirst.Core.Data.EmployeeRepository(new SaphirCongesCore.Data.SaphirCongesDB()));
            SalesFirst.Core.Model.Employee employe = employeService.GetEmployeeByUsername(username);
            List<Conges> list = db.GetCongesAccepteByEmploye(employe).ToList();

            var query = list.GroupBy(r => r.TypeConges, (key, values) => new { Group = key, Count = values.Count() });
            var result = list.GroupBy(s => s.TypeConges)
                    .Select(sd =>
                        new
                        {
                            Group = sd.Key,
                            Count = sd.Count(),
                            Sum = sd.Sum(s => s.NoOfDays)
                        });
            Dictionary<string, float> groups = new Dictionary<string, float>();
            foreach (var item in result)
            {
                groups.Add(item.Group, item.Sum);
            }
            return groups;

        }

        public static Dictionary<string, string> GetCongesNomEtColors()
        {
            SaphirCongesDB db = new SaphirCongesDB();
            List<CongesDescription> list = db.GetAllCongesDescriptions.ToList();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (var congeType in list)
            {
                dictionary.Add(congeType.TypeConge, congeType.CongesColor);
            }
            return dictionary;
        }

        public static IHtmlString HTMLRaw(string source)
        {
            return new HtmlString(source);
        }

        public class CongesEnum
        {
            public string name;
            public DateTime date;
            public string Statut;
            public float day;
            public string type;
        }

    }
}

