namespace Blog.ViewModels;

public class ResultViewModel<Generic>
{
    public Generic Data { get; private set; }
    public List<string> Errors { get; private set; } = new();
   
    public ResultViewModel(Generic data, List<string> errors)
    {  
        Data = data;
        Errors = errors;
    }
    public ResultViewModel(Generic data)
    {
        Data = data;
    }
    public ResultViewModel(List<string> errors)
    {
        Errors = errors;
    }
    public ResultViewModel(string error)
    {
        Errors.Add(error);
    }
}