﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DevEdu.API.Models.InputModels;
using AutoMapper;
using DevEdu.DAL.Models;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using DevEdu.Business.Services;
using DevEdu.API.Models.OutputModels;
using DevEdu.API.Common;
using DevEdu.DAL.Enums;
using DevEdu.API.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace DevEdu.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TopicController : Controller
    {
        private readonly ITopicService _topicService;
        private readonly IMapper _mapper;

        public TopicController(IMapper mapper, ITopicService topicService)
        {
            _topicService = topicService;
            _mapper = mapper;
        }

        //  api/topic/{id}
        [HttpGet("{id}")]
        [Description("Get topic by id")]
        [ProducesResponseType(typeof(TopicOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public TopicOutputModel GetTopicById(int id)
        {
            var output= _topicService.GetTopic(id);
            return _mapper.Map<TopicOutputModel>(output);
        }

        [HttpGet]
        [Description("Get all topics")]
        [ProducesResponseType(typeof(List<TopicOutputModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        public List<TopicOutputModel> GetAllTopics()
        {
            var output = _topicService.GetAllTopics();
            return _mapper.Map<List<TopicOutputModel>>(output);
        }

        //  api/topic
        [AuthorizeRoles(Role.Methodist, Role.Manager)]
        [HttpPost]
        [Description("Add topic")]
        [ProducesResponseType(typeof(TopicOutputModel), (StatusCodes.Status201Created))]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public TopicOutputModel AddTopic([FromBody] TopicInputModel model)
        {
            var dto = _mapper.Map<TopicDto>(model);
            var output = _topicService.AddTopic(dto);
            return GetTopicById(output);
        }

        //  api/topic/{id}
        [AuthorizeRoles(Role.Methodist, Role.Manager)]
        [HttpDelete("{id}")]
        [Description("Delete topic")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public void DeleteTopic(int id)
        {
            _topicService.DeleteTopic(id);
        }

        //  api/topic/{id}
        [AuthorizeRoles(Role.Methodist, Role.Manager)]
        [HttpPut("{id}")]
        [Description("Update topic")]
        [ProducesResponseType(typeof(TopicOutputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationExceptionResponse), StatusCodes.Status422UnprocessableEntity)]
        public TopicOutputModel UpdateTopic(int id, [FromBody] TopicInputModel model)
        {
            var dto = _mapper.Map<TopicDto>(model);           
            _topicService.UpdateTopic(id, dto);
            return GetTopicById(id);
        }

        //  api/topic/{topicId}/tag/{tagId}
        [AuthorizeRoles(Role.Methodist, Role.Teacher)]
        [HttpPost("{topicId}/tag/{tagId}")]
        [Description("Add tag to topic")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public int AddTagToTopic(int topicId, int tagId)
        {
            return _topicService.AddTagToTopic(topicId, tagId);
        }

        //  api/opic/{topicId}/tag/{tagId}
        [AuthorizeRoles(Role.Methodist, Role.Teacher)]
        [HttpDelete("{topicId}/tag/{tagId}")]
        [Description("Delete tag from topic")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
        public int DeleteTagFromTopic(int topicId, int tagId)
        {
            return _topicService.DeleteTagFromTopic(topicId, tagId);
        }
    }
}