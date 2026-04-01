namespace TaskFlow.DAL.Entities;

public enum TaskStatus
{
    Todo = 0,
    InProgress = 1,
    Done = 2,
    Blocked = 3
}

public enum TaskCategory
{
    General = 0,
    Bug = 1,
    Feature = 2,
    Chore = 3
}
