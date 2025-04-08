using System.Net;

namespace Application.Exceptions
{
    public class BaseApplicationException : ApplicationException
    {
        public BaseApplicationException(string message) : base(message) { }
        public BaseApplicationException(string message, Exception innerException) : base(message, innerException) { }

        public virtual HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public virtual string Title => "Application Exception occured.";
    }
}
