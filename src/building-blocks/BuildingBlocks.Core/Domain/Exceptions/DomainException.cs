using BuildingBlocks.Core.Exceptions;
using System.Net;

namespace BuildingBlocks.Core.Domain.Exceptions;
public class DomainException : CustomException {
	public DomainException(String message, params String[] errors) : this(message, HttpStatusCode.BadRequest, errors) { }
	public DomainException(String message, HttpStatusCode statusCode, params String[] errors) : base(message, statusCode, errors) { }
}