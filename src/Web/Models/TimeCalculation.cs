using System.ComponentModel.DataAnnotations;

namespace ShouldIAutomateThat.Models
{
    public enum TimeUnit
    {
        Minutes,
        Hours,
        Days,
        Weeks
    }

    public enum TimeFrequency
    {
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    public class TimeCalculation
    {
        [Required]
        public decimal TimeSaved { get; set; }
        
        [Required]
        public TimeUnit TimeSavedUnit { get; set; }
        
        [Required]
        public TimeFrequency HowOften { get; set; }
        
        [Required]
        public decimal TimeSpent { get; set; }
        
        [Required]
        public TimeUnit TimeSpentUnit { get; set; }

        [Range(1, 10)]
        public decimal FudgeFactor { get; set; } = 1.5m;
        
        [Range(1, 100)]
        public decimal TimeHorizonInYears { get; set; } = 5;
        
        public int NumberOfPeople { get; set; } = 1;
        
        public decimal CostOfYourTime { get; set; } = 100;
        
        public decimal CostOfBeneficiariesTime { get; set; } = 100;

        public TimeCalculationResult CalculateResult()
        {
            var timeSaved = CalculateTimeSaved();
            var timeSpent = CalculateTimeSpent();
            var result = new TimeCalculationResult()
            {
                HoursSaved = timeSaved,
                HoursSpent = timeSpent,
                CostSaved = CalculateCostSaved(timeSaved),
                CostToImplement = CalculateCostToImplement(timeSpent)
            };

            return result;
        }

        private decimal CalculateTimeSaved()
        {
            return Math.Round(CalculateHoursPerYear(ConvertToHours(TimeSaved, TimeSavedUnit), HowOften) * TimeHorizonInYears,2);
        }

        private decimal CalculateCostSaved(decimal timeSavedInHours)
        {
            return Math.Round(NumberOfPeople * timeSavedInHours * CostOfBeneficiariesTime, 2);
        }

        private decimal CalculateTimeSpent()
        {
            return Math.Round(ConvertToHours(TimeSpent, TimeSpentUnit) * FudgeFactor, 2);
        }

        private decimal CalculateCostToImplement(decimal timeSpentInHours)
        {
            return Math.Round(timeSpentInHours * CostOfYourTime, 2);
        }

        private static decimal CalculateHoursPerYear(decimal hours, TimeFrequency frequency)
        {
            // Assumptions: 8 hours per day, 5 days per week, 50 weeks per year
            return frequency switch
            {
                TimeFrequency.Hourly => hours * 8 * 5 * 50,
                TimeFrequency.Daily => hours * 5 * 50,
                TimeFrequency.Weekly => hours * 50,
                TimeFrequency.Monthly => hours * 12,
                TimeFrequency.Yearly => hours,
                _ => throw new Exception("Unknown time frequency"),
            };
        }

        private static decimal ConvertToHours(decimal time, TimeUnit unit)
        {
            // Assumptions: 8 hours per day, 5 days per week, 50 weeks per year
            return unit switch
            {
                TimeUnit.Minutes => time / 60,
                TimeUnit.Hours => time,
                TimeUnit.Days => time * 8,
                TimeUnit.Weeks => time * 8 * 5,
                _ => throw new Exception("Unknown time unit"),
            };
        }
    }
}