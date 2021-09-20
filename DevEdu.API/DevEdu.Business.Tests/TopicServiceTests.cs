using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class TopicServiceTests
    {
        private Mock<ITopicRepository> _topicRepoMock;
        private Mock<ITagRepository> _tagRepoMock;
        private ITopicService _sut;

        [SetUp]
        public void Setup()
        {
            _topicRepoMock = new Mock<ITopicRepository>();
            _tagRepoMock = new Mock<ITagRepository>();
            _sut = new TopicService(
            _topicRepoMock.Object,
            _tagRepoMock.Object,
            new TopicValidationHelper(
                _topicRepoMock.Object),
            new TagValidationHelper(_tagRepoMock.Object)
            );
        }

        [Test]
        public void AddTopic_SimpleDtoWithoutTags_TopicCreated()
        {
            //Given
            var expectedTopicId = 77;
            var topicDto = new TopicDto { Name = "Topic1", Duration = 5 };

            _topicRepoMock.Setup(x => x.AddTopicAsync(topicDto)).Returns(expectedTopicId);
            _topicRepoMock.Setup(x => x.AddTagToTopicAsync(It.IsAny<int>(), It.IsAny<int>()));


            //When
            var actualTopicId = _sut.AddTopicAsync(topicDto);

            //Than
            Assert.AreEqual(expectedTopicId, actualTopicId);
            _topicRepoMock.Verify(x => x.AddTopicAsync(topicDto), Times.Once);
            _topicRepoMock.Verify(x => x.AddTagToTopicAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddTopic_DtoWithTags_TopicWithTagsCreated()
        {
            //Given
            var expectedTopicId = 77;
            var topicDto = TopicData.GetTopicDtoWithTags();

            _topicRepoMock.Setup(x => x.AddTopicAsync(topicDto)).Returns(expectedTopicId);
            _topicRepoMock.Setup(x => x.AddTagToTopicAsync(expectedTopicId, It.IsAny<int>()));
            _topicRepoMock.Setup(x => x.GetTopicAsync(expectedTopicId)).Returns(topicDto);
            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(It.IsAny<int>())).Returns(topicDto.Tags[0]);


            //When
            var actualTopicId = _sut.AddTopicAsync(topicDto);

            //Than
            Assert.AreEqual(expectedTopicId, actualTopicId);
            _topicRepoMock.Verify(x => x.AddTopicAsync(topicDto), Times.Once);
            _topicRepoMock.Verify(x => x.AddTagToTopicAsync(expectedTopicId, It.IsAny<int>()), Times.Exactly(topicDto.Tags.Count));
        }

        [Test]
        public void DeleteTopic_IntTopicId_DeleteTopic()
        {
            //Given
            var topicDto = TopicData.GetTopicDtoWithoutTags();
            var expectedTopicId = 1;

            _topicRepoMock.Setup(x => x.DeleteTopicAsync(expectedTopicId));
            _topicRepoMock.Setup(x => x.GetTopicAsync(expectedTopicId)).Returns(topicDto);

            //When
            _sut.DeleteTopicAsync(expectedTopicId);

            //Than
            _topicRepoMock.Verify(x => x.DeleteTopicAsync(expectedTopicId), Times.Once);
            _topicRepoMock.Verify(x => x.GetTopicAsync(expectedTopicId), Times.Once);
        }
        [Test]
        public void GetTopic_IntTopicId_GetTopic()
        {
            //Given
            var topicDto = TopicData.GetTopicDtoWithoutTags();
            var topicId = 1;

            _topicRepoMock.Setup(x => x.GetTopicAsync(topicId)).Returns(topicDto);

            //When
            var dto = _sut.GetTopicAsync(topicId);

            //Than
            Assert.AreEqual(topicDto, dto);
            _topicRepoMock.Verify(x => x.GetTopicAsync(topicId), Times.Once);
        }

        [Test]
        public void GetAllTopics_NoEntries_ReturnedAllTopics()
        {
            //Given
            var expectedList = TopicData.GetListTopicDto();
            _topicRepoMock.Setup(x => x.GetAllTopicsAsync()).Returns(expectedList);

            //When
            var actualList = _sut.GetAllTopicsAsync();

            //Then
            Assert.AreEqual(expectedList, actualList);
            _topicRepoMock.Verify(x => x.GetAllTopicsAsync(), Times.Once);
        }

        [Test]
        public void UpdateTopic_TopicDto_ReturnUpdatedTopicDto()
        {
            //Given
            var topicDto = TopicData.GetTopicDtoWithoutTags();
            var topicId = 1;

            _topicRepoMock.Setup(x => x.UpdateTopicAsync(topicDto));
            _topicRepoMock.Setup(x => x.GetTopicAsync(topicId)).Returns(topicDto);

            //When
            var dto = _sut.UpdateTopicAsync(topicId, topicDto);

            //Than
            Assert.AreEqual(topicDto, dto);
            _topicRepoMock.Verify(x => x.UpdateTopicAsync(topicDto), Times.Once);
            _topicRepoMock.Verify(x => x.GetTopicAsync(topicId), Times.Exactly(2));
        }

        [Test]
        public void AddTagToTopic_WhenTopicNotFound_ThrownEntityNotFoundException()
        {
            var expectedTopicId = 77;
            var expectedTagId = 55;

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "topic", expectedTopicId)),
                () => _sut.AddTagToTopicAsync(expectedTopicId, expectedTagId));

            _topicRepoMock.Verify(x => x.AddTagToTopicAsync(expectedTopicId, expectedTagId), Times.Never);
        }

        [Test]
        public void AddTagToTopic_WhenTagNotFound_ThrownEntityNotFoundException()
        {
            var expectedTopicId = 77;
            var expectedTagId = 55;

            _topicRepoMock.Setup(x => x.GetTopicAsync(expectedTopicId)).Returns(TopicData.GetTopicDtoWithTags);

            Assert.Throws(Is.TypeOf<EntityNotFoundException>()
                .And.Message.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage, "tag", expectedTagId)),
            () => _sut.AddTagToTopicAsync(expectedTopicId, expectedTagId));

            _topicRepoMock.Verify(x => x.AddTagToTopicAsync(expectedTopicId, expectedTagId), Times.Never);
        }

        [Test]
        public void DeleteTagFromTopic_IntTopicIdAndTagId_DeleteTagFromTopic()
        {
            //Given
            var topicId = 1;
            var tagId = 13;
            var expecectedAffectedRows = 1;

            _topicRepoMock.Setup(x => x.GetTopicAsync(topicId)).Returns(TopicData.GetTopicDtoWithTags());
            _tagRepoMock.Setup(x => x.SelectTagByIdAsync(tagId)).Returns(TagData.GetTagDto());
            _topicRepoMock.Setup(x => x.DeleteTagFromTopicAsync(topicId, tagId)).Returns(expecectedAffectedRows);

            //When
            var actualAffectedRows = _sut.DeleteTagFromTopicAsync(topicId, tagId);

            //Than
            Assert.AreEqual(expecectedAffectedRows, actualAffectedRows);
            _topicRepoMock.Verify(x => x.DeleteTagFromTopicAsync(topicId, tagId), Times.Once);
        }
    }
}