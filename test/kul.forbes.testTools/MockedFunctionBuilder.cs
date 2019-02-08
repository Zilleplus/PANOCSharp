using kul.forbes.contracts;
using MathNet.Numerics.LinearAlgebra;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace kul.forbes.testTools
{
    public class MockedFunctionBuilder : IBuilder<Mock<IFunction>>
    {
        private Mock<IFunction> mock = new Mock<IFunction>();

        public MockedFunctionBuilder WithCostGradient(
            Func<Vector<double>,double> cost,
            Func<Vector<double>,Vector<double>> gradient)
        {
            Func<Vector<double>, (double, Vector<double>)> dummyFunction = location => (cost(location), gradient(location));
            mock.Setup(x => x.Evaluate(It.IsAny<Vector<double>>()))
                .Returns(dummyFunction);

            return this;
        }

        public Mock<IFunction> Build()
        {
            return mock;
        }
    }
}
