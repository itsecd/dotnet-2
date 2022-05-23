using Kanban_board.Model;
using System.Collections.Generic;

namespace Kanban_board.Repositories
{
    public interface IStatusesRepository
    {
        Status AddStatus(Status status);
        Status DeleteStatus(string id);
        Status EditStatus(Status newStatus);
        List<Status> GetAllStatuses();
        Status GetStatusById(string id);
    }
}