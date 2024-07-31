using Microsoft.Extensions.Hosting;
using System.Xml.Linq;

namespace UnitOfWork.API.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }

        public List<Post> Posts { get; set; }
    }
}
