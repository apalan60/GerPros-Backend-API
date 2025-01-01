using GerPros_Backend_API.Application.Common.Interfaces;
using GerPros_Backend_API.Application.Common.Models;
using GerPros_Backend_API.Application.ContactUsForm;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace GerPros_Backend_API.Infrastructure.Mails;

public class MailKitEmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _gmailAccount;
    private readonly string _gmailPassword;

    public MailKitEmailService(string smtpServer, int smtpPort, string gmailAccount, string gmailPassword)
    {
        _smtpServer = smtpServer;
        _smtpPort = smtpPort;
        _gmailAccount = gmailAccount;
        _gmailPassword = gmailPassword;
    }

    public Task SendContactFormAsync(ContactForm formData, UploadedFile[]? files)
    {
        throw new NotImplementedException();
//         var message = new MimeMessage();
//
//         message.From.Add(new MailboxAddress("Web Contact", _gmailAccount));
//         message.To.Add(new MailboxAddress("Admin", _gmailAccount));
//         message.Subject = $"聯絡我們表單 - {formData.LastName}{formData.FirstName}";
//
//         var bodyBuilder = new BodyBuilder
//         {
//             HtmlBody = $"""
//                         
//                                         <html>
//                                         <body>
//                                             <h2>聯絡我們表單內容</h2>
//                                             <table border='1' cellpadding='5' cellspacing='0'>
//                                                 <tr><th>姓</th><td>{formData.LastName}</td></tr>
//                                                 <tr><th>名</th><td>{formData.FirstName}</td></tr>
//                                                 <tr><th>稱謂</th><td>{formData.CustomerTitle}</td></tr>
//                                                 <tr><th>公司名稱</th><td>{formData.CompanyName}</td></tr>
//                                                 <tr><th>聯絡電話</th><td>{formData.Phone}</td></tr>
//                                                 <tr><th>E-mail</th><td>{formData.Email}</td></tr>
//                                                 <tr><th>身份</th><td>{formData.CustomerType}</td></tr>
//                                                 <tr><th>可聯絡時間</th><td>{formData.ContactTime}</td></tr>
//                                                 <tr><th>地址</th><td>{formData.ZipCode} {formData.Country} {formData.Area} {formData.Address}</td></tr>
//                                                 <tr><th>服務種類</th><td>{formData.ServiceType}</td></tr>
//                                                 <tr><th>其他服務</th><td>{formData.OtherServiceType}</td></tr>
//                                                 <tr><th>如何得知</th><td>{formData.KnowMethod}</td></tr>
//                                                 <tr><th>其他得知方式</th><td>{formData.OtherKnowMethod}</td></tr>
//                                                 <tr><th>留言</th><td>{formData.Comment}</td></tr>
//                                             </table>
//                                             <br/>
//                                             <p style='color:gray; font-size: 0.9em;'>此郵件為系統自動發送，請勿直接回覆。</p>
//                                         </body>
//                                         </html>
//                         """
//         };
//
//         if (files != null)
//         {
//             foreach (var file in files)
//             {
//                 await bodyBuilder.Attachments.AddAsync(file.FileName, file.Content,
//                     ContentType.Parse(file.ContentType));
//             }
//         }
//
//         message.Body = bodyBuilder.ToMessageBody();
//
//         using var smtp = new SmtpClient();
//         await smtp.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
//         await smtp.AuthenticateAsync(_gmailAccount, _gmailPassword);
//         await smtp.SendAsync(message);
//         await smtp.DisconnectAsync(true);
//     }
    }
}




