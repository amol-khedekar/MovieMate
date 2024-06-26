﻿using System.ComponentModel.DataAnnotations;

namespace MovieMate.Models
{
    public class MovieModel
    {
        [Key]
        public int movieId { get; set; }

        [Required]
        public string title { get; set; }

        public string imageExtension { get; set; }

        [DataType(DataType.MultilineText)]
        public string? description { get; set; }

        // A Movie can have multiple shows
        public virtual ICollection<ShowModel> Shows { get; set; }

    }
}
