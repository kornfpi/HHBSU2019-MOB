using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoREST
{
    public class HomeIssueManager
    {
        IIssueService restService;

        public HomeIssueManager(IIssueService service)
        {
            restService = service;
        }

        public Task<List<HomeIssue>> GetTasksAsync()
        {
            return restService.RefreshDataAsync();
        }

        public Task SaveTaskAsync(HomeIssue item, bool isNewItem = false)
        {
            return restService.SaveTodoItemAsync(item, isNewItem);
        }

        public Task DeleteTaskAsync(HomeIssue item)
        {
            return restService.DeleteTodoItemAsync(item.IssueId);
        }
    }
}

