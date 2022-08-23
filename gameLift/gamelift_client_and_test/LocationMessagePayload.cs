using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace gamelift_client_and_test
{
    internal class LocationMessagePayload
    {
        float x;
        float y;

        public LocationMessagePayload(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
