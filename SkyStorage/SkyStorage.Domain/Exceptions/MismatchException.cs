namespace SkyStorage.Domain.Exceptions;

public class MismatchException(string FirstIdentifier, string SecondIdentifier) : Exception($"The {FirstIdentifier} does not match the {SecondIdentifier}")
{
}
