using TaskListKhvatskova.Models;
using TaskListKhvatskova.Repositories;
using Xunit;

namespace TaskListKhvatskovaTests
{
    public class TagRepositoryTests
    {
        [Fact]
        public void AddTagWithoutTest()
        {
            TagRepository tagRepository = new();
            var tag = new Tags
            {
                Name = "for Chief",
                Color = "green"
            };
            tagRepository.RemoveAllTags();
            var tagId = tagRepository.AddTag(tag);
            Assert.Equal(1, tagId);
            tagRepository.RemoveTag(tagId);
        }

        [Fact]
        public void AddTagTest()
        {
            TagRepository tagRepository = new();
            var tag = new Tags
            {
                TagId = 100,
                Name = "for Chief",
                Color = "green"
            };
            Assert.Equal(100, tagRepository.AddTag(tag));
            tagRepository.RemoveTag(100);
        }

        [Fact]
        public void RemoveTagTest()
        {
            var tag = new Tags
            {
                TagId = 14,
                Name = "for Chief",
                Color = "green"
            };
            TagRepository repository = new();
            repository.AddTag(tag);
            Assert.Equal(14, repository.RemoveTag(14));
        }

        [Fact]
        public void UpdateTagTest()
        {
            var tag = new Tags
            {
                TagId = 200,
                Name = "next",
                Color = "green"
            };
            TagRepository repository = new();
            repository.AddTag(tag);
            Assert.Equal(200, repository.UpdateTag(200, tag));
            repository.RemoveTag(200);
        }
    }
}
