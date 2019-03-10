using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.contracts
{
    public interface ICalculator<Tout>
    {
        Tout Calculate();
    }

    public interface ICalculator<Tin,Tout>
    {
        Tout Calculate(Tin input);
    }

    public interface ICalculator<Tin1,Tin2, Tout>
    {
        Tout Calculate(Tin1 input1,Tin2 input2);
    }
}
