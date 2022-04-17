using Xunit;
using Lab2.Repositories;
using Lab2.Models;

namespace Lab2Test
{
    public class TagRepositoryTest
    {
        [Fact]
        public void AddTagTest()
        {
            var tag = new Tags
            {
                TagId = 3,
                Name = "for boss",
                Color = 3
            };
            var tagRepository = new TagRepository();
            Assert.Equal(3, tagRepository.AddTag(tag));
            tagRepository.RemoveTag(3);
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
    }
}
