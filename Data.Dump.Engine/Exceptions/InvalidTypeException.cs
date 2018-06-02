using System;

namespace Data.Dump.Exceptions
{
    public class InvalidTypeException : ArgumentException
    {
        public override string ParamName { get; }
        public Type InvalidType { get; }
        public Type ExpectedType { get; }

        public override string Message 
            => $"{ParamName} of type {InvalidType} is not assignable to {ExpectedType}.";

        public InvalidTypeException(string paramName, Type invalidType, Type expectedType)
        {
            ParamName = paramName;
            InvalidType = invalidType;
            ExpectedType = expectedType;
        }
    }
}
