using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.Common.Models;
using GerPros_Backend_API.Application.ContactUsForm;
using MimeKit;

namespace GerPros_Backend_API.Infrastructure.Mails;

public class AwsSesEmailService : IEmailService
{
    private readonly string _regionName;
    private readonly string _verifiedSenderEmail;

    public AwsSesEmailService(string regionName,
        string verifiedSenderEmail)
    {
        _regionName = regionName;
        RegionEndpoint.GetBySystemName(regionName);
        _verifiedSenderEmail = verifiedSenderEmail;
    }

    public async Task SendContactFormAsync(ContactForm formData, UploadedFile[]? files)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress("Web Contact", _verifiedSenderEmail));
        message.To.Add(new MailboxAddress("Admin", _verifiedSenderEmail));

        message.Subject = $"聯絡我們-表單內容 - {formData.LastName}{formData.FirstName}";
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $"""
                        <html>
                        <body>
                            <h2>聯絡我們表單內容</h2>
                            <table border='1' cellpadding='5' cellspacing='0'>
                                <tr><th>姓</th><td>{formData.LastName}</td></tr>
                                <tr><th>名</th><td>{formData.FirstName}</td></tr>
                                <tr><th>稱謂</th><td>{formData.CustomerTitle}</td></tr>
                                <tr><th>公司名稱</th><td>{formData.CompanyName}</td></tr>
                                <tr><th>聯絡電話</th><td>{formData.Phone}</td></tr>
                                <tr><th>E-mail</th><td>{formData.Email}</td></tr>
                                <tr><th>身份</th><td>{formData.CustomerType}</td></tr>
                                <tr><th>可聯絡時間</th><td>{formData.ContactTime}</td></tr>
                                <tr><th>地址</th><td>{formData.ZipCode} {formData.Country} {formData.Area} {formData.Address}</td></tr>
                                <tr><th>服務種類</th><td>{formData.ServiceType}</td></tr>
                                <tr><th>其他服務</th><td>{formData.OtherServiceType}</td></tr>
                                <tr><th>如何得知</th><td>{formData.KnowMethod}</td></tr>
                                <tr><th>其他得知方式</th><td>{formData.OtherKnowMethod}</td></tr>
                                <tr><th>留言</th><td>{formData.Comment}</td></tr>
                            </table>
                            <br/>
                            <p style='color:gray; font-size: 0.9em;'>此郵件為系統自動發送，請勿直接回覆。</p>
                        </body>
                        </html>
                        """
        };

        if (files != null)
        {
            foreach (var file in files)
            {
                await bodyBuilder.Attachments.AddAsync(
                    fileName: file.FileName,
                    stream: file.Content,
                    contentType: ContentType.Parse(file.ContentType)
                );
            }
        }

        message.Body = bodyBuilder.ToMessageBody();

        using var ms = new MemoryStream();
        await message.WriteToAsync(ms);
        ms.Position = 0;

        using var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.GetBySystemName(_regionName));

        var sendRequest = new SendRawEmailRequest { RawMessage = new RawMessage(ms) };

        await client.SendRawEmailAsync(sendRequest);
    }
}
