using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsAtBitmarck.Data
{
    public class GreetingRepository : IGreetingRepository
    {
        public string GetGreeting(IDateTimeProvider dateTime)
        {
            return dateTime.Now.Hour switch
            {
                < 12 => "Good Morning",
                < 17 => "Good Afternoon",
                _ => "Good Evening"
            };
        }
    }
}
