using Explorer.BuildingBlocks.Tests;

namespace Explorer.Shopping.Tests
{
    public class BaseShoppingIntegrationTest : BaseWebIntegrationTest<ShoppingTestFactory>
    {
        public BaseShoppingIntegrationTest(ShoppingTestFactory factory) : base(factory)
        {
        }
    }
}
