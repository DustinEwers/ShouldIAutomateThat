namespace ShouldIAutomateThat.Models
{
    public class TimeCalculationResult
    {
        public decimal HoursSaved { get; set; }
        public decimal HoursSpent { get; set; }
        public decimal CostToImplement { get; set; }
        public decimal CostSaved { get; set; }
        public bool ShouldIAutomateThat { get { return CostSaved > CostToImplement; } }
    }

}