using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerEmailTest
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
                    var emails = new StreamReader("C:\\email\\emails.txt");
                    mail.From = new MailAddress("email");
                    foreach (var email in emails.ReadLine().Split(','))
                    {
                        mail.To.Add(email);
                    }
                    mail.Subject = "ASSUNTO DO EMAIL";
                    mail.IsBodyHtml = true;

                    var body = File.ReadAllText("C:\\email\\email.html");
                    body = body.Replace("@Teste", "Willyam Santos");

                    mail.Body = body;


                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("email", "senha");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);

                    _logger.LogInformation("Sen mail: {time}", DateTimeOffset.Now);

                }
                catch(Exception e)
                {

                    _logger.LogError("Error application : {error}",e);
                }
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
