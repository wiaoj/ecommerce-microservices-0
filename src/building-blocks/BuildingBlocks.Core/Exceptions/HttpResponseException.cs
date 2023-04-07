using System.Net;

namespace BuildingBlocks.Core.Exceptions;
// https://stackoverflow.com/questions/21097730/usage-of-ensuresuccessstatuscode-and-handling-of-httprequestexception-it-throws
public class HttpResponseException : CustomException {
	public HttpResponseException(String message) : base(message, HttpStatusCode.InternalServerError) { }
	public HttpResponseException(String message, HttpStatusCode statusCode) : base(message, statusCode) { }
}