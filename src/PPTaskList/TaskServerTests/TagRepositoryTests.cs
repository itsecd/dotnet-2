using PPTask.Model;
using PPTask.Repositories;
using Xunit;

namespace ServerTest
{
    public class TagRepositoryTests
    {
        [Fact]
        public void AddTagTest()
        {
            var tag1 = new Tag
            {
                TagStatus = "Done",
                TagColour = "Green"
            };
            var repository = new JsonTagRepository();

            repository.RemoveAllTags();
            repository.AddTag(tag1);

            Assert.Equal(1, repository.GetTags()[0].TagId);
            Assert.Equal("Done", repository.GetTags()[0].TagStatus);
            Assert.Equal("Green", repository.GetTags()[0].TagColour);

            repository.RemoveAllTags();
        }

        [Fact]
        public void AddFullTagTest()
        {
            var tag1 = new Tag(1, "Done", "Green");
            var repository = new JsonTagRepository();

            repository.RemoveAllTags();
            repository.AddTag(tag1);

            Assert.Equal(1, repository.GetTags()[0].TagId);
            Assert.Equal("Done", repository.GetTags()[0].TagStatus);
            Assert.Equal("Green", repository.GetTags()[0].TagColour);

            repository.RemoveAllTags();
        }

        [Fact]
        public void RemoveTagTest()
        {
            var tag1 = new Tag(1, "Done", "Green");
            var repository = new JsonTagRepository();

            repository.AddTag(tag1);
            repository.RemoveTag(1);

            Assert.DoesNotContain(tag1, repository.GetTags());
        }

        [Fact]
        public void RemoveAllTagsTest()
        {
            var tag1 = new Tag(1, "Done", "Green");
            var tag2 = new Tag(2, "Immediately", "Red");
            var repository = new JsonTagRepository();

            repository.AddTag(tag1);
            repository.AddTag(tag2);
            repository.RemoveAllTags();

            Assert.DoesNotContain(tag1, repository.GetTags());
            Assert.DoesNotContain(tag2, repository.GetTags());
        }
    }
}
