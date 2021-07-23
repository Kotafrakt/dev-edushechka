﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        private const string _commentAddProcedure = "dbo.Comment_Insert";
        private const string _commentDeleteProcedure = "dbo.Comment_Delete";
        private const string _commentSelectByIdProcedure = "dbo.Comment_SelectById";
        private const string _commentSelectAllByUserIdProcedure = "dbo.Comment_SelectAllByUserId";
        private const string _commentUpdateProcedure = "dbo.Comment_Update";
        private const string _commentsFromLessonSelectByLessonIdProcedure = "dbo.Comment_SelectByLessonId";

        public CommentRepository() { }

        public int AddComment(CommentDto commentDto)
        {
            return _connection.QuerySingle<int>(
                _commentAddProcedure,
                new
                {
                    userId = commentDto.User.Id,
                    commentDto.Text
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteComment(int id)
        {
            _connection.Execute(
                _commentDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public CommentDto GetComment(int id)
        {
            CommentDto result = default;
            return _connection
                .Query<CommentDto, UserDto, Role, CommentDto>(
                    _commentSelectByIdProcedure,
                    (comment, user, role) =>
                    {
                        if (result == null)
                        {
                            result = comment;
                            result.User = user;
                            result.User.Roles = new List<Role> { role };
                        }
                        else
                        {
                            result.User.Roles.Add(role);
                        }
                        return result;
                    },
                    new { id },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .FirstOrDefault();
        }

        public List<CommentDto> GetCommentsByUser(int userId)
        {
            var commentDictionary = new Dictionary<int, CommentDto>();
            CommentDto result;

            return _connection
                .Query<CommentDto, UserDto, Role, CommentDto>(
                    _commentSelectAllByUserIdProcedure,
                    (comment, user, role) =>
                    {

                        if (!commentDictionary.TryGetValue(comment.Id, out result))
                        {
                            result = comment;
                            result.User = user;
                            result.User.Roles = new List<Role> { role };
                            commentDictionary.Add(comment.Id, result);
                        }
                        else
                        {
                            result.User.Roles.Add(role);
                        }

                        return result;
                    },
                    new { userId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .Distinct()
                .ToList();
        }

        public void UpdateComment(CommentDto commentDto)
        {
            _connection.Execute(
                _commentUpdateProcedure,
                new
                {
                    commentDto.Id,
                    commentDto.Text
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<CommentDto> SelectCommentsFromLessonByLessonId(int lessonId)
        {
            return _connection
                .Query<CommentDto>(
                    _commentsFromLessonSelectByLessonIdProcedure,
                    new { lessonId },
                    commandType: CommandType.StoredProcedure
                )
                .Distinct()
                .ToList();
        }
    }
}