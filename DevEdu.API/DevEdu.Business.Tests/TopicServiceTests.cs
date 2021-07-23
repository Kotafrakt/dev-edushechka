using System.Collections.Generic;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using NUnit.Framework;
using Moq;

namespace DevEdu.Business.Tests
{
    public class TopicServiceTests
    {
        private Mock<ITopicRepository> _topicRepoMock;
        private Mock<ITagRepository> _tagRepoMock;

        [SetUp]
        public void Setup()
        {
            _topicRepoMock = new Mock<ITopicRepository>();
            _tagRepoMock = new Mock<ITagRepository>();
        }

        [Test]
        public void AddTopic_SimpleDtoWithoutTags_TopicCreated()
        {
            //Given
            var expectedTopicId = 77;
            var topicDto = new TopicDto {Name = "Topic1", Duration = 5};

            _topicRepoMock.Setup(x => x.AddTopic(topicDto)).Returns(expectedTopicId);
            _topicRepoMock.Setup(x => x.AddTagToTopic(It.IsAny<int>(), It.IsAny<int>()));

            var sut = new TopicService(_topicRepoMock.Object, _tagRepoMock.Object);

            //When
            var actualTopicId = sut.AddTopic(topicDto);

            //Than
            Assert.AreEqual(expectedTopicId, actualTopicId);
            _topicRepoMock.Verify(x=> x.AddTopic(topicDto), Times.Once);
            _topicRepoMock.Verify(x => x.AddTagToTopic(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddTopic_DtoWithTags_TopicWithTagsCreated()
        {
            //Given
            var expectedTopicId = 77;
            var topicDto = new TopicDto
            {
                Name = "Topic1",
                Duration = 5,
                Tags = new List<TagDto>
                {
                    new TagDto{ Id = 1 },
                    new TagDto{ Id = 2 },
                    new TagDto{ Id = 3 }
                }
            };

            _topicRepoMock.Setup(x => x.AddTopic(topicDto)).Returns(expectedTopicId);
            _topicRepoMock.Setup(x => x.AddTagToTopic(expectedTopicId, It.IsAny<int>()));
            _topicRepoMock.Setup(x => x.GetTopic(expectedTopicId)).Returns(topicDto);
            _tagRepoMock.Setup(x => x.SelectTagById(It.IsAny<int>())).Returns(topicDto.Tags[0]);

            var sut = new TopicService(_topicRepoMock.Object, _tagRepoMock.Object);

            //When
            var actualTopicId = sut.AddTopic(topicDto);

            //Than
            Assert.AreEqual(expectedTopicId, actualTopicId);
            _topicRepoMock.Verify(x => x.AddTopic(topicDto), Times.Once);
            _topicRepoMock.Verify(x => x.AddTagToTopic(expectedTopicId, It.IsAny<int>()), Times.Exactly(topicDto.Tags.Count));
        }

        [Test]
        public void AddTagToTopic_WhenTopicNotFound_EntityNotFoundException()
        {
            _topicRepoMock.Setup(x => x.AddTagToTopic(TopicData.expectedTopicId, TagData.expectedTagId)).Throws(new EntityNotFoundException($"topic with id = {TopicData.expectedTopicId} was not found"));

            var sut = new TopicService(_topicRepoMock.Object, _tagRepoMock.Object);

            EntityNotFoundException ex = Assert.Throws<EntityNotFoundException>(
                () => sut.AddTagToTopic(TopicData.expectedTopicId, TagData.expectedTagId));
            Assert.That(ex.Message, Is.EqualTo($"topic with id = {TopicData.expectedTopicId} was not found"));

            _topicRepoMock.Verify(x => x.AddTagToTopic(TopicData.expectedTopicId, TagData.expectedTagId), Times.Never);
            _topicRepoMock.Verify(x => x.GetTopic(TopicData.expectedTopicId), Times.Exactly(1));
            _tagRepoMock.Verify(x => x.SelectTagById(TagData.expectedTagId), Times.Never);
        }

        [Test]
        public void AddTagToTopic_WhenTagNotFound_EntityNotFoundException()
        {
            _topicRepoMock.Setup(x => x.AddTagToTopic(TopicData.expectedTopicId, TagData.expectedTagId)).Throws(new EntityNotFoundException($"tag with id = {TagData.expectedTagId} was not found"));
            _topicRepoMock.Setup(x => x.GetTopic(TopicData.expectedTopicId)).Returns(TopicData.GetTopicDtoWithTags);

            var sut = new TopicService(_topicRepoMock.Object, _tagRepoMock.Object);

            EntityNotFoundException ex = Assert.Throws<EntityNotFoundException>(
                () => sut.AddTagToTopic(TopicData.expectedTopicId, TagData.expectedTagId));
            Assert.That(ex.Message, Is.EqualTo($"tag with id = {TagData.expectedTagId} was not found"));

            _topicRepoMock.Verify(x => x.AddTagToTopic(TopicData.expectedTopicId, TagData.expectedTagId), Times.Never);
            _topicRepoMock.Verify(x => x.GetTopic(TopicData.expectedTopicId), Times.Exactly(1));
            _tagRepoMock.Verify(x => x.SelectTagById(TagData.expectedTagId), Times.Exactly(1));
        }
    }
}