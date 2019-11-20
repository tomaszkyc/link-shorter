using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LinkShorter.Models
{
    public class Ad
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }


        public string ShortUrl { get; set; }

        [DisplayName("link")]
        [Required]
        [DataType(DataType.Url)]
        public string RedirectUrl { get; set; }

    }
}
