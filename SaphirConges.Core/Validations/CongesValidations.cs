using System;
using System.ComponentModel.DataAnnotations;


namespace SaphirCongesCore.Validation
{
    public class CongesValidation : ValidationAttribute
    {
        String Prop;
        public CongesValidation(string prop)
                    : base("{0} doit être supérieur ou égal a {1}")
        {
            Prop = prop;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(ErrorMessageString, name, Prop);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propinfo = validationContext.ObjectType.GetProperty(Prop);
            var otherDate = (DateTime)propinfo.GetValue(validationContext.ObjectInstance, null);
            var thisDate = (DateTime)value;
            
            if (thisDate < otherDate)
            {
                var msg = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(msg);
            }
            return null;
        }
    }

    public class CurrentDateCheck : ValidationAttribute
    {
        public CurrentDateCheck() : base("{0} doit être supérieur ou égal a la date du jour")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var thisDate = (DateTime)value;
            var today = DateTime.Today;
            if (thisDate < today)
            {
                var msg = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(msg);
            }
            return null;
        }
    }


    public class DifferenceInDays : ValidationAttribute
    {
        public String d1 { get; set; }
        public String d2 { get; set; }
        public String isHalf { get; set; }
        public String isWeekend { get; set; }

        public DifferenceInDays(String startDate, String endDate, String halfDay)
                : base("Le nombre de jour doit etre la difference entre 'StartDate' et 'EndDate'")
        {
            d1 = startDate;
            d2 = endDate;
            isHalf = halfDay;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var start = validationContext.ObjectType.GetProperty(d1); //get StartDate;
            var end = validationContext.ObjectType.GetProperty(d2);//get endDate
            var isHalfDay = validationContext.ObjectType.GetProperty(isHalf); //check halfdays
            

            var Date1 = (DateTime)start.GetValue(validationContext.ObjectInstance, null);
            var Date2 = (DateTime)end.GetValue(validationContext.ObjectInstance, null);
            var ifHalfDay = (String)isHalfDay.GetValue(validationContext.ObjectInstance, null);

            var Days = (float)value;
            Double diff;
            diff = (Date2 - Date1).TotalDays + 1;

          /*while(Date1 <= Date2)
            {
                if (Date1.DayOfWeek == DayOfWeek.Saturday || Date1.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekenday++;
                }
                Date1 = Date1.AddDays(1);
            }
            diff = diff - weekenday;*/
            if (ifHalfDay != null || Date1.DayOfWeek == DayOfWeek.Friday || Date2.DayOfWeek == DayOfWeek.Friday)
            {
                diff -= 0.5;
            }
            if (Days != diff)
            {
                var msg = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(msg);
            }

            return null;
        }
    }
}
