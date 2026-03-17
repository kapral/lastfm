using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IF.Lastfm.Core.Api.Commands.Tag;
using IF.Lastfm.Core.Api.Enums;
using IF.Lastfm.Core.Objects;
using IF.Lastfm.Core.Tests.Resources;
using NUnit.Framework;

namespace IF.Lastfm.Core.Tests.Api.Commands.Tag
{
    public class GetInfoCommandTests: CommandTestsBase
    {
        [Test]
        public async Task HandleSuccessResponse()
        {
            //Arrange
            const string tagName = "disco";
            const string tagUri = "http://www.last.fm/tag/disco";

            var command = new GetInfoCommand(MAuth.Object, tagName);
            var expectedTag=new LastTag(tagName,tagUri)
            {
                Reach = 34671,
                Count = 172224,
                Streamable = true
            };


            //Act
            var file = GetFileContents("Tag.GetInfoSuccess.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TagApiResponses.GetInfoSuccess));
            var lastResponse = await command.HandleResponse(response);
            var tag = lastResponse.Content;

            //Assert
            Assert.That(lastResponse.Success);
            Assert.That(tag.Reach, Is.EqualTo(expectedTag.Reach));
            Assert.That(tag.Name, Is.EqualTo(expectedTag.Name));
            Assert.That(tag.Count, Is.EqualTo(expectedTag.Count));
            Assert.That(tag.Streamable, Is.EqualTo(expectedTag.Streamable));
        }

        [Test]
        public async Task HandleErrorResponse()
        {
            var command = new GetInfoCommand(MAuth.Object, "errorTag");

            var file = GetFileContents("Tag.GetInfoError.json");
            var response = CreateResponseMessage(file);
            //var response = CreateResponseMessage(Encoding.UTF8.GetString(TagApiResponses.GetInfoError));

            var parsed = await command.HandleResponse(response);

            Assert.That(parsed.Success, Is.False);
            Assert.That(parsed.Status, Is.EqualTo(LastResponseStatus.MissingParameters));
        }
    }
}
