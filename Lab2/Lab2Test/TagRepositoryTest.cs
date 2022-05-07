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
                TagId = 40,
                Name = "for boss",
                Color = 3
            };
            var tagRepository = new TagRepository();
            Assert.Equal(40, tagRepository.AddTag(tag));
            tagRepository.RemoveTag(40);
        }

        [Fact]
        public void RemoveTagTest()
        {
            var tag = new Tags
            {
                TagId = 4,
                Name = "for boss",
                Color = 3
            };
            TagRepository repository = new();
            repository.AddTag(tag);
            Assert.Equal(4, repository.RemoveTag(4));
        }

        [Fact]
        public void UpdateTagTest()
        {
            var tag = new Tags
            {
                TagId = 8,
                Name = "next",
                Color = 3
            };

            TagRepository repository = new();
            repository.AddTag(tag);
            Assert.Equal(8, repository.UpdateTag(8, tag));
            repository.RemoveTag(8);
        }
    }
}
