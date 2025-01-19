using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IApplicationGradeRepository
    {
        ApplicationGrade Get(int id);  // Dohvatanje entiteta ApplicationGrade po ID-ju
        IEnumerable<ApplicationGrade> GetAll();  // Dohvatanje svih ocena
        IEnumerable<ApplicationGrade> GetByUserId(long userId);  // Dohvatanje ocena po ID-ju korisnika
        void Add(ApplicationGrade applicationGrade);  // Dodavanje nove ocene
        void Update(ApplicationGrade applicationGrade);  // Ažuriranje ocene
        void Delete(int id);  // Brisanje ocene po ID-ju
    }
}
