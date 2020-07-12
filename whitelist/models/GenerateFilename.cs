using System;

namespace whitelist.models
{
    public interface IGenerateFilename
    {
        string Create();
    }

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

            if (_date.DayOfWeek != MagicDate.DayOfWeek)
            {
                int diff = MagicDate.DayOfWeek - _date.DayOfWeek;
                if (diff > 0)
                {
                    newDate = newDate.AddDays(-(7-diff));
                }
                else
                {
                    newDate = newDate.AddDays(diff);
                }            
            }

            return $"ServiceTags_Public_{newDate:yyyyMMdd}";
        }
    }
}