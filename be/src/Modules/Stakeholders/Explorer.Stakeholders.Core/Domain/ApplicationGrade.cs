using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain
{
    public class ApplicationGrade : Entity
    {
        public int Id { get; init; }  // Ostavljen Id polje,ako nam bude trebalo za kasnije
        public int Rating { get; init; }  // Ocena (1-5)
        public string? Comment { get; init; }  // Opciono polje za komentar
        public DateTime Created { get; init; }  // Datum i vreme kreiranja ocene
        public long UserId { get; init; }  // ID korisnika koji je postavio ocenu

        // Konstruktor sa validacijom
        public ApplicationGrade(int rating, string? comment, long userId)
        {
            if (rating < 1 || rating > 5) throw new ArgumentException("Invalid rating"); // Validacija ocene

            Rating = rating;

            // Postavljanje komentara ili zadate vrednosti ako komentar nije dat
            Comment = string.IsNullOrWhiteSpace(comment) ? "No comment added." : comment;

            Created = DateTime.UtcNow;  // Automatski beležimo vreme kreiranja ocene
            UserId = userId;  // Veza sa korisnikom koji je dao ocenu
        }
    }
}
