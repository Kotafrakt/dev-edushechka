﻿using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class CourseServiceTests
    {
        private  Mock<ICourseRepository> _courseRepositoryMock;
        private  Mock<ITopicRepository> _topicRepositoryMock;
        private  Mock<ITaskRepository> _taskRepositoryMock;
        private  Mock<IMaterialRepository> _materialRepositoryMock;
        private  Mock<ICourseValidationHelper> _courseValidationHelperMock;
        private  Mock<ITopicValidationHelper> _topicValidationHelperMock;

        [SetUp]
        public void Setup()
        {
            _courseRepositoryMock = new Mock<ICourseRepository>();
            _topicRepositoryMock = new Mock<ITopicRepository>();
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _materialRepositoryMock = new Mock<IMaterialRepository>();
            _courseValidationHelperMock = new Mock<ICourseValidationHelper>();
            _topicValidationHelperMock = new Mock<ITopicValidationHelper>();
        }
        [Test]
        public void AddTopicToCourse_WithCourseIdAndSimpleDto_TopicWasAdded() 
        {
            //Given
            var givenCourseId = 12;
            var givenTopicId = 8;
            var courseTopicDto = new CourseTopicDto { Position = 3 };

            _topicRepositoryMock.Setup(x => x.AddTopicToCourse(courseTopicDto));
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.GetTopic(givenTopicId)).Returns(new TopicDto() { Id = givenTopicId });
            var sut = new CourseService(_topicRepositoryMock.Object, 
                                        _courseRepositoryMock.Object, 
                                        _taskRepositoryMock.Object, 
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));
            //When
            sut.AddTopicToCourse(givenCourseId, givenTopicId, courseTopicDto);
            //Then
            _topicRepositoryMock.Verify(x => x.AddTopicToCourse(courseTopicDto), Times.Once);
            
        }
        [Test]
        public void AddTopicsToCourse_WithCourseIdAndListSimpleDto_TopicsWereAdded()
        {
            //Given
            var givenCourseId = 2;
            var courseTopicsDto = CourseData.GetListCourseTopicDto();
            var topicsDto = CourseData.GetTopics();
            var sut = new CourseService(_topicRepositoryMock.Object, 
                                        _courseRepositoryMock.Object, 
                                        _taskRepositoryMock.Object, 
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));

            _topicRepositoryMock.Setup(x => x.AddTopicsToCourse(courseTopicsDto));
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsDto);
            //When
            sut.AddTopicsToCourse(givenCourseId, courseTopicsDto);
            //Then
            _topicRepositoryMock.Verify(x => x.AddTopicsToCourse(courseTopicsDto), Times.Once);
        }
        [Test]
        public void DeleteTopicFromCourse_ByCourseIdAndTopicId_TopicDeletedFromCourse()
        {
            //Given
            var givenCourseId = 4;
            var givenTopicId = 7;

            var sut = new CourseService(_topicRepositoryMock.Object, 
                                        _courseRepositoryMock.Object, 
                                        _taskRepositoryMock.Object, 
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));

            _topicRepositoryMock.Setup(x => x.DeleteTopicFromCourse(givenCourseId, givenTopicId));
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.GetTopic(givenTopicId)).Returns(new TopicDto() { Id = givenTopicId });
            //When
            sut.DeleteTopicFromCourse(givenCourseId, givenTopicId);
            //Then
            _topicRepositoryMock.Verify(x => x.DeleteTopicFromCourse(givenCourseId, givenTopicId), Times.Once);
        }
        [Test]
        public void SelectAllTopicsByCourseId_ByCourseId_GotListOfCourseTopics()
        {
            //Given
            var givenCourseId = 4;

            var sut = new CourseService(_topicRepositoryMock.Object, 
                                        _courseRepositoryMock.Object, 
                                        _taskRepositoryMock.Object, 
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));

            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId));
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            //When
            sut.SelectAllTopicsByCourseId(givenCourseId);
            //Then
            _courseRepositoryMock.Verify(x => x.SelectAllTopicsByCourseId(givenCourseId), Times.Once);

        }
        [Test]
        public void UpdateCourseTopicsByCourseId_WhenCountOfTopicsNotChanged_ThenUpdateMethodCalled()
        {
            //Given
            var givenCourseId = 7;
            var givenTopicsToUpdate = CourseData.GetListCourseTopicDto();
            var courseTopicsFromDB = CourseData.GetListCourseTopicDtoFromDataBase();
            var topicsInDB = CourseData.GetTopics();
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId)).Returns(courseTopicsFromDB);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate));
            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsInDB);
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            var sut = new CourseService(_topicRepositoryMock.Object, 
                                        _courseRepositoryMock.Object, 
                                        _taskRepositoryMock.Object, 
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));
            //When
            sut.UpdateCourseTopicsByCourseId(givenCourseId, givenTopicsToUpdate);
            //Then
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseId(givenCourseId), Times.Never);
            _courseRepositoryMock.Verify(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate), Times.Once);

        }
        [Test]
        public void UpdateCourseTopicsByCourseId_WhenCountOfTopicsIsChanged_ThenDeleteAndInsertMethodsCalled()
        {
            //Given
            var givenCourseId = 7;
            var givenCourseTopicsToUpdate = new List<CourseTopicDto>();
            givenCourseTopicsToUpdate.Add(new CourseTopicDto { Position = 1, Id = 8, Topic = new TopicDto { Id = 8 } });
            givenCourseTopicsToUpdate.Add(new CourseTopicDto { Position = 3, Id = 6, Topic = new TopicDto { Id = 6 } });
            givenCourseTopicsToUpdate.Add(new CourseTopicDto { Position = 6, Id = 9, Topic = new TopicDto { Id = 9 } });
            givenCourseTopicsToUpdate.Add(new CourseTopicDto { Position = 8, Id = 2, Topic = new TopicDto { Id = 2 } });

            var topicsInDB = CourseData.GetTopics();
            var courseToicsFromDB = CourseData.GetListCourseTopicDtoFromDataBase();
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId)).Returns(courseToicsFromDB);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenCourseTopicsToUpdate));
            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsInDB);
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            var sut = new CourseService(_topicRepositoryMock.Object, 
                                        _courseRepositoryMock.Object, 
                                        _taskRepositoryMock.Object, 
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));
           
            //When
            sut.UpdateCourseTopicsByCourseId(givenCourseId, givenCourseTopicsToUpdate);
            //Then
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseId(givenCourseId), Times.Once);
            _topicRepositoryMock.Verify(x => x.AddTopicsToCourse(givenCourseTopicsToUpdate), Times.Once);
        }
        [Test]
        public void UpdateCourseTopicsByCourseId_TopicsInDatabaseAreAbsentForCourse_AddedTopicsForCourse()
        {
            //Given
            var givenCourseId = 3;
            var givenTopicsToUpdate = CourseData.GetListCourseTopicDto();
            var courseToicsFromDB = new List<CourseTopicDto>();
            List<TopicDto> topicsInDB = CourseData.GetTopics();
            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsInDB);
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId)).Returns(courseToicsFromDB);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate));
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            var sut = new CourseService(_topicRepositoryMock.Object, 
                                        _courseRepositoryMock.Object, 
                                        _taskRepositoryMock.Object, 
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));
            //When
            sut.UpdateCourseTopicsByCourseId(givenCourseId, givenTopicsToUpdate);
            //Then
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseId(givenCourseId), Times.Never);
            _topicRepositoryMock.Verify(x => x.AddTopicsToCourse(givenTopicsToUpdate), Times.Once);
        }
        [Test]
        public void UpdateCourseTopicsByCourseId_WhenTopicsToUpdateNotProvided_ThenUpdateTerminates()
        {
            //Given
            var givenCourseId = 3;
            var givenTopicsToUpdate = new List<CourseTopicDto>();
            var toicsFromDB = CourseData.GetListCourseTopicDtoFromDataBase();
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId)).Returns(toicsFromDB);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate));
            var sut = new CourseService(_topicRepositoryMock.Object, 
                                        _courseRepositoryMock.Object, 
                                        _taskRepositoryMock.Object, 
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));
            //When
            sut.UpdateCourseTopicsByCourseId(givenCourseId, givenTopicsToUpdate);
            //Then
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseId(givenCourseId), Times.Never);
            _courseRepositoryMock.Verify(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate), Times.Never);
        }
        [Test]
        public void UpdateCourseTopicsByCourseId_WhenPositionsAreNotUnique_ValidationExceptionThrown()
        {
            //Given
            var givenCourseId = 3;
            var givenTopicsToUpdate = new List<CourseTopicDto>();
            var topicsDto = CourseData.GetTopics();

            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 4, Id = 1, Topic = new TopicDto { Id = 1 } });
            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 4, Id = 2, Topic = new TopicDto { Id = 2 } });
            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 1, Id = 3, Topic = new TopicDto { Id = 3 } });

            var toicsFromDB = CourseData.GetListCourseTopicDtoFromDataBase();
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId)).Returns(toicsFromDB);
            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsDto);

            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate));
            var sut = new CourseService(_topicRepositoryMock.Object, 
                                        _courseRepositoryMock.Object, 
                                        _taskRepositoryMock.Object, 
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));
            //When
            var exception = Assert.Throws<ValidationException>(() => 
            sut.UpdateCourseTopicsByCourseId(givenCourseId, givenTopicsToUpdate));
            //Then
            Assert.That(exception.Message, Is.EqualTo(ServiceMessages.SamePositionsInCourseTopics));
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseId(givenCourseId), Times.Never);
            _courseRepositoryMock.Verify(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate), Times.Never);
            
        }
        [Test]
        public void UpdateCourseTopicsByCourseId_WhenTopicsAreNotUnique_ValidationExceptionThrown()
        {
            var givenCourseId = 3;
            var givenTopicsToUpdate = new List<CourseTopicDto>();

            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 4, Id = 15, Topic = new TopicDto { Id = 15 } });
            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 3, Id = 21, Topic = new TopicDto { Id = 21 } });
            givenTopicsToUpdate.Add(new CourseTopicDto { Position = 1, Id = 15, Topic = new TopicDto { Id = 15 } });
            List<TopicDto> topicsDto = new List<TopicDto>();

            topicsDto.Add(new TopicDto { Id = 15 });
            topicsDto.Add(new TopicDto { Id = 21 });
            topicsDto.Add(new TopicDto { Id = 15 });

            var toicsFromDB = CourseData.GetListCourseTopicDtoFromDataBase();
            _courseValidationHelperMock.Setup(x => x.CheckCourseExistence(givenCourseId));
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsDto);
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId)).Returns(toicsFromDB);
            _courseRepositoryMock.Setup(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate));
            var sut = new CourseService(_topicRepositoryMock.Object, 
                                        _courseRepositoryMock.Object, 
                                        _taskRepositoryMock.Object, 
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));
            //When
            var exception = Assert.Throws<ValidationException>(() => 
            sut.UpdateCourseTopicsByCourseId(givenCourseId, givenTopicsToUpdate));
            //Then
            Assert.That(exception.Message, Is.EqualTo(ServiceMessages.SameTopicsInCourseTopics));
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseId(givenCourseId), Times.Never);
            _courseRepositoryMock.Verify(x => x.UpdateCourseTopicsByCourseId(givenTopicsToUpdate), Times.Never);
        }
        [Test]
        public void SelectAllTopicsByCourseId_CourseIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 0;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId);
            var sut = new CourseService(_topicRepositoryMock.Object,
                                        _courseRepositoryMock.Object,
                                        _taskRepositoryMock.Object,
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));

            _courseValidationHelperMock.Setup(x => x.CheckCourseExistence(givenCourseId));
            _courseRepositoryMock.Setup(x => x.SelectAllTopicsByCourseId(givenCourseId));
            //When
            var exception = Assert.Throws<EntityNotFoundException>(() =>
            sut.SelectAllTopicsByCourseId(givenCourseId));
            //Then
            Assert.That(exception.Message, Is.EqualTo(string.Format(ServiceMessages.EntityNotFoundMessage,"course", givenCourseId)));
            _courseRepositoryMock.Verify(x => x.SelectAllTopicsByCourseId(givenCourseId), Times.Never);

        }

        [Test]
        public void AddTopicToCourse_CourseIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId =3;
            var givenTopicId = 0;
            CourseTopicDto topic = default;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId);
            var sut = new CourseService(_topicRepositoryMock.Object,
                                        _courseRepositoryMock.Object,
                                        _taskRepositoryMock.Object,
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));
            _courseValidationHelperMock.Setup(x => x.CheckCourseExistence(givenCourseId));
            _topicRepositoryMock.Setup(x => x.AddTopicToCourse(topic));
            //When
            var exception = Assert.Throws<EntityNotFoundException>(() =>
            sut.AddTopicToCourse(givenCourseId, givenTopicId,topic));
            //Then
            Assert.That(exception.Message, Is.EqualTo(exp));
            _topicRepositoryMock.Verify(x => x.AddTopicToCourse(topic), Times.Never);
        }
        [Test]
        public void AddTopicToCourse_TopicIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 3;
            var givenTopicId = 0;
            CourseTopicDto topic = default;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "topic", givenTopicId);
            var sut = new CourseService(_topicRepositoryMock.Object,
                                        _courseRepositoryMock.Object,
                                        _taskRepositoryMock.Object,
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            _courseValidationHelperMock.Setup(x => x.CheckCourseExistence(givenCourseId));
            _topicValidationHelperMock.Setup(x => x.CheckTopicExistence(givenTopicId));
            _topicRepositoryMock.Setup(x => x.AddTopicToCourse(topic));
            //When
            var exception = Assert.Throws<EntityNotFoundException>(() =>
            sut.AddTopicToCourse(givenCourseId, givenTopicId, topic));
            //Then
            Assert.That(exception.Message, Is.EqualTo(exp));
            _topicRepositoryMock.Verify(x => x.AddTopicToCourse(topic), Times.Never);
        }
        [Test]
        public void AddTopicsToCourse_TopicIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Give
            var givenCourseId = 2;
            var courrseTopic = CourseData.GetListCourseTopicDto();
            List<TopicDto> topicsInDB = CourseData.GetTopicsFromBDUseWhenTopicAbsent();
            var sut = new CourseService(_topicRepositoryMock.Object,
                                        _courseRepositoryMock.Object,
                                        _taskRepositoryMock.Object,
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            _topicValidationHelperMock.Setup(x => x.CheckTopicsExistence(courrseTopic));
            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsInDB);
            _topicRepositoryMock.Setup(x => x.AddTopicsToCourse(courrseTopic));
            //When
            var exp = Assert.Throws<EntityNotFoundException>(() =>
            sut.AddTopicsToCourse(givenCourseId,courrseTopic));
            //Then
            Assert.That(ServiceMessages.EntityNotFound, Is.EqualTo(exp.Message));
            _topicRepositoryMock.Verify(x => x.AddTopicsToCourse(courrseTopic), Times.Never);

        }
        [Test]
        public void AddTopicsToCourse_CourseIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            var givenCourseId = 2;
            var courrseTopic = CourseData.GetListCourseTopicDto();
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId);
            List<TopicDto> topicsInDB = CourseData.GetTopics();
            var sut = new CourseService(_topicRepositoryMock.Object,
                                        _courseRepositoryMock.Object,
                                        _taskRepositoryMock.Object,
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));
            _topicValidationHelperMock.Setup(x => x.CheckTopicsExistence(courrseTopic));
            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsInDB);
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId));
            _topicRepositoryMock.Setup(x => x.AddTopicsToCourse(courrseTopic));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            sut.AddTopicsToCourse(givenCourseId, courrseTopic));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _topicRepositoryMock.Verify(x => x.AddTopicsToCourse(courrseTopic), Times.Never);
        }
        [Test]
        public void DeleteTopicFromCourse_CourseIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 2;
            var givenTopicId = 3;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId);
            var sut = new CourseService(_topicRepositoryMock.Object,
                                        _courseRepositoryMock.Object,
                                        _taskRepositoryMock.Object,
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId));
            _courseValidationHelperMock.Setup(x => x.CheckCourseExistence(givenCourseId));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            sut.DeleteTopicFromCourse(givenCourseId, givenTopicId));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _topicRepositoryMock.Verify(x => x.DeleteTopicFromCourse(givenCourseId, givenTopicId), Times.Never);
        }
        [Test]
        public void DeleteTopicFromCourse_TopicIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 2;
            var givenTopicId = 3;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "topic", givenTopicId);
            var sut = new CourseService(_topicRepositoryMock.Object,
                                        _courseRepositoryMock.Object,
                                        _taskRepositoryMock.Object,
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto() { Id = givenCourseId });
            _topicValidationHelperMock.Setup(x => x.CheckTopicExistence(givenTopicId));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            sut.DeleteTopicFromCourse(givenCourseId, givenTopicId));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _topicRepositoryMock.Verify(x => x.DeleteTopicFromCourse(givenCourseId, givenTopicId), Times.Never);
        }
        [Test]
        public void UpdateCourseTopicsByCourseId_CourseIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 2;
            var givenCourseTopic = CourseData.GetListCourseTopicDto();
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId);
            var sut = new CourseService(_topicRepositoryMock.Object,
                                        _courseRepositoryMock.Object,
                                        _taskRepositoryMock.Object,
                                        _materialRepositoryMock.Object,
                                        new CourseValidationHelper(_courseRepositoryMock.Object),
                                        new TopicValidationHelper(_topicRepositoryMock.Object));
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId));
            _courseValidationHelperMock.Setup(x => x.CheckCourseExistence(givenCourseId));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            sut.UpdateCourseTopicsByCourseId(givenCourseId, givenCourseTopic));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _courseRepositoryMock.Verify(x => x.UpdateCourseTopicsByCourseId(givenCourseTopic), Times.Never);

        }
        [Test]
        public void UpdateCourseTopicsByCourseId_TopicIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 2;
            var givenCourseTopic = CourseData.GetListCourseTopicDto();
            var topicsInBd = CourseData.GetTopicsFromBDUseWhenTopicAbsent();
            var exp = ServiceMessages.EntityNotFound;
            var sut = new CourseService(_topicRepositoryMock.Object,
                                       _courseRepositoryMock.Object,
                                       _taskRepositoryMock.Object,
                                       _materialRepositoryMock.Object,
                                       new CourseValidationHelper(_courseRepositoryMock.Object),
                                       new TopicValidationHelper(_topicRepositoryMock.Object));
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId)).Returns(new CourseDto { Id = givenCourseId });
            _topicRepositoryMock.Setup(x => x.GetAllTopics()).Returns(topicsInBd);
            _topicValidationHelperMock.Setup(x => x.CheckTopicsExistence(givenCourseTopic));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            sut.UpdateCourseTopicsByCourseId(givenCourseId, givenCourseTopic));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _courseRepositoryMock.Verify(x => x.UpdateCourseTopicsByCourseId(givenCourseTopic), Times.Never);

        }
        [Test]
        public void DeleteAllTopicsByCourseId__CourseIdIsAbsentInDatabase_EntityNotFoundExceptionThrown()
        {
            //Given
            var givenCourseId = 2;
            var exp = string.Format(ServiceMessages.EntityNotFoundMessage, "course", givenCourseId);
            var sut = new CourseService(_topicRepositoryMock.Object,
                                       _courseRepositoryMock.Object,
                                       _taskRepositoryMock.Object,
                                       _materialRepositoryMock.Object,
                                       new CourseValidationHelper(_courseRepositoryMock.Object),
                                       new TopicValidationHelper(_topicRepositoryMock.Object));
            _courseRepositoryMock.Setup(x => x.GetCourse(givenCourseId));
            //When
            var result = Assert.Throws<EntityNotFoundException>(() =>
            sut.DeleteAllTopicsByCourseId(givenCourseId));
            //Then
            Assert.That(result.Message, Is.EqualTo(exp));
            _courseRepositoryMock.Verify(x => x.DeleteAllTopicsByCourseId(givenCourseId), Times.Never);

        }



    }
}
