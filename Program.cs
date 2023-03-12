using System;
using System.Collections.Generic;
using System.IO;

class Goal
{
    public string Name { get; set; }
    public int Value { get; set; }
    public bool Completed { get; set; }

    public Goal(string name, int value)
    {
        Name = name;
        Value = value;
        Completed = false;
    }

    public void MarkCompleted()
    {
        Completed = true;
    }
}

class SimpleGoal : Goal
{
    public SimpleGoal(string name, int value) : base(name, value) { }
}

class EternalGoal : Goal
{
    public EternalGoal(string name, int value) : base(name, value) { }

    public void Record()
    {
        Completed = true;
    }
}

class ChecklistGoal : Goal
{
    public int NumRequired { get; set; }
    public int NumCompleted { get; set; }

    public ChecklistGoal(string name, int value, int numRequired) : base(name, value)
    {
        NumRequired = numRequired;
        NumCompleted = 0;
    }

    public void Record()
    {
        NumCompleted++;
        if (NumCompleted == NumRequired)
        {
            Completed = true;
        }
    }
}

class GoalTracker
{
    private List<Goal> goals;
    public int Score { get; private set; }

    public GoalTracker()
    {
        goals = new List<Goal>();
        Score = 0;
    }

    public void AddGoal(Goal goal)
    {
        goals.Add(goal);
    }

    public void RemoveGoal(Goal goal)
    {
        goals.Remove(goal);
    }

    public void RecordEvent(Goal goal)
    {
        goal.MarkCompleted();
        Score += goal.Value;
        if (goal is ChecklistGoal && goal.Completed)
        {
            Score += goal.Value * 10;
        }
    }

    public void DisplayGoals()
    {
        foreach (Goal goal in goals)
        {
            if (goal is ChecklistGoal)
            {
                ChecklistGoal checklistGoal = (ChecklistGoal)goal;
                Console.Write($"{checklistGoal.Name} Completed {checklistGoal.NumCompleted}/{checklistGoal.NumRequired} times. ");
            }
            else
            {
                Console.Write($"{goal.Name} ");
            }
            Console.WriteLine(goal.Completed ? "[X]" : "[ ]");
        }
        Console.WriteLine($"Score: {Score}");
    }

    public void SaveGoals(string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (Goal goal in goals)
            {
                if (goal is ChecklistGoal)
                {
                    ChecklistGoal checklistGoal = (ChecklistGoal)goal;
                    writer.WriteLine($"ChecklistGoal,{checklistGoal.Name},{checklistGoal.Value},{checklistGoal.NumRequired},{checklistGoal.NumCompleted},{checklistGoal.Completed}");
                }
                else
                {
                    writer.WriteLine($"{goal.GetType().Name},{goal.Name},{goal.Value},{goal.Completed}");
                }
            }
        }
    }

    public void LoadGoals(string filename)
    {
        using (StreamReader reader = new StreamReader(filename))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] fields = line.Split(',');
                string goalType = fields[0];
                string name = fields[1];
                int value = int.Parse(fields[2]);
                bool completed = bool.Parse(fields[3]);
                if (goalType == "ChecklistGoal")
                {
                    int numRequired = int.Parse(fields[4]);
                    int numCompleted = int.Parse(fields[5]);
                    ChecklistGoal goal = new ChecklistGoal(string: name, int: value, int: numRequired);
                    goal.NumCompleted = numCompleted;
                    goal.Completed = completed;
                    goals.Add(goal);
                }
            else if (goalType == "EternalGoal"){
                                       
                EternalGoal goal = new EternalGoal(name, value);
                goal.Completed = completed;
                goals.Add(goal);
            }
            else{
        
                SimpleGoal goal = new SimpleGoal(name, value);
                goal.Completed = completed;
                goals.Add(goal);
                }
            }
        }
    }
}

class Program
                {
                    static void Main(string[] args)
                    {
                        GoalTracker tracker = new GoalTracker();
                        tracker.AddGoal(new SimpleGoal("Run a Marathon", 1000));
                        tracker.AddGoal(new EternalGoal("Read Scriptures", 100));
                        tracker.AddGoal(new ChecklistGoal("Attend the Temple", 50, 10));
                            while (true)
    {
        Console.WriteLine("Enter a command: add, remove, record, display, save, load, or exit.");
        string command = Console.ReadLine().ToLower();
        switch (command)
        {
            case "add":
                Console.WriteLine("Enter the goal name:");
                string name = Console.ReadLine();
                Console.WriteLine("Enter the point value:");
                int value = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter the goal type: simple, eternal, or checklist.");
                string type = Console.ReadLine().ToLower();
                switch (type)
                {
                    case "simple":
                        tracker.AddGoal(new SimpleGoal(name, value));
                        break;
                    case "eternal":
                        tracker.AddGoal(new EternalGoal(name, value));
                        break;
                    case "checklist":
                        Console.WriteLine("Enter the number of times this goal must be completed:");
                        int numRequired = int.Parse(Console.ReadLine());
                        tracker.AddGoal(new ChecklistGoal(name, value, numRequired));
                        break;
                    default:
                        Console.WriteLine("Invalid goal type.");
                        break;
                }
                break;
            case "remove":
                Console.WriteLine("Enter the goal name:");
                name = Console.ReadLine();
                Goal goalToRemove = null;
                foreach (Goal goal in tracker.goals)
                {
                    if (goal.Name == name)
                    {
                        goalToRemove = goal;
                        break;
                    }
                }
                if (goalToRemove != null)
                {
                    tracker.RemoveGoal(goalToRemove);
                }
                else
                {
                    Console.WriteLine("Goal not found.");
                }
                break;
            case "record":
                Console.WriteLine("Enter the goal name:");
                name = Console.ReadLine();
                Goal goalToRecord = null;
                foreach (Goal goal in tracker.goals)
                {
                    if (goal.Name == name)
                    {
                        goalToRecord = goal;
                        break;
                    }
                }
                if (goalToRecord != null)
                {
                    tracker.RecordEvent(goalToRecord);
                }
                else
                {
                    Console.WriteLine("Goal not found.");
                }
                break;
            case "display":
                tracker.DisplayGoals();
                break;
            case "save":
                Console.WriteLine("Enter the filename to save to:");
                string filename = Console.ReadLine();
                tracker.SaveGoals(filename);
                break;
            case "load":
                Console.WriteLine("Enter the filename to load from:");
                filename = Console.ReadLine();
                tracker.LoadGoals(filename);
                break;
            case "exit":
                return;
            default:
                Console.WriteLine("Invalid command.");
                break;
        }
    }
}

