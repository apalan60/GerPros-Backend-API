using System.Text.Json;
using GerPros_Backend_API.Application.Faq.Command;
using GerPros_Backend_API.Domain.Entities;

namespace GerPros_Backend_API.Application.FunctionalTests.Faq.Commands;

using static Testing;

public class CreateFaqTests : BaseTestFixture
{
    
    [Test]
    public async Task ShouldCreateFaq()
    {
        var command = new CreateFaqCommand("分類一", [new CreateOrUpdateFaqItemDto("問題一","答案一")]);
        var faqDto = await SendAsync(command);
        var faq = await FindAsync<FaqCategory>(faqDto.Id);
        faq.Should().NotBeNull();
        faq!.Name.Should().Be(command.CategoryName);
    }

    [Test]
    public async Task ShouldUpdateFaq()
    {
        var command = new CreateFaqCommand("分類一", [new CreateOrUpdateFaqItemDto("問題一","答案一")]);
        var faqDto = await SendAsync(command);
        
        var updateCommand = new UpdateFaqCommand(faqDto.Id, "分類二", [new CreateOrUpdateFaqItemDto("問題二","答案二")]);
        await SendAsync(updateCommand);
        
        var faq = await FindAsync<FaqCategory>(faqDto.Id);
        faq.Should().NotBeNull();
        faq!.Name.Should().Be(updateCommand.CategoryName);
    }
    
    [Test]
    public async Task ShouldRemoveFaqItemByUpdate()
    {
        var command = new CreateFaqCommand("分類一", [new CreateOrUpdateFaqItemDto("問題一","答案一")]);
        var faqDto = await SendAsync(command);
        
        var updateCommand = new UpdateFaqCommand(faqDto.Id, "分類一", []);
        await SendAsync(updateCommand);
        
        var faq = await FindAsync<FaqCategory>(faqDto.Id);
        faq.Should().NotBeNull();
        faq!.FaqItems.Should().BeEmpty();
    }
}
