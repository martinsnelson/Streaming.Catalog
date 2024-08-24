using FluentAssertions;
using Streaming.Catalog.Domain.Entity;
using Streaming.Catalog.Domain.Exceptions;

namespace Streaming.Catalog.UnitTests.Domain.Entity.Category
{
    [Collection(nameof(CategoryTestFixture))]
    public class CategoryTest
    {

        private readonly CategoryTestFixture _categoryTestFixture;

        public CategoryTest(CategoryTestFixture categoryTestFixture) 
            => _categoryTestFixture = categoryTestFixture;

        [Fact(DisplayName = nameof(ValidCategory))]
        [Trait("Domain", "Category - Aggregates")]
        public void ValidCategory() 
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();
            var datetimeBefore = DateTime.Now;

            // Act
            var category = new Catalog.Domain.Entity.Category(validCategory.Name, validCategory.Description);
            var datetimeAfter = DateTime.Now.AddSeconds(1);

            // Assert
            category.Should().NotBeNull();
            category.Name.Should().Be(validCategory.Name);
            category.Description.Should().Be(validCategory.Description);
            category.Id.Should().NotBeEmpty();
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
            (category.CreatedAt > datetimeBefore).Should().BeTrue();
            (category.CreatedAt < datetimeAfter).Should().BeTrue();
            (category.IsActive).Should().BeTrue();
        }


        [Theory(DisplayName = nameof(IsActive))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void IsActive(bool isActive)
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();
            var datetimeBefore = DateTime.Now;

            // Act
            var category = new Catalog.Domain.Entity.Category(validCategory.Name, validCategory.Description, isActive);
            var datetimeAfter = DateTime.Now;

            // Assert
            Assert.NotNull(category);
            Assert.Equal(validCategory.Name, category.Name);
            Assert.Equal(validCategory.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
            Assert.Equal(isActive, category.IsActive);
        }


        [Theory(DisplayName = nameof(ErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  ")]
        public void ErrorWhenNameIsEmpty(string? name) 
        {
            var validCategory = _categoryTestFixture.GetValidCategory();


            Action action = () => new Catalog.Domain.Entity.Category(name!, validCategory.Description);
            
            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Name should not be empty or null", exception.Message);
        }


        [Fact(DisplayName = nameof(ErrorWhenDescriptionIsNull))]
        [Trait("Domain", "Category - Aggregates")]
        public void ErrorWhenDescriptionIsNull()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            Action action = () => new Catalog.Domain.Entity.Category(validCategory.Name, null!);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Description should not be null", exception.Message);
        }

        [Theory(DisplayName = nameof(ErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("a")]
        [InlineData("ca")]
        public void ErrorWhenNameIsLessThan3Characters(string invalidName) 
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            Action action = () => new Catalog.Domain.Entity.Category(invalidName, validCategory.Description);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Name should be at leats 3 characters long", exception.Message);
        }


        [Fact(DisplayName = nameof(ErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void ErrorWhenNameIsGreaterThan255Characters()
        {
            var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
             Action action = () => new Catalog.Domain.Entity.Category(invalidName, "Category Description Ok");

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
        }


        [Fact(DisplayName = nameof(ErrorWhenDescriptionIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void ErrorWhenDescriptionIsGreaterThan10_000Characters()
        {
            var invalidDescription = String.Join(null, Enumerable.Range(0, 10_001).Select(_ => "a").ToArray());
            Action action = () => new Catalog.Domain.Entity.Category("Category Name", invalidDescription);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
        }


        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Activate()
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();
            var datetimeBefore = DateTime.Now;

            // Act
            var category = new Catalog.Domain.Entity.Category(validCategory.Name, validCategory.Description, false);
            category.Activate();
            // Assert
            Assert.True(category.IsActive);
        }


        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Deactivate()
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();
            var datetimeBefore = DateTime.Now;

            // Act
            var category = new Catalog.Domain.Entity.Category(validCategory.Name, validCategory.Description, true);
            category.Deactivate();
            // Assert
            Assert.False(category.IsActive);
        }


        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update() 
        {
            var validCategory = _categoryTestFixture.GetValidCategory();
            var newValues = new { Name = "New Name", Description = "New Description" };

            validCategory.Update(newValues.Name, newValues.Description);

            Assert.Equal(newValues.Name, validCategory.Name);
            Assert.Equal(newValues.Description, validCategory.Description);
        }


        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyName()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();
            var newValues = new { Name = "New Name"};
            var currenteDescription = validCategory.Description;

            validCategory.Update(newValues.Name);

            Assert.Equal(newValues.Name, validCategory.Name);
            Assert.Equal(currenteDescription, validCategory.Description);
        }


        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  ")]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var validCategory = _categoryTestFixture.GetValidCategory();
            
            Action action = () => validCategory.Update(name!);
            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Name should not be empty or null", exception.Message);
        }


        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("a")]
        [InlineData("ca")]
        public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            Action action = () => validCategory.Update(invalidName);
            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Name should be at leats 3 characters long", exception.Message);
        }


        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenNameIsGreaterThan255Characters()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
            Action action = () => validCategory.Update(invalidName);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
        }


        [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var invalidDescription = String.Join(null, Enumerable.Range(0, 10_001).Select(_ => "a").ToArray());
            Action action = () => validCategory.Update("Category New Name", invalidDescription);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
        }
    }
}