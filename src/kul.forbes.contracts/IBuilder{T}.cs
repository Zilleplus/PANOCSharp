using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.contracts
{
    public interface IBuilder<T>
    {
        T Builder();
    }
}
