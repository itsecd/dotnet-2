using Lab2.Models;
using Lab2.Repositories;
using Xunit;

namespace Lab2Test
{
    public class TagRepositoryTest
    {
        [Fact]
        public void AddTagTest()
        {
            var tag = new Tags
            {
                TagId = 2,
                Name = "for boss",
                Color = 3
            };
            var tagRepository = new TagRepository();
            Assert.Equal(2, tagRepository.AddTag(tag));
            tagRepository.RemoveTag(2);
        }

        [Fact]
        public void RemoveTagTest()
        {
            var tag = new Tags
            {
                TagId = 2,
                Name = "for boss",
                Color = 3
            };
            TagRepository repository = new();
            repository.AddTag(tag);
            Assert.Equal(2, repository.RemoveTag(2));
        }

        [Fact]
        public void UpdateTagTest()
        {
            var tag = new Tags
            {
                TagId = 2,
                Name = "next",
                Color = 3
            };

            TagRepository repository = new();
            repository.AddTag(tag);
            Assert.Equal(2, repository.UpdateTag(2, tag));
            repository.RemoveTag(2);
        }
    }
}
