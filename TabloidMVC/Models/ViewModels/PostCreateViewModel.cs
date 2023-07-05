//PostCreateViewModel provides the elements needed for the Edit Post page so users can see information from the Posts and a list of the categories, not just their Ids.


namespace TabloidMVC.Models.ViewModels
{
    public class PostCreateViewModel
    {
        public Post Post { get; set; }
        public List<Category> CategoryOptions { get; set; }

    }
}
