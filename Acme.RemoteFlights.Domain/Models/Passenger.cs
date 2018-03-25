using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.RemoteFlights.Domain.Models
{
    public class Passenger
    {
        public Guid Id { get; internal set; }
        public Guid Name { get; internal set; }
    }
}
