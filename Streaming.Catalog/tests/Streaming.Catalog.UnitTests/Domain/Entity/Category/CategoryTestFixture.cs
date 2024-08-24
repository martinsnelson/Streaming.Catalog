namespace Streaming.Catalog.UnitTests.Domain.Entity.Category
{
    public class CategoryTestFixture
    {
        public Catalog.Domain.Entity.Category GetValidCategory() 
            => new("Category Name", "Category Description");
    }

    [CollectionDefinition(nameof(CategoryTestFixture))]
    public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture> 
    { }
}
