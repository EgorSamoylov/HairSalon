using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class EntityUpdateException : BaseApplicationException
    {
        public EntityUpdateException(string message) : base(message)
        {

        }

        public EntityUpdateException(string message, Exception inner) : base(message, inner)
        {

        }
        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public override string Title => "Updating Entity failed.";
    }
}
