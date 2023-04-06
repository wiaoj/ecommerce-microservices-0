using System.Net;

namespace BuildingBlocks.Core.Exceptions;
public abstract class NotFoundException : CustomException {
	protected NotFoundException(String message) : base(message, HttpStatusCode.NotFound) {
	}
}