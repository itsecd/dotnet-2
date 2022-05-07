using Lab2.Models;
using Lab2.Repositories;
using Xunit;
using System.Linq;
using System;

namespace Lab2Test
{
    public class TagRepositoryTest
    {
        [Fact]
        public void AddTagWithoutTest()
        {
            TagRepository tagRepository = new ();
            var tag = new Tags
            {
                Name = "for boss",
                Color = 3
            };
            if (tagRepository.GetTags().Count == 0)
            {
                tag.TagId = 1;
                Assert.Equal(1, tagRepository.AddTag(tag));
                tagRepository.RemoveTag(1);
            }
            else
            {
                int count = tagRepository.GetTags().Count;
                Assert.Equal(count+1, tagRepository.AddTag(tag));
                tagRepository.RemoveTag(count + 1);
            }

        }

        [Fact]
        public void AddTagTest()
        {
            var tag = new Tags
            {
                TagId = 10,
                Name = "for boss",
                Color = 3
            };
            var tagRepository = new TagRepository();
            if(tagRepository.GetTags().FindIndex(t => t.TagId == 10) == -1)
            {
                Assert.Equal(10, tagRepository.AddTag(tag));
                tagRepository.RemoveTag(10);
            }
            else
            {
                throw new Exception("This ID already exists");
            }
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
    }
}
