using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Opw.PineBlog.Entities;
using Opw.PineBlog.Files;
using Opw.PineBlog.GitDb;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Opw.PineBlog.Posts.Search
{
    public class SearchPostsQueryTests : GitDbTestsBase
    {
        private readonly SearchPostsQuery.Handler searchPostsQueryHandler;

        public SearchPostsQueryTests(GitDbFixture fixture) : base(fixture)
        {
            var uow = ServiceProvider.GetRequiredService<IBlogUnitOfWork>();
            var options = ServiceProvider.GetRequiredService<IOptionsSnapshot<PineBlogOptions>>();
            var postUrlHelper = ServiceProvider.GetRequiredService<PostUrlHelper>();
            var fileUrlHelper = ServiceProvider.GetRequiredService<FileUrlHelper>();

            var postRankerMock = new Mock<IPostRanker>();
            postRankerMock
                .Setup(m => m.Rank(It.IsAny<IEnumerable<Post>>(), It.IsAny<string>()))
                .Returns((IEnumerable<Post> posts, string _) => posts);

            searchPostsQueryHandler = new SearchPostsQuery.Handler(uow, postRankerMock.Object, options, postUrlHelper, fileUrlHelper);
        }

        [Fact]
        public async Task Handler_Should_Return6Posts_MatchOnTitle()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "title", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(6);
        }

        [Fact]
        public async Task Handler_Should_Return1Post_MatchOnTitle()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "title1", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handler_Should_Return6Post_MatchOnCategories()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "categories", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(6);
        }

        [Fact]
        public async Task Handler_Should_Return2Post_MatchOnCategories()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "catc", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handler_Should_Return6Post_MatchOnDescription()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "description", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(6);
        }

        [Fact]
        public async Task Handler_Should_Return1Post_MatchOnDescription()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "description1", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handler_Should_Return6Post_MatchOnContent()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "content", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(6);
        }

        [Fact]
        public async Task Handler_Should_Return1Post_MatchOnContent()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "content1", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handler_Should_Return2Post_MultipleTerms_MatchOnTitle()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "title1 title2", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handler_Should_Return2Post_MultipleTerms_MatchOnTitleAndDescription()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "title1 description2", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handler_Should_Return3Post_MultipleTerms_MatchOnTitleAndCategories()
        {
            var result = await searchPostsQueryHandler.Handle(new SearchPostsQuery { Page = 1, SearchQuery = "title1 catc", ItemsPerPage = 100 }, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            result.Value.Posts.Should().HaveCount(3);
        }
    }
}
