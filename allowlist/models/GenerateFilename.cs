using System;

namespace AllowList.models
{
    /// <summary>
    /// The URI of the endpoint is updated every Monday so get new file on the next day (avoid complications with time differences etc).
    /// On Monday or before, always get the previous week's JSON
    /// </summary>
    public class GenerateFilename : IGenerateFilename
    {
        private readonly DateTime _date;

        public GenerateFilename(DateTime date)
        {
            _date = date;
        }

        public static DateTime MagicDate => new DateTime(2020, 7, 6);

        public string Create()
        {
            DateTime newDate = _date;

            int diff = MagicDate.DayOfWeek - _date.DayOfWeek;

            if (_date.DayOfWeek > MagicDate.DayOfWeek)
            {
                newDate = newDate.AddDays(diff);
            }
            else
            {
                newDate = newDate.AddDays(-(7 - diff));
            }

            return $"ServiceTags_Public_{newDate:yyyyMMdd}";
        }
    }
}