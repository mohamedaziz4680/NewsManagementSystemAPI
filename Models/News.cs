using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        [Required]
        public string Content { get; set; }
        public byte[] ImageData { get; set; }
        [Required]
        public DateTime PublicationDate { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
    }
}
