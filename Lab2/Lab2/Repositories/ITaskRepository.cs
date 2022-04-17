﻿using Lab2.Models;
using System.Collections.Generic;

namespace Lab2.Repositories
{
    public interface ITaskRepository
    {
        int AddTask(TaskList task);
        List<TaskList> GetTasks();
        void RemoveAllTasks();
        void SaveFile();
        int RemoveTask(int id);
    }
}
