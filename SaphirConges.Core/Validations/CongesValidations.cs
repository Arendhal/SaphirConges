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
       
        public DifferenceInDays(String startDate, String endDate)
                : base("Le nombre de jour doit etre la difference entre 'StartDate' et 'EndDate'")
        {
            d1 = startDate;
            d2 = endDate;  
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                // throw new ArgumentNullException("value", "Value parameter is required.");
                Convert.ToSingle(value);
            }
            var start = validationContext.ObjectType.GetProperty(d1); //get StartDate;
            var end = validationContext.ObjectType.GetProperty(d2);//get endDate
            var Date1 = (DateTime)start.GetValue(validationContext.ObjectInstance, null);
            var Date2 = (DateTime)end.GetValue(validationContext.ObjectInstance, null);
            float Days=0;
            Double diff;
            diff = (Date2 - Date1).TotalDays + 1;
            

           if(value == null )
            {
               Days = (float)diff;
            }
            Days = Convert.ToSingle(value);
            if (Days != diff)
            {
                var msg = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(msg);
            }
            
            return null;
        }
    }
}
