using NSwag.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elastic.Kibana.Serilog.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerIgnore]
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Genre { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
