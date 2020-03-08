using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LinkShorter.Models.UrlStatistics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LinkShorter.Models
{
    public class Link
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [DisplayName("Short URL")]
        public string ShortUrl { get; set; }

        [DisplayName("Link")]
        [Required]
        [DataType(DataType.Url)]
        public string RedirectUrl { get; set; }

        
        public ICollection<UrlStatistic> UrlStatistics { get; set; }

        public IdentityUser AdOwner { get; set; }

    }
}
