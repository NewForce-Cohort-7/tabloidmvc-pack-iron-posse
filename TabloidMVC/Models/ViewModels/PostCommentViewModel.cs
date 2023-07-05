using System.Collections.Generic;
using System.Linq;

namespace TabloidMVC.Models.ViewModels
{ public class PostCommentViewModel
	{
		public Post Post { get; set; }
        public Comment Comment { get; set; }
        public List<Comment> Comments { get; set; }
        public UserProfile UserProfile { get; set; }
 
    }
      
}