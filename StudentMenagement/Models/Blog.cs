using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentMenagement.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string BloggerName { get; set; }
        public virtual BlogImage blogImage { get; set; }
        public virtual List<Post> Posts { get; set; }
    }

    public class BlogImage
    {
        public int BlogImageId { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int BId { get; set; }
        [ForeignKey("BId")]
        public virtual Blog Blog { get; set; }
    }


    //[Table(name: "Blogs")]
    //public class Blog
    //{
    //    [Key]
    //    public int Id { get; set; }
    //    [Column(ColumnName = "BlogTitle")]
    //    [StringLength(50, MinimumLength = 3)]
    //    public string Title { get; set; }
    //    public string BloggerName { get; set; }

    //}
}
