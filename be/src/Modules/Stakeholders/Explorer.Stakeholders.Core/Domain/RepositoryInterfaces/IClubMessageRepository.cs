using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IClubMessageRepository
    {
        public ICollection<ClubMessage> GetAll();
        public ClubMessage Create(ClubMessage message);
        public ClubMessage Update(ClubMessage message);
        public ClubMessage Delete(long clubMessageId);
        public ClubMessage Get(long clubMessageId);
    }
}
