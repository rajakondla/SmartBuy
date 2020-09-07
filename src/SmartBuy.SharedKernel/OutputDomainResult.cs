using System.Collections.Generic;

namespace SmartBuy.SharedKernel
{
    public class OutputDomainResult<T> where T:class
    {
        public OutputDomainResult(bool isSuccess,
            IEnumerable<string>? messages = null,
            T? entity = null)
        {
            IsSuccess = isSuccess;
            ErrorMessages = messages;
            Entity = entity;
        }

        public bool IsSuccess { get; }

        public IEnumerable<string>? ErrorMessages { get; }

        public T? Entity { get; }

    }
}
