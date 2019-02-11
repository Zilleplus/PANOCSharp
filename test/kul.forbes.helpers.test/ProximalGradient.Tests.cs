using kul.forbes.domain;
using kul.forbes.testTools;

namespace kul.forbes.helpers.test
{
    public class ProximalGradient
    {
        public void Given_Polynomial_Solve()
        {
            var sut = new ProximalGradient();

            var degree = 5;
            var poly = new MockedFunctionBuilder()
                .WithCostGradient(
                    (x) => x.PointwisePower(degree).Sum(),
                    (x) => 5*x.PointwisePower(degree-1))
                .Build()
                .Object;

            //var proximalOperator = new ProxBox();
        }
    }
}
