using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.contracts
{
    public interface IBuilder<Tin1,Tin2,Tout>
    {
        Tout Build(Tin1 input1,Tin2 input2);
    }
}
