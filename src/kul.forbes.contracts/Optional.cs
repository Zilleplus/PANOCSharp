using System;

namespace kul.forbes.contracts
{
    public class Optional<TData>
    {
        private readonly TData data=default;
        private readonly bool isEmpty=true;

        public Optional()
        {
            data = default;
            isEmpty = true;
        }

        public Optional(TData data)
        {
            this.data = data;
            isEmpty = false;
        }

        public Optional<T> Map<T>(Func<TData, T> func)
            => isEmpty
                ? new Optional<T>()
                :new Optional<T>(func(data)) ;

        public TData OrElse(TData other)
            => isEmpty
            ? other
            : data;
    }
}
