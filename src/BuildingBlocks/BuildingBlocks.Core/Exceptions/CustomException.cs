using System.Net;

namespace BuildingBlocks.Core.Exceptions;
public abstract class CustomException : Exception {
	public IEnumerable<String> ErrorMessages { get; }
	public HttpStatusCode StatusCode { get; }

	public CustomException(String message, params String[] errors) : this(message, HttpStatusCode.InternalServerError, errors) { }

	public CustomException(String message, HttpStatusCode statusCode, params String[] errors) : base(message) {
		this.ErrorMessages = errors;
		this.StatusCode = statusCode;
	}
}