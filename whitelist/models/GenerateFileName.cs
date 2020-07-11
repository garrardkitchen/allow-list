using System;

namespace whitelist.models
{
    public class GenerateFileName
    {
        private readonly DateTime _date;

        public GenerateFileName(DateTime date)
        {
            _date = date;
        }

        public string Create()
        {
            return $"ServiceTags_Public_{_date:yyyyMMdd}";
        }
    }
}