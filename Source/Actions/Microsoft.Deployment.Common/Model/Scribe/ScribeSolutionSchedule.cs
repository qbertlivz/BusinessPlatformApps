namespace Microsoft.Deployment.Common.Model.Scribe
{
    public class TimesStructure
    {
        public string timeOption;
        public int? hourlyMinutes;
        public int? timeInterval;
        public string timeIntervalUnit;
        public string timeOfDay;

        public TimesStructure(bool isHourly)
        {
            timeOption = isHourly ? "HourlyMinutes" : "TimeOfDay";
            hourlyMinutes = isHourly ? 10 : hourlyMinutes;
            timeOfDay = isHourly ? timeOfDay : "01:00";
        }
    }

    public class DaysStructure
    {
        public int? daysInterval = 1;
        public int?[] daysOfMonth;
        public string[] daysOfWeek;
        public string daysIntervalStartDate = "2016-01-01";
        public string daysOption = "DaysInterval";
        public bool? lastDayOfMonth;
    }

    public class RecurringOptionsStructure
    {
        public DaysStructure days = new DaysStructure();
        public TimesStructure times;
        public string timeZone = "Coordinated Universal Time";

        public RecurringOptionsStructure(bool isHourly)
        {
            times = new TimesStructure(isHourly);
        }
    }

    public class ScribeSolutionSchedule
    {
        public RecurringOptionsStructure recurringOptions;
        public string runOnceOptions;
        public string scheduleOption;
        public string solutionId;

        public ScribeSolutionSchedule(string frequency)
        {
            switch (frequency)
            {
                case "Hourly":
                    recurringOptions = new RecurringOptionsStructure(true);
                    scheduleOption = "Recurring";
                    break;
                case "Daily":
                    recurringOptions = new RecurringOptionsStructure(false);
                    scheduleOption = "Recurring";
                    break;
                case "None":
                    scheduleOption = "OnDemand";
                    break;
            }
        }
    }
}