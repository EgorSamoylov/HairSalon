using System.Net;

namespace Application.Exceptions
{
    public class EntityDeleteException : BaseApplicationException
    {
        public EntityDeleteException(string message) : base(message)
        {

        }

        public EntityDeleteException(string message, Exception inner) : base(message, inner)
        {

        }
        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public override string Title => "Deleting Entity failed.";
    }
}
