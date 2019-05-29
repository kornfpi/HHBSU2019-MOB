using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoREST
{
    public interface IIssueService
    {
        Task<List<HomeIssue>> RefreshDataAsync();

        Task SaveTodoItemAsync(HomeIssue item, bool isNewItem);

        Task DeleteTodoItemAsync(string id);
    }
}
