using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }

        public List<Comment> GetCommentsByPostId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT c.Id, c.PostId, c.UserProfileId, c.Subject, c.Content, c.CreateDateTime,
                               up.DisplayName AS CommenterName
                        FROM Comment c
                        LEFT JOIN UserProfile up ON c.UserProfileId = up.Id
                        WHERE c.PostId = @id
                        ORDER BY c.CreateDateTime DESC";

                    cmd.Parameters.AddWithValue("@id", id);

                    var reader = cmd.ExecuteReader();

                    var comments = new List<Comment>();

                    while (reader.Read())
                    {
                        // Create a new Comment object and populate its properties
                        Comment comment = new Comment()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            UserProfile = new UserProfile()
                            {
                                DisplayName = reader.GetString(reader.GetOrdinal("CommenterName"))
                            }
                      
                        };

                        comments.Add(comment);
                    }

                    reader.Close();

                    return comments;
                }
            }
        }

        

        public Comment GetCommentById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT c.Id, c.PostId, c.UserProfileId, c.Subject, c.Content, c.CreateDateTime,
                               up.DisplayName AS CommenterName
                        FROM Comment c
                        LEFT JOIN UserProfile up ON c.UserProfileId = up.Id
                        WHERE c.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    var reader = cmd.ExecuteReader();

                    Comment comment = null;

                    if (reader.Read())
                    {
                        // Create a new Comment object and populate its properties
                        comment = new Comment()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            UserProfile = new UserProfile()
                            {
                                DisplayName = reader.GetString(reader.GetOrdinal("CommenterName"))
                            }
                        };
                    }

                    reader.Close();

                    return comment;
                }
            }
        }

        public void AddComment(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Comment (PostId, UserProfileId, Subject, Content, CreateDateTime)
                        OUTPUT INSERTED.ID
                        VALUES (@postId, @userProfileId, @subject, @content, @createDateTime)";

                    cmd.Parameters.AddWithValue("@postId", comment.PostId);
                    cmd.Parameters.AddWithValue("@userProfileId", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@content", comment.Content);
                    cmd.Parameters.AddWithValue("@createDateTime", comment.CreateDateTime);

                    comment.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void DeleteComment(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Comment WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateComment(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Comment
                        SET PostId = @postId,
                            UserProfileId = @userProfileId,
                            Subject = @subject,
                            Content = @content,
                            CreateDateTime = @createDateTime
                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@postId", comment.PostId);
                    cmd.Parameters.AddWithValue("@userProfileId", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@content", comment.Content);
                    cmd.Parameters.AddWithValue("@createDateTime", comment.CreateDateTime);
                    cmd.Parameters.AddWithValue("@id", comment.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        //public Comment GetUserDisplayName(int id)
        //{
        //    using (var conn = Connection)
        //    {
        //        conn.Open();
        //        using (var cmd = conn.CreateCommand())
        //        {
              

        //            cmd.CommandText = @"
        //                SELECT up.DisplayName AS CommenterName
        //                FROM Comment c
        //                LEFT JOIN UserProfile up ON c.UserProfileId = up.Id
        //                WHERE c.Id = @id";

        //            cmd.Parameters.AddWithValue("@id", id);

        //            var reader = cmd.ExecuteReader();

        //            Comment comment = null;

        //            if (reader.Read())
        //            {
        //                // Create a new Comment object and populate its properties
        //                comment = new Comment()
        //                {
        //                     Id = reader.GetInt32(reader.GetOrdinal("Id")),
        //                     PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
        //                     UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
        //                     Subject = reader.GetString(reader.GetOrdinal("Subject")),
        //                     Content = reader.GetString(reader.GetOrdinal("Content")),
        //                    CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
        //                    UserProfile = new UserProfile()
        //                    {
        //                        DisplayName = reader.GetString(reader.GetOrdinal("CommenterName"))
        //                    }
        //                };
        //            }

        //            reader.Close();

        //            return comment;
        //        }
        //    }
        //}
    }
}
