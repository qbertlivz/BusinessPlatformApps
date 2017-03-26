using System;
using Newtonsoft.Json;

namespace Microsoft.Deployment.Common.Model.Informatica
{
    public class InformaticaSchedule : InformaticaObject
    {
        [JsonProperty("orgId", NullValueHandling = NullValueHandling.Ignore)]
        public string OrgId;

        [JsonProperty("startTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 0, 0, DateTimeKind.Utc);

        [JsonProperty("interval", NullValueHandling = NullValueHandling.Ignore)]
        public string Interval;

        [JsonProperty("frequency")]
        public int Frequency = 1;

        [JsonProperty("mon")]
        public bool Mon = true;

        [JsonProperty("tue")]
        public bool Tue = true;

        [JsonProperty("wed")]
        public bool Wed = true;

        [JsonProperty("thu")]
        public bool Thu = true;

        [JsonProperty("fri")]
        public bool Fri = true;

        [JsonProperty("sat")]
        public bool Sat = true;

        [JsonProperty("sun")]
        public bool Sun = true;

        [JsonProperty("weekDay")]
        public const bool WeekDay = false;

        [JsonProperty("dayOfMonth")]
        public const int DayOfMonth = 0;

        [JsonProperty("timeZoneId", NullValueHandling = NullValueHandling.Ignore)]
        public const string TimeZoneId = "GMT";

        public InformaticaSchedule()
        {
            Type = "schedule";
        }

        public InformaticaSchedule(DateTime start, int frequency) : this()
        {
            this.Frequency = frequency;

            StartTime = start.ToUniversalTime();
        }
    }
}