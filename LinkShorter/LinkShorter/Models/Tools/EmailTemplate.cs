using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LinkShorter.Models.Tools
{
    public class EmailTemplate
    {
        private readonly ILogger _logger;


        public EmailTemplate(ILogger<EmailTemplate> logger)
        {
            _logger = logger;
        }

        public async Task<string> generateMailBody(string emailTemplatePath, ListDictionary replacements)
        {
            //get email template content
            _logger.LogDebug("Starting reading email template from path: {0}", emailTemplatePath);
            string templateContent = await File.ReadAllTextAsync(emailTemplatePath);

            _logger.LogDebug("Starting changing values in email template");
            //replace template content by given values
            foreach (DictionaryEntry replacement in replacements)
            {
                templateContent = templateContent.Replace(replacement.Key.ToString(), replacement.Value.ToString());
            }
            _logger.LogDebug("Finished creating mail body.");
            //return email template with replaced values
            return templateContent;
        }
    }
}
