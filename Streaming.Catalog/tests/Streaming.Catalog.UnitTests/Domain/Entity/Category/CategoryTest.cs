using Streaming.Catalog.Domain.Entity;
using Streaming.Catalog.Domain.Exceptions;

namespace Streaming.Catalog.UnitTests.Domain.Entity.Category
{
    public class CategoryTest
    {
        [Fact(DisplayName = nameof(ValidCategory))]
        [Trait("Domain", "Category - Aggregates")]
        public void ValidCategory() 
        {
            // Arrange
            var validData = new 
            {
                Name = "Category name",
                Description = "Description category"
            };
            var datetimeBefore = DateTime.Now;

            // Act
            var category = new Catalog.Domain.Entity.Category(validData.Name, validData.Description);
            var datetimeAfter = DateTime.Now;

            // Assert
            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);
            Assert.Equal(validData.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt > datetimeBefore);
            Assert.True(category.CreatedAt < datetimeAfter);
            Assert.True(category.IsActive);
        }


        [Theory(DisplayName = nameof(IsActive))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void IsActive(bool isActive)
        {
            // Arrange
            var validData = new
            {
                Name = "Category name",
                Description = "Description category"
            };
            var datetimeBefore = DateTime.Now;

            // Act
            var category = new Catalog.Domain.Entity.Category(validData.Name, validData.Description, isActive);
            var datetimeAfter = DateTime.Now;

            // Assert
            Assert.NotNull(category);
            Assert.Equal(validData.Name, category.Name);
            Assert.Equal(validData.Description, category.Description);
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
            Action action = () => new Catalog.Domain.Entity.Category(name!, "Description category");
            
            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Name should not be empty or null", exception.Message);
        }


        [Fact(DisplayName = nameof(ErrorWhenDescriptionIsNull))]
        [Trait("Domain", "Category - Aggregates")]
        public void ErrorWhenDescriptionIsNull()
        {
            Action action = () => new Catalog.Domain.Entity.Category("Category Name", null!);

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
            Action action = () => new Catalog.Domain.Entity.Category(invalidName, "Category Description Ok");

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
            var validData = new
            {
                Name = "Category name",
                Description = "Description category"
            };
            var datetimeBefore = DateTime.Now;

            // Act
            var category = new Catalog.Domain.Entity.Category(validData.Name, validData.Description, false);
            category.Activate();
            // Assert
            Assert.True(category.IsActive);
        }


        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Deactivate()
        {
            // Arrange
            var validData = new
            {
                Name = "Category name",
                Description = "Description category"
            };
            var datetimeBefore = DateTime.Now;

            // Act
            var category = new Catalog.Domain.Entity.Category(validData.Name, validData.Description, true);
            category.Deactivate();
            // Assert
            Assert.False(category.IsActive);
        }


        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update() 
        {
            var category = new Catalog.Domain.Entity.Category("Category Name", "Category Description");
            var newValues = new { Name = "New Name", Description = "New Description" };

            category.Update(newValues.Name, newValues.Description);

            Assert.Equal(newValues.Name, category.Name);
            Assert.Equal(newValues.Description, category.Description);
        }


        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyName()
        {
            var category = new Catalog.Domain.Entity.Category("Category Name", "Category Description");
            var newValues = new { Name = "New Name"};
            var currenteDescription = category.Description;

            category.Update(newValues.Name);

            Assert.Equal(newValues.Name, category.Name);
            Assert.Equal(currenteDescription, category.Description);
        }


        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  ")]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var category = new Catalog.Domain.Entity.Category("Category Name", "Category Description");
            
            Action action = () => category.Update(name!);
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
            var category = new Catalog.Domain.Entity.Category("Category Name", "Category Description");

            Action action = () => category.Update(invalidName);
            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Name should be at leats 3 characters long", exception.Message);
        }


        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenNameIsGreaterThan255Characters()
        {
            var category = new Catalog.Domain.Entity.Category("Category Name", "Category Description");

            var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
            Action action = () => category.Update(invalidName);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
        }


        [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
        {
            var category = new Catalog.Domain.Entity.Category("Category Name", "Category Description");

            var invalidDescription = String.Join(null, Enumerable.Range(0, 10_001).Select(_ => "a").ToArray());
            Action action = () => category.Update("Category New Name", invalidDescription);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
        }
    }
}