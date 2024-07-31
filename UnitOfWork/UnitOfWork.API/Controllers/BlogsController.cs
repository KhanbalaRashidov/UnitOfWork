using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnitOfWork.API.Entities;
using UnitOfWork.Interfaces;

namespace UnitOfWork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ILogger<BlogsController> _logger;

        public BlogsController(IUnitOfWork unitOfWork, ILogger<BlogsController> logger)
        {
            _unitOfWork= unitOfWork;
            _logger= logger;

            // seeding
            var repo = _unitOfWork.GetRepository<Blog>(hasCustomRepository: true);
            if (repo.Count() == 0)
            {
                repo.Insert(new Blog
                {
                    Id = 1,
                    Url = "/a/" + 1,
                    Title = $"a{1}",
                    Posts = new List<Post>{
                                new Post
                                {
                                    Id = 1,
                                    Title = "A",
                                    Content = "A's content",
                                    Comments = new List<Comment>
                                    {
                                        new Comment
                                        {
                                            Id = 1,
                                            Title = "A",
                                            Content = "A's content",
                                        },
                                        new Comment
                                        {
                                            Id = 2,
                                            Title = "b",
                                            Content = "b's content",
                                        },
                                        new Comment
                                        {
                                            Id = 3,
                                            Title = "c",
                                            Content = "c's content",
                                        }
                                    },
                                },
                                new Post
                                {
                                    Id = 2,
                                    Title = "B",
                                    Content = "B's content",
                                    Comments = new List<Comment>
                                    {
                                        new Comment
                                        {
                                            Id = 4,
                                            Title = "A",
                                            Content = "A's content",
                                        },
                                        new Comment
                                        {
                                            Id = 5,
                                            Title = "b",
                                            Content = "b's content",
                                        },
                                        new Comment
                                        {
                                            Id = 6,
                                            Title = "c",
                                            Content = "c's content",
                                        }
                                    },
                                },
                                new Post
                                {
                                    Id = 3,
                                    Title = "C",
                                    Content = "C's content",
                                    Comments = new List<Comment>
                                    {
                                        new Comment
                                        {
                                            Id = 7,
                                            Title = "A",
                                            Content = "A's content",
                                        },
                                        new Comment
                                        {
                                            Id = 8,
                                            Title = "b",
                                            Content = "b's content",
                                        },
                                        new Comment
                                        {
                                            Id = 9,
                                            Title = "c",
                                            Content = "c's content",
                                        }
                                    },
                                },
                                new Post
                                {
                                    Id = 4,
                                    Title = "D",
                                    Content = "D's content",
                                    Comments = new List<Comment>
                                    {
                                        new Comment
                                        {
                                            Id = 10,
                                            Title = "A",
                                            Content = "A's content",
                                        },
                                        new Comment
                                        {
                                            Id = 11,
                                            Title = "b",
                                            Content = "b's content",
                                        },
                                        new Comment
                                        {
                                            Id = 12,
                                            Title = "c",
                                            Content = "c's content",
                                        }
                                    },
                                }
                            },
                });
                _unitOfWork.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<IList<Blog>> Get() =>
            await _unitOfWork.GetRepository<Blog>()
                              .GetAllAsync(include: source => source.Include(blog => blog.Posts)
                                 .ThenInclude(post => post.Comments));

        [HttpGet("page/{pageIndex}/{pageSize}")]
        public async Task<IPagedList<Blog>> Get(int pageIndex, int pageSize)
        {
            // projection
            var items = _unitOfWork.GetRepository<Blog>().GetPagedList(b => new { Name = b.Title, Link = b.Url });

            return await _unitOfWork.GetRepository<Blog>().GetPagedListAsync(pageIndex: pageIndex, pageSize: pageSize);
        }

        [HttpGet("search/{term}")]
        public async Task<IPagedList<Blog>> Get(string term)
        {
            _logger.LogInformation("demo about first or default with include");

            var item = _unitOfWork.GetRepository<Blog>().GetFirstOrDefault(predicate: x => x.Title.Contains(term), include: source => source.Include(blog => blog.Posts).ThenInclude(post => post.Comments));

            _logger.LogInformation("demo about first or default without include");

            item = _unitOfWork.GetRepository<Blog>().GetFirstOrDefault(predicate: x => x.Title.Contains(term), orderBy: source => source.OrderByDescending(b => b.Id));

            _logger.LogInformation("demo about first or default with projection");

            var projection = _unitOfWork.GetRepository<Blog>().GetFirstOrDefault(b => new { Name = b.Title, Link = b.Url }, predicate: x => x.Title.Contains(term));

            return await _unitOfWork.GetRepository<Blog>().GetPagedListAsync(predicate: x => x.Title.Contains(term));
        }

        [HttpGet("{id}")]
        public async Task<Blog> Get(int id) => await _unitOfWork.GetRepository<Blog>().FindAsync(id);

        [HttpPost]
        public async Task Post([FromBody] Blog value)
        {
            var repo = _unitOfWork.GetRepository<Blog>();
            repo.Insert(value);
            await _unitOfWork.SaveChangesAsync();
        }

        [HttpPut]
        public async Task Put([FromBody] Blog blog)
        {
            var repo= _unitOfWork.GetRepository<Blog>();
            repo.Update(blog);
            await _unitOfWork.SaveChangesAsync();
        }

        [HttpDelete]
        public async Task Delete(int id)
        {
            var repo = _unitOfWork.GetRepository<Blog>();
            repo.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
