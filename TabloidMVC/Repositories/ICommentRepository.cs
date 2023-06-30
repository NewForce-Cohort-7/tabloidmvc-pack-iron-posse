using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
        public interface ICommentRepository
    {
        List<Comment> GetCommentsByPostId(int id);
        void AddComment(Comment comment);
        void DeleteComment(int id);
        Comment GetCommentById(int id);
        void UpdateComment(Comment comment);
    }
}