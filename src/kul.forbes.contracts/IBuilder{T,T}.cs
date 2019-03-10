using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.contracts
{
    public interface IBuilder<Tin,Tout>
    {
        Tout Build(Tin input);
    }
}
