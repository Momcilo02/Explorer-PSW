using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.API.Public
{
    public interface INotificationHandler
    {
        void Notify(int userId, string notificationType);
    }
}
